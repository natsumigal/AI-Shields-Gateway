using AiGovernance.Core.Models;

namespace AiGovernance.Api.Models;

public sealed class ApiProcessResponse
{
    public string CorrelationId { get; set; } = "";

    public string Decision { get; set; } = "";

    public string RiskLevel { get; set; } = "";

    public decimal RiskScore { get; set; }

    public string RiskCategory { get; set; } = "";

    public string Message { get; set; } = "";

    public string? AiResponse { get; set; }

    public List<string> Reasons { get; set; } = [];

    public List<string> AppliedControls { get; set; } = [];

    public Dictionary<string, string> Metadata { get; set; } = [];

    public static ApiProcessResponse FromGovernanceResponse(GovernanceResponse response)
    {
        return new ApiProcessResponse
        {
            CorrelationId = response.CorrelationId,
            Decision = response.Decision,
            RiskLevel = response.RiskLevel,
            RiskScore = response.RiskScore,
            RiskCategory = response.RiskCategory,
            Message = response.Message,
            AiResponse = response.AiResponse,
            Reasons = response.Reasons,
            AppliedControls = response.AppliedControls,
            Metadata = response.Metadata
        };
    }
}
