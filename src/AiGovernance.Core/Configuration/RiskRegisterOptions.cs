namespace AiGovernance.Core.Configuration;

public sealed class RiskRegisterOptions
{
    public const string SectionName = "RiskRegister";

    public string Version { get; set; } = "1.0";

    public List<RiskRegisterEntry> Risks { get; set; } = [];
}

public sealed class RiskRegisterEntry
{
    public string RiskId { get; set; } = "";

    public string Category { get; set; } = "";

    public string Description { get; set; } = "";

    public string Likelihood { get; set; } = "";

    public string Impact { get; set; } = "";

    public string Treatment { get; set; } = "";

    public List<string> MappedFrameworks { get; set; } = [];
}
