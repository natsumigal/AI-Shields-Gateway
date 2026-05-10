namespace AiGovernance.Core.Models;

public sealed class RiskAssessment
{
    public string RiskLevel { get; set; } = "Low";

    public decimal RiskScore { get; set; }

    public string Category { get; set; } = "General";

    public List<string> Reasons { get; set; } = [];
}
