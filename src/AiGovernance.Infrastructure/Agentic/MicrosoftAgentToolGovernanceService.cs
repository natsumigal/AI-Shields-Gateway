using AgentGovernance;
using AgentGovernance.Policy;
using AiGovernance.Core.Abstractions;
using AiGovernance.Core.Configuration;
using AiGovernance.Core.Models;
using Microsoft.Extensions.Options;

namespace AiGovernance.Infrastructure.Agentic;

public sealed class MicrosoftAgentToolGovernanceService : IAgentToolGovernanceService
{
    private readonly IOptionsMonitor<AgentGovernanceOptions> _options;
    private readonly IAuditLogger _auditLogger;

    public MicrosoftAgentToolGovernanceService(
        IOptionsMonitor<AgentGovernanceOptions> options,
        IAuditLogger auditLogger)
    {
        _options = options;
        _auditLogger = auditLogger;
    }

    public async Task<AgentToolGovernanceResponse> EvaluateAsync(
        AgentToolGovernanceRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.CorrelationId))
        {
            request.CorrelationId = Guid.NewGuid().ToString("n");
        }

        if (string.IsNullOrWhiteSpace(request.AgentId))
        {
            return await BlockAsync(request, "Agent id is required.", cancellationToken);
        }

        if (string.IsNullOrWhiteSpace(request.ToolName))
        {
            return await BlockAsync(request, "Tool name is required.", cancellationToken);
        }

        var options = _options.CurrentValue;
        if (!options.Enabled)
        {
            return await BlockAsync(request, "Agent governance is disabled.", cancellationToken);
        }

        using var kernel = new GovernanceKernel(new GovernanceOptions
        {
            PolicyPaths = options.PolicyPaths.Select(ResolvePolicyPath).ToList(),
            ConflictStrategy = ParseConflictStrategy(options.ConflictStrategy),
            EnableRings = options.EnableRings,
            EnablePromptInjectionDetection = options.EnablePromptInjectionDetection,
            EnableCircuitBreaker = options.EnableCircuitBreaker,
            EnableAudit = options.EnableAudit,
            EnableMetrics = options.EnableMetrics
        });

        AgentGovernance.Integration.ToolCallResult result;
        try
        {
            result = kernel.EvaluateToolCall(
                request.AgentId,
                request.ToolName,
                request.Arguments);
        }
        catch (ArgumentException exception)
        {
            return await BlockAsync(request, exception.Message, cancellationToken);
        }

        var policyDecision = result.PolicyDecision;
        var response = new AgentToolGovernanceResponse
        {
            CorrelationId = request.CorrelationId,
            AgentId = request.AgentId,
            ToolName = request.ToolName,
            Allowed = result.Allowed,
            Decision = result.Allowed ? GovernanceDecision.Allow : GovernanceDecision.Block,
            Reason = result.Reason,
            PolicyName = policyDecision?.PolicyName ?? "",
            MatchedRule = policyDecision?.MatchedRule ?? "",
            PolicyAction = policyDecision?.Action.ToString() ?? "",
            RateLimited = policyDecision?.RateLimited ?? false,
            RequiredReview = !result.Allowed,
            Metadata =
            {
                ["source"] = "Microsoft.AgentGovernance",
                ["package"] = "Microsoft.AgentGovernance"
            }
        };

        await AuditAsync(request, response, cancellationToken);
        return response;
    }

    private async Task<AgentToolGovernanceResponse> BlockAsync(
        AgentToolGovernanceRequest request,
        string reason,
        CancellationToken cancellationToken)
    {
        var response = new AgentToolGovernanceResponse
        {
            CorrelationId = request.CorrelationId,
            AgentId = request.AgentId,
            ToolName = request.ToolName,
            Allowed = false,
            Decision = GovernanceDecision.Block,
            Reason = reason,
            RequiredReview = true,
            Metadata =
            {
                ["source"] = "AI Governance Gateway"
            }
        };

        await AuditAsync(request, response, cancellationToken);
        return response;
    }

    private static ConflictResolutionStrategy ParseConflictStrategy(string value)
    {
        return Enum.TryParse<ConflictResolutionStrategy>(value, ignoreCase: true, out var strategy)
            ? strategy
            : ConflictResolutionStrategy.DenyOverrides;
    }

    private static string ResolvePolicyPath(string path)
    {
        if (Path.IsPathRooted(path))
        {
            return path;
        }

        var outputPath = Path.Combine(AppContext.BaseDirectory, path);
        return File.Exists(outputPath)
            ? outputPath
            : Path.GetFullPath(path);
    }

    private Task AuditAsync(
        AgentToolGovernanceRequest request,
        AgentToolGovernanceResponse response,
        CancellationToken cancellationToken)
    {
        return _auditLogger.LogAsync(new AuditEntry
        {
            CorrelationId = response.CorrelationId,
            EventType = "AgentToolEvaluated",
            UserId = request.UserId,
            TenantId = request.TenantId,
            Purpose = request.Purpose,
            Message = response.Reason,
            Decision = response.Decision,
            RiskLevel = response.Allowed ? "Low" : "High",
            RiskScore = response.Allowed ? 0.2m : 0.8m,
            RiskCategory = "AgentToolGovernance",
            Metadata = new Dictionary<string, string>(request.Metadata)
            {
                ["agentId"] = response.AgentId,
                ["toolName"] = response.ToolName,
                ["policyName"] = response.PolicyName,
                ["matchedRule"] = response.MatchedRule,
                ["policyAction"] = response.PolicyAction,
                ["rateLimited"] = response.RateLimited.ToString()
            }
        }, cancellationToken);
    }
}
