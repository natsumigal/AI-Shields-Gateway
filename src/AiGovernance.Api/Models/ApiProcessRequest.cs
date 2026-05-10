using AiGovernance.Core.Models;

namespace AiGovernance.Api.Models;

public sealed class ApiProcessRequest
{
    public string Prompt { get; set; } = "";

    public string UserId { get; set; } = "";

    public string TenantId { get; set; } = "";

    public string Purpose { get; set; } = "";

    public string CorrelationId { get; set; } = "";

    public Dictionary<string, string> Metadata { get; set; } = [];

    public GovernanceRequest ToGovernanceRequest()
    {
        return new GovernanceRequest
        {
            Prompt = Prompt,
            UserId = UserId,
            TenantId = TenantId,
            Purpose = Purpose,
            CorrelationId = CorrelationId,
            Metadata = Metadata
        };
    }
}
