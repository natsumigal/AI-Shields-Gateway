namespace AiGovernance.Core.Models;

public sealed class AgentToolGovernanceResponse
{
    public string CorrelationId { get; set; } = "";

    public string AgentId { get; set; } = "";

    public string ToolName { get; set; } = "";

    public bool Allowed { get; set; }

    public string Decision { get; set; } = "";

    public string Reason { get; set; } = "";

    public string PolicyName { get; set; } = "";

    public string MatchedRule { get; set; } = "";

    public string PolicyAction { get; set; } = "";

    public bool RateLimited { get; set; }

    public bool RequiredReview { get; set; }

    public DateTimeOffset EvaluatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Dictionary<string, string> Metadata { get; set; } = [];
}
