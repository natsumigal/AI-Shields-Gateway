using AiGovernance.Core.Models;

namespace AiGovernance.Core.Abstractions;

public interface IGovernancePipeline
{
    Task<GovernanceResponse> ProcessAsync(GovernanceRequest request, CancellationToken cancellationToken);
}
