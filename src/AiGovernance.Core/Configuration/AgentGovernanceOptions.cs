namespace AiGovernance.Core.Configuration;

public sealed class AgentGovernanceOptions
{
    public const string SectionName = "AgentGovernance";

    public bool Enabled { get; set; } = true;

    public List<string> PolicyPaths { get; set; } = [];

    public string ConflictStrategy { get; set; } = "DenyOverrides";

    public bool EnableRings { get; set; } = true;

    public bool EnablePromptInjectionDetection { get; set; } = true;

    public bool EnableCircuitBreaker { get; set; } = true;

    public bool EnableAudit { get; set; } = true;

    public bool EnableMetrics { get; set; } = true;
}
