using AiGovernance.Core.Models;

namespace AiGovernance.Core.Abstractions;

public interface IRiskAnalyzer
{
    Task<RiskAssessment> AnalyzeAsync(GovernanceContext context, InputValidationResult validationResult, CancellationToken cancellationToken);
}
