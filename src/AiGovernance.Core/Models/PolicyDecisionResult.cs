namespace AiGovernance.Core.Models;

public sealed class PolicyDecisionResult
{
    public string Decision { get; set; } = GovernanceDecision.Block;

    public string Message { get; set; } = "";

    public List<string> Reasons { get; set; } = [];

    public List<string> AppliedControls { get; set; } = [];
}
