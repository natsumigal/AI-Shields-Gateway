namespace AiGovernance.Core.Models;

public sealed class InputValidationResult
{
    public bool IsValid { get; set; } = true;

    public List<InputValidationFinding> Findings { get; set; } = [];

    public List<string> AppliedControls { get; set; } = [];
}

public sealed class InputValidationFinding
{
    public string Category { get; set; } = "";

    public string Severity { get; set; } = "Warning";

    public string RiskLevel { get; set; } = "Medium";

    public decimal RiskScore { get; set; }

    public string Message { get; set; } = "";

    public string MatchedTerm { get; set; } = "";
}
