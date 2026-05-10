namespace AiGovernance.Core.Configuration;

public sealed class PolicyEngineOptions
{
    public const string SectionName = "PolicyEngine";

    public decimal HumanReviewThreshold { get; set; } = 0.65m;

    public decimal BlockThreshold { get; set; } = 0.9m;

    public List<string> HumanReviewCategories { get; set; } = [];

    public List<string> BlockCategories { get; set; } = [];

    public List<string> AllowedPurposes { get; set; } = [];
}
