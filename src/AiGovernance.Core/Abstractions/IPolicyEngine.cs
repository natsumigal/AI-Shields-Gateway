using AiGovernance.Core.Models;

namespace AiGovernance.Core.Abstractions;

public interface IPolicyEngine
{
    Task<PolicyDecisionResult> DecideAsync(GovernanceContext context, RiskAssessment riskAssessment, CancellationToken cancellationToken);
}
