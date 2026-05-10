namespace AiGovernance.Core.Models;

public sealed class OutputGuardResult
{
    public string Content { get; set; } = "";

    public List<string> AppliedControls { get; set; } = [];

    public List<string> Findings { get; set; } = [];
}
