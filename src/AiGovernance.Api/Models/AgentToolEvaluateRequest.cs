using System.Text.Json;
using AiGovernance.Core.Models;

namespace AiGovernance.Api.Models;

public sealed class AgentToolEvaluateRequest
{
    public string AgentId { get; set; } = "";

    public string ToolName { get; set; } = "";

    public Dictionary<string, JsonElement> Arguments { get; set; } = [];

    public string UserId { get; set; } = "";

    public string TenantId { get; set; } = "";

    public string Purpose { get; set; } = "";

    public string CorrelationId { get; set; } = "";

    public Dictionary<string, string> Metadata { get; set; } = [];

    public AgentToolGovernanceRequest ToGovernanceRequest()
    {
        return new AgentToolGovernanceRequest
        {
            AgentId = AgentId,
            ToolName = ToolName,
            Arguments = Arguments.ToDictionary(
                pair => pair.Key,
                pair => ConvertJsonElement(pair.Value)),
            UserId = UserId,
            TenantId = TenantId,
            Purpose = Purpose,
            CorrelationId = CorrelationId,
            Metadata = Metadata
        };
    }

    private static object ConvertJsonElement(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString() ?? "",
            JsonValueKind.Number when element.TryGetInt64(out var longValue) => longValue,
            JsonValueKind.Number when element.TryGetDouble(out var doubleValue) => doubleValue,
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => "",
            _ => element.GetRawText()
        };
    }
}
