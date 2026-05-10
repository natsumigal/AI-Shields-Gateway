namespace AiGovernance.Core.Models;

public sealed class GovernanceResponse
{
    public string CorrelationId { get; set; } = "";

    public string Decision { get; set; } = GovernanceDecision.Block;

    public string RiskLevel { get; set; } = "Low";

    public decimal RiskScore { get; set; }

    public string RiskCategory { get; set; } = "General";

    public string Message { get; set; } = "";

    public string? AiResponse { get; set; }

    public List<string> Reasons { get; set; } = [];

    public List<string> AppliedControls { get; set; } = [];

    public Dictionary<string, string> Metadata { get; set; } = [];
}

public static class GovernanceDecision
{
    public const string Allow = "Allow";
    public const string Block = "Block";
    public const string HumanReview = "HumanReview";
}
