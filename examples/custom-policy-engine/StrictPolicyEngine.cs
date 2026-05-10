using AiGovernance.Core.Abstractions;
using AiGovernance.Core.Models;

namespace AiGovernance.Examples.CustomPolicyEngine;

public sealed class StrictPolicyEngine : IPolicyEngine
{
    public Task<PolicyDecision> DecideAsync(GovernanceContext context, CancellationToken cancellationToken)
    {
        if (context.Input?.Findings.Any(finding => finding.Severity == GuardSeverity.Error) == true)
        {
            return Task.FromResult(new PolicyDecision(
                PolicyAction.Block,
                "strict.input.block",
                "Input validation produced a blocking finding."));
        }

        if (context.Model?.RiskTier >= RiskLevel.High)
        {
            return Task.FromResult(new PolicyDecision(
                PolicyAction.HumanReview,
                "strict.model.review",
                "The selected model has a high or critical risk tier."));
        }

        if (context.Risk?.Level == RiskLevel.Critical || context.Risk?.Score >= 0.85)
        {
            return Task.FromResult(new PolicyDecision(
                PolicyAction.Block,
                "strict.risk.block",
                "Risk exceeded the strict blocking threshold."));
        }

        if (context.Risk?.Level == RiskLevel.High || context.Risk?.Score >= 0.6)
        {
            return Task.FromResult(new PolicyDecision(
                PolicyAction.HumanReview,
                "strict.risk.review",
                "Risk exceeded the strict human-review threshold."));
        }

        return Task.FromResult(new PolicyDecision(
            PolicyAction.Allow,
            "strict.allow",
            "Request passed strict policy checks."));
    }
}
