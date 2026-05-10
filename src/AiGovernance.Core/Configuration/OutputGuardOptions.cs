namespace AiGovernance.Core.Configuration;

public sealed class OutputGuardOptions
{
    public const string SectionName = "OutputGuard";

    public List<string> BlockedOutputTerms { get; set; } = [];

    public List<string> RedactionPatterns { get; set; } = [];

    public string ReplacementText { get; set; } = "[redacted]";
}
