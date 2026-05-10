using AiGovernance.Core.Abstractions;
using AiGovernance.Core.Configuration;
using AiGovernance.Core.Models;
using Microsoft.Extensions.Options;

namespace AiGovernance.Infrastructure.Ai;

public sealed class AiConnectorRouter : IAiConnector
{
    private readonly IOptionsMonitor<AiConnectorOptions> _options;
    private readonly MockAiConnector _mockConnector;
    private readonly OpenAiResponsesConnector _openAiConnector;

    public AiConnectorRouter(
        IOptionsMonitor<AiConnectorOptions> options,
        MockAiConnector mockConnector,
        OpenAiResponsesConnector openAiConnector)
    {
        _options = options;
        _mockConnector = mockConnector;
        _openAiConnector = openAiConnector;
    }

    public Task<AiConnectorResponse> GenerateAsync(GovernanceContext context, CancellationToken cancellationToken)
    {
        return _options.CurrentValue.Mode.Equals("OpenAI", StringComparison.OrdinalIgnoreCase)
            ? _openAiConnector.GenerateAsync(context, cancellationToken)
            : _mockConnector.GenerateAsync(context, cancellationToken);
    }
}
