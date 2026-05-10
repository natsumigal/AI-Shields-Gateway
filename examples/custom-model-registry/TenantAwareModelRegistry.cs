using AiGovernance.Core.Abstractions;
using AiGovernance.Core.Models;

namespace AiGovernance.Examples.CustomModelRegistry;

public sealed class TenantAwareModelRegistry : IModelRegistry
{
    private static readonly AiModelProfile[] Models =
    [
        new()
        {
            ModelId = "support-assistant",
            DisplayName = "Support Assistant",
            Provider = "OpenAI",
            Version = "1.0",
            Owner = "Support Platform Team",
            Status = "Approved",
            RiskTier = RiskLevel.Medium,
            ApprovedPurposes = ["customer-support", "agent-assist"],
            DataClassifications = ["Public", "Internal"],
            PolicyVersion = "support-policy-v1"
        },
        new()
        {
            ModelId = "finance-assistant",
            DisplayName = "Finance Assistant",
            Provider = "AzureOpenAI",
            Version = "1.0",
            Owner = "Finance Systems Team",
            Status = "ReviewRequired",
            RiskTier = RiskLevel.High,
            ApprovedPurposes = ["finance-analysis"],
            DataClassifications = ["Internal", "Confidential"],
            PolicyVersion = "finance-policy-v1"
        }
    ];

    public Task<IReadOnlyList<AiModelProfile>> ListAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<IReadOnlyList<AiModelProfile>>(Models);
    }

    public Task<AiModelProfile?> GetDefaultAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<AiModelProfile?>(Models[0]);
    }

    public Task<AiModelProfile?> GetByIdAsync(string modelId, CancellationToken cancellationToken)
    {
        var model = Models.FirstOrDefault(candidate =>
            candidate.ModelId.Equals(modelId, StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(model);
    }
}
