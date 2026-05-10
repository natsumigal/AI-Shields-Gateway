using AiGovernance.Core.Abstractions;
using AiGovernance.Core.Models;

namespace AiGovernance.Infrastructure.Services;

public sealed class GovernancePipeline : IGovernancePipeline
{
    private readonly IInputValidator _inputValidator;
    private readonly IRiskAnalyzer _riskAnalyzer;
    private readonly IPolicyEngine _policyEngine;
    private readonly IAiConnector _aiConnector;
    private readonly IOutputGuard _outputGuard;
    private readonly IAuditLogger _auditLogger;

    public GovernancePipeline(
        IInputValidator inputValidator,
        IRiskAnalyzer riskAnalyzer,
        IPolicyEngine policyEngine,
        IAiConnector aiConnector,
        IOutputGuard outputGuard,
        IAuditLogger auditLogger)
    {
        _inputValidator = inputValidator;
        _riskAnalyzer = riskAnalyzer;
        _policyEngine = policyEngine;
        _aiConnector = aiConnector;
        _outputGuard = outputGuard;
        _auditLogger = auditLogger;
    }

    public async Task<GovernanceResponse> ProcessAsync(GovernanceRequest request, CancellationToken cancellationToken)
    {
        var context = CreateContext(request);

        await AuditAsync(context, "RequestReceived", "Request received by governance pipeline.", null, cancellationToken);

        var validationResult = await _inputValidator.ValidateAsync(context, cancellationToken);
        if (!validationResult.IsValid)
        {
            var blockedRisk = BuildBlockedRisk(validationResult);
            await AuditAsync(context, "InputBlocked", "Input validator blocked the request.", blockedRisk, cancellationToken);

            var blockedResponse = new GovernanceResponse
            {
                CorrelationId = context.CorrelationId,
                Decision = GovernanceDecision.Block,
                RiskLevel = blockedRisk.RiskLevel,
                RiskScore = blockedRisk.RiskScore,
                RiskCategory = blockedRisk.Category,
                Message = "Request blocked by input validation.",
                Reasons = blockedRisk.Reasons,
                AppliedControls = validationResult.AppliedControls
            };

            await AuditAsync(context, "ResponseCompleted", blockedResponse.Message, blockedRisk, cancellationToken, blockedResponse.Decision);
            return blockedResponse;
        }

        var riskAssessment = await _riskAnalyzer.AnalyzeAsync(context, validationResult, cancellationToken);
        await AuditAsync(context, "RiskAssessed", "Risk analyzer completed.", riskAssessment, cancellationToken);

        var policyDecision = await _policyEngine.DecideAsync(context, riskAssessment, cancellationToken);
        await AuditAsync(context, "PolicyDecided", policyDecision.Message, riskAssessment, cancellationToken, policyDecision.Decision);

        if (!policyDecision.Decision.Equals(GovernanceDecision.Allow, StringComparison.OrdinalIgnoreCase))
        {
            var stoppedResponse = new GovernanceResponse
            {
                CorrelationId = context.CorrelationId,
                Decision = policyDecision.Decision,
                RiskLevel = riskAssessment.RiskLevel,
                RiskScore = riskAssessment.RiskScore,
                RiskCategory = riskAssessment.Category,
                Message = policyDecision.Message,
                Reasons = policyDecision.Reasons,
                AppliedControls = validationResult.AppliedControls.Concat(policyDecision.AppliedControls).Distinct().ToList()
            };

            await AuditAsync(context, "ResponseCompleted", stoppedResponse.Message, riskAssessment, cancellationToken, stoppedResponse.Decision);
            return stoppedResponse;
        }

        var aiResponse = await _aiConnector.GenerateAsync(context, cancellationToken);
        await AuditAsync(context, "AiConnectorCalled", "AI connector returned a response.", riskAssessment, cancellationToken, policyDecision.Decision);

        var guardedOutput = await _outputGuard.SanitizeAsync(context, aiResponse, cancellationToken);

        var response = new GovernanceResponse
        {
            CorrelationId = context.CorrelationId,
            Decision = policyDecision.Decision,
            RiskLevel = riskAssessment.RiskLevel,
            RiskScore = riskAssessment.RiskScore,
            RiskCategory = riskAssessment.Category,
            Message = "Request completed through governance pipeline.",
            AiResponse = guardedOutput.Content,
            Reasons = policyDecision.Reasons.Concat(guardedOutput.Findings).ToList(),
            AppliedControls = validationResult.AppliedControls
                .Concat(policyDecision.AppliedControls)
                .Concat(guardedOutput.AppliedControls)
                .Distinct()
                .ToList(),
            Metadata =
            {
                ["provider"] = aiResponse.Provider,
                ["model"] = aiResponse.Model
            }
        };

        await AuditAsync(context, "ResponseCompleted", response.Message, riskAssessment, cancellationToken, response.Decision);
        return response;
    }

    private static GovernanceContext CreateContext(GovernanceRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CorrelationId))
        {
            request.CorrelationId = Guid.NewGuid().ToString("n");
        }

        return new GovernanceContext(request);
    }

    private static RiskAssessment BuildBlockedRisk(InputValidationResult validationResult)
    {
        var topFinding = validationResult.Findings.OrderByDescending(f => f.RiskScore).First();

        return new RiskAssessment
        {
            RiskLevel = topFinding.RiskLevel,
            RiskScore = topFinding.RiskScore,
            Category = topFinding.Category,
            Reasons = validationResult.Findings.Select(f => $"{f.Category}: {f.Message}").Distinct().ToList()
        };
    }

    private Task AuditAsync(
        GovernanceContext context,
        string eventType,
        string message,
        RiskAssessment? riskAssessment,
        CancellationToken cancellationToken,
        string decision = "")
    {
        return _auditLogger.LogAsync(new AuditEntry
        {
            CorrelationId = context.CorrelationId,
            EventType = eventType,
            UserId = context.Request.UserId,
            TenantId = context.Request.TenantId,
            Purpose = context.Request.Purpose,
            Message = message,
            Decision = decision,
            RiskLevel = riskAssessment?.RiskLevel ?? "",
            RiskScore = riskAssessment?.RiskScore ?? 0m,
            RiskCategory = riskAssessment?.Category ?? "",
            Metadata = new Dictionary<string, string>(context.Request.Metadata)
        }, cancellationToken);
    }
}
