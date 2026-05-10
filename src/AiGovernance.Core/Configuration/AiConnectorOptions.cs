namespace AiGovernance.Core.Configuration;

public sealed class AiConnectorOptions
{
    public const string SectionName = "AiConnector";

    public string Mode { get; set; } = "Mock";

    public string Provider { get; set; } = "MockProvider";

    public string Model { get; set; } = "mock-governed-model";

    public string Endpoint { get; set; } = "";

    public int TimeoutSeconds { get; set; } = 30;

    public string ApiKeyEnvironmentVariable { get; set; } = "OPENAI_API_KEY";

    public string OrganizationIdEnvironmentVariable { get; set; } = "OPENAI_ORG_ID";

    public string ProjectIdEnvironmentVariable { get; set; } = "OPENAI_PROJECT_ID";

    public string SystemInstruction { get; set; } =
        "You are a helpful AI assistant. Follow the governance decision and avoid unsafe or sensitive output.";

    public int MaxOutputTokens { get; set; } = 600;

    public decimal? Temperature { get; set; } = 0.2m;
}
