namespace AiGovernance.Core.Models;

public sealed class GovernanceContext
{
    public GovernanceRequest Request { get; }

    public DateTimeOffset ReceivedAt { get; } = DateTimeOffset.UtcNow;

    public string CorrelationId => Request.CorrelationId;

    public GovernanceContext(GovernanceRequest request)
    {
        Request = request;
    }
}
