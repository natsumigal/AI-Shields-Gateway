using AiGovernance.Core.Abstractions;
using AiGovernance.Infrastructure.Ai;
using AiGovernance.Infrastructure.Agentic;
using AiGovernance.Infrastructure.Audit;
using AiGovernance.Infrastructure.Guards;
using AiGovernance.Infrastructure.Policy;
using AiGovernance.Infrastructure.Risk;
using AiGovernance.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AiGovernance.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddGovernanceServices(this IServiceCollection services)
    {
        services.AddSingleton<IInputValidator, CategorizedInputValidator>();
        services.AddSingleton<IRiskAnalyzer, RuleBasedRiskAnalyzer>();
        services.AddSingleton<IPolicyEngine, RuleBasedPolicyEngine>();
        services.AddSingleton<MockAiConnector>();
        services.AddHttpClient<OpenAiResponsesConnector>((serviceProvider, client) =>
        {
            var options = serviceProvider
                .GetRequiredService<Microsoft.Extensions.Options.IOptionsMonitor<AiGovernance.Core.Configuration.AiConnectorOptions>>()
                .CurrentValue;

            client.Timeout = TimeSpan.FromSeconds(Math.Max(1, options.TimeoutSeconds));
        });
        services.AddSingleton<IAiConnector, AiConnectorRouter>();
        services.AddSingleton<IOutputGuard, RegexOutputGuard>();
        services.AddSingleton<IAuditLogger, InMemoryAuditLogger>();
        services.AddSingleton<IGovernancePipeline, GovernancePipeline>();
        services.AddSingleton<IAgentToolGovernanceService, MicrosoftAgentToolGovernanceService>();

        return services;
    }
}
