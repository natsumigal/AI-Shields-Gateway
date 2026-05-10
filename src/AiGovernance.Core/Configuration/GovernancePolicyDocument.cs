namespace AiGovernance.Core.Configuration;

public sealed class GovernancePolicyDocument
{
    public const string SectionName = "GovernancePolicy";

    public string Name { get; set; } = "AI Governance Policy";

    public string Version { get; set; } = "1.0";

    public string Owner { get; set; } = "AI Governance Team";

    public string FrameworkProfile { get; set; } = "";

    public string RiskAppetite { get; set; } = "";

    public List<GovernanceControl> Controls { get; set; } = [];
}

public sealed class GovernanceControl
{
    public string ControlId { get; set; } = "";

    public string Function { get; set; } = "";

    public string Component { get; set; } = "";

    public string Description { get; set; } = "";

    public string Evidence { get; set; } = "";
}
