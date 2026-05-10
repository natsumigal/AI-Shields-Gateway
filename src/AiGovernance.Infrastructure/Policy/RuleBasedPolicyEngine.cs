using AiGovernance.Core.Abstractions;
using AiGovernance.Core.Configuration;
using AiGovernance.Core.Models;
using Microsoft.Extensions.Options;

namespace AiGovernance.Infrastructure.Policy;

public sealed class RuleBasedPolicyEngine : IPolicyEngine
{
    private readonly IOptionsMonitor<PolicyEngineOptions> _options;

    public RuleBasedPolicyEngine(IOptionsMonitor<PolicyEngineOptions> options)
    {
        _options = options;
    }

    public Task<PolicyDecisionResult> DecideAsync(GovernanceContext context, RiskAssessment riskAssessment, CancellationToken cancellationToken)
    {
        var options = _options.CurrentValue;
        var result = new PolicyDecisionResult();
        result.AppliedControls.Add("PolicyEngine.ScoreThresholds");
        result.AppliedControls.Add("PolicyEngine.CategoryRules");

        if (options.AllowedPurposes.Count > 0 && !options.AllowedPurposes.Contains(context.Request.Purpose, StringComparer.OrdinalIgnoreCase))
        {
            result.Decision = GovernanceDecision.HumanReview;
            result.Message = "Purpose is outside the configured allowed purpose list.";
            result.Reasons.Add($"Purpose '{context.Request.Purpose}' requires review.");
            return Task.FromResult(result);
        }

        if (options.BlockCategories.Contains(riskAssessment.Category, StringComparer.OrdinalIgnoreCase)
            || riskAssessment.RiskScore >= options.BlockThreshold)
        {
            result.Decision = GovernanceDecision.Block;
            result.Message = "Request blocked by policy.";
            result.Reasons.Add($"Risk category '{riskAssessment.Category}' or score '{riskAssessment.RiskScore}' crossed the block policy.");
            return Task.FromResult(result);
        }

        if (options.HumanReviewCategories.Contains(riskAssessment.Category, StringComparer.OrdinalIgnoreCase)
            || riskAssessment.RiskScore >= options.HumanReviewThreshold)
        {
            result.Decision = GovernanceDecision.HumanReview;
            result.Message = "Request requires human review before calling the AI model.";
            result.Reasons.Add($"Risk category '{riskAssessment.Category}' or score '{riskAssessment.RiskScore}' crossed the review policy.");
            return Task.FromResult(result);
        }

        result.Decision = GovernanceDecision.Allow;
        result.Message = "Request allowed by governance policy.";
        result.Reasons.Add("Risk score and category are within policy limits.");

        return Task.FromResult(result);
    }
}
