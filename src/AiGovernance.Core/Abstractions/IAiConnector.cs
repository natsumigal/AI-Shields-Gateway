using AiGovernance.Core.Models;

namespace AiGovernance.Core.Abstractions;

public interface IAiConnector
{
    Task<AiConnectorResponse> GenerateAsync(GovernanceContext context, CancellationToken cancellationToken);
}
