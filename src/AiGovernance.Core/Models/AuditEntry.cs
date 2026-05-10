namespace AiGovernance.Core.Models;

public sealed class AuditEntry
{
    public string CorrelationId { get; set; } = "";

    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

    public string EventType { get; set; } = "";

    public string UserId { get; set; } = "";

    public string TenantId { get; set; } = "";

    public string Purpose { get; set; } = "";

    public string Message { get; set; } = "";

    public string Decision { get; set; } = "";

    public string RiskLevel { get; set; } = "";

    public decimal RiskScore { get; set; }

    public string RiskCategory { get; set; } = "";

    public Dictionary<string, string> Metadata { get; set; } = [];
}
