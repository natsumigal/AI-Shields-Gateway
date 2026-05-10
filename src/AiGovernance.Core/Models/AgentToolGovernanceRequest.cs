namespace AiGovernance.Core.Models;

public sealed class AgentToolGovernanceRequest
{
    public string AgentId { get; set; } = "";

    public string ToolName { get; set; } = "";

    public Dictionary<string, object> Arguments { get; set; } = [];

    public string UserId { get; set; } = "";

    public string TenantId { get; set; } = "";

    public string Purpose { get; set; } = "";

    public string CorrelationId { get; set; } = "";

    public Dictionary<string, string> Metadata { get; set; } = [];
}
