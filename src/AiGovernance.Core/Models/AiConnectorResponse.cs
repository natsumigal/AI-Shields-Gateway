namespace AiGovernance.Core.Models;

public sealed class AiConnectorResponse
{
    public string Provider { get; set; } = "";

    public string Model { get; set; } = "";

    public string Content { get; set; } = "";

    public Dictionary<string, string> Metadata { get; set; } = [];
}
