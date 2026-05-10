using AiGovernance.Core.Models;

namespace AiGovernance.Core.Abstractions;

public interface IInputValidator
{
    Task<InputValidationResult> ValidateAsync(GovernanceContext context, CancellationToken cancellationToken);
}
