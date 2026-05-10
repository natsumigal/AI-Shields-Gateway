using AiGovernance.Core.Models;

namespace AiGovernance.Core.Abstractions;

public interface IAgentToolGovernanceService
{
    Task<AgentToolGovernanceResponse> EvaluateAsync(
        AgentToolGovernanceRequest request,
        CancellationToken cancellationToken);
}
