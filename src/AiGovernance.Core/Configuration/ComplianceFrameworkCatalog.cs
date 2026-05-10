namespace AiGovernance.Core.Configuration;

public sealed class ComplianceFrameworkCatalog
{
    public const string SectionName = "ComplianceFrameworks";

    public string Version { get; set; } = "1.0";

    public string Owner { get; set; } = "AI Governance Team";

    public string Scope { get; set; } = "AI governance gateway controls and evidence mapping";

    public List<ComplianceFramework> Frameworks { get; set; } = [];
}

public sealed class ComplianceFramework
{
    public string FrameworkId { get; set; } = "";

    public string Name { get; set; } = "";

    public string Version { get; set; } = "";

    public string Type { get; set; } = "";

    public string SourceUrl { get; set; } = "";

    public string AlignmentSummary { get; set; } = "";

    public List<string> ProjectComponents { get; set; } = [];

    public List<ComplianceRequirement> Requirements { get; set; } = [];
}

public sealed class ComplianceRequirement
{
    public string RequirementId { get; set; } = "";

    public string Title { get; set; } = "";

    public string Description { get; set; } = "";

    public List<string> MappedComponents { get; set; } = [];

    public List<string> Evidence { get; set; } = [];

    public string Status { get; set; } = "Mapped";

    public string Notes { get; set; } = "";
}
