using AiGovernance.Core.Abstractions;
using AiGovernance.Core.Configuration;
using AiGovernance.Core.Models;
using Microsoft.Extensions.Options;

namespace AiGovernance.Infrastructure.Risk;

public sealed class RuleBasedRiskAnalyzer : IRiskAnalyzer
{
    private readonly IOptionsMonitor<RiskEngineOptions> _options;

    public RuleBasedRiskAnalyzer(IOptionsMonitor<RiskEngineOptions> options)
    {
        _options = options;
    }

    public Task<RiskAssessment> AnalyzeAsync(GovernanceContext context, InputValidationResult validationResult, CancellationToken cancellationToken)
    {
        var topFinding = validationResult.Findings
            .OrderByDescending(f => f.RiskScore)
            .FirstOrDefault();

        if (topFinding is null)
        {
            return Task.FromResult(new RiskAssessment
            {
                RiskLevel = "Low",
                RiskScore = 0.1m,
                Category = "General",
                Reasons = ["No configured risk category matched the prompt."]
            });
        }

        var level = ResolveRiskLevel(topFinding.RiskScore);

        return Task.FromResult(new RiskAssessment
        {
            RiskLevel = level,
            RiskScore = topFinding.RiskScore,
            Category = topFinding.Category,
            Reasons = validationResult.Findings.Select(f => $"{f.Category}: {f.Message}").Distinct().ToList()
        });
    }

    private string ResolveRiskLevel(decimal score)
    {
        var options = _options.CurrentValue;

        if (score >= options.CriticalThreshold)
        {
            return "Critical";
        }

        if (score >= options.HighThreshold)
        {
            return "High";
        }

        if (score >= options.MediumThreshold)
        {
            return "Medium";
        }

        return score >= options.LowThreshold ? "Low" : "Minimal";
    }
}
