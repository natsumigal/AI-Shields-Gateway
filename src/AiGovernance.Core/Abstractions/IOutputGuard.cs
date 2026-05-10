using AiGovernance.Core.Models;

namespace AiGovernance.Core.Abstractions;

public interface IOutputGuard
{
    Task<OutputGuardResult> SanitizeAsync(GovernanceContext context, AiConnectorResponse aiResponse, CancellationToken cancellationToken);
}
