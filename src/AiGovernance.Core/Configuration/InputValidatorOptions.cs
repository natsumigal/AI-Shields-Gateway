namespace AiGovernance.Core.Configuration;

public sealed class InputValidatorOptions
{
    public const string SectionName = "InputValidator";

    public int MaxPromptLength { get; set; } = 4000;

    public List<string> BlockedTerms { get; set; } = [];

    public List<ValidationCategoryRule> Categories { get; set; } = [];
}

public sealed class ValidationCategoryRule
{
    public string Category { get; set; } = "";

    public string Description { get; set; } = "";

    public string Severity { get; set; } = "Warning";

    public string RiskLevel { get; set; } = "Medium";

    public decimal RiskScore { get; set; } = 0.5m;

    public string Action { get; set; } = "Review";

    public List<string> Terms { get; set; } = [];

    public List<string> RedactionPatterns { get; set; } = [];
}
