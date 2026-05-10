namespace AiGovernance.Api.Models;

public class HealthResponse
{
    public string Status { get; set; } = "";

    public DateTimeOffset CheckedAt { get; set; }
}

public sealed class ReadinessResponse : HealthResponse
{
    public Dictionary<string, string> Checks { get; set; } = [];
}
