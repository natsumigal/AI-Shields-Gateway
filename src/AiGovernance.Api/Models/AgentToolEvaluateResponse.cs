using AiGovernance.Core.Models;

namespace AiGovernance.Api.Models;

public sealed class AgentToolEvaluateResponse
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

    public DateTimeOffset EvaluatedAt { get; set; }

    public Dictionary<string, string> Metadata { get; set; } = [];

    public static AgentToolEvaluateResponse FromGovernanceResponse(AgentToolGovernanceResponse response)
    {
        return new AgentToolEvaluateResponse
        {
            CorrelationId = response.CorrelationId,
            AgentId = response.AgentId,
            ToolName = response.ToolName,
            Allowed = response.Allowed,
            Decision = response.Decision,
            Reason = response.Reason,
            PolicyName = response.PolicyName,
            MatchedRule = response.MatchedRule,
            PolicyAction = response.PolicyAction,
            RateLimited = response.RateLimited,
            RequiredReview = response.RequiredReview,
            EvaluatedAt = response.EvaluatedAt,
            Metadata = response.Metadata
        };
    }
}
