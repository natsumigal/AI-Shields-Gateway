namespace AiGovernance.Core.Models;

public sealed class GovernanceRequest
{
    public string Prompt { get; set; } = "";

    public string UserId { get; set; } = "";

    public string TenantId { get; set; } = "";

    public string Purpose { get; set; } = "";

    public string CorrelationId { get; set; } = "";

    public Dictionary<string, string> Metadata { get; set; } = [];
}
