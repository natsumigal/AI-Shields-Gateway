namespace AiGovernance.Core.Configuration;

public sealed class AuditOptions
{
    public const string SectionName = "Audit";

    public bool Enabled { get; set; } = true;

    public int RetentionDays { get; set; } = 90;

    public bool IncludePromptInAudit { get; set; } = false;
}
