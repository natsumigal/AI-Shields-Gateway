using AiGovernance.Core.Abstractions;
using AiGovernance.Core.Configuration;
using AiGovernance.Core.Models;
using Microsoft.Extensions.Options;

namespace AiGovernance.Infrastructure.Ai;

public sealed class MockAiConnector : IAiConnector
{
    private readonly IOptionsMonitor<AiConnectorOptions> _options;

    public MockAiConnector(IOptionsMonitor<AiConnectorOptions> options)
    {
        _options = options;
    }

    public Task<AiConnectorResponse> GenerateAsync(GovernanceContext context, CancellationToken cancellationToken)
    {
        var options = _options.CurrentValue;

        var response = new AiConnectorResponse
        {
            Provider = options.Provider,
            Model = options.Model,
            Content = $"Mock AI response approved for purpose '{context.Request.Purpose}'. Governance checks completed before model call.",
            Metadata =
            {
                ["mode"] = options.Mode,
                ["tenantId"] = context.Request.TenantId
            }
        };

        return Task.FromResult(response);
    }
}
