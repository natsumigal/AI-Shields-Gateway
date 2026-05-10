using AiGovernance.Core.Configuration;

namespace AiGovernance.Api.Configuration;

public static class GovernanceOptionsRegistration
{
    public static IServiceCollection ConfigureGovernanceOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<InputValidatorOptions>(configuration.GetSection(InputValidatorOptions.SectionName));
        services.Configure<RiskEngineOptions>(configuration.GetSection(RiskEngineOptions.SectionName));
        services.Configure<PolicyEngineOptions>(configuration.GetSection(PolicyEngineOptions.SectionName));
        services.Configure<AiConnectorOptions>(configuration.GetSection(AiConnectorOptions.SectionName));
        services.Configure<OutputGuardOptions>(configuration.GetSection(OutputGuardOptions.SectionName));
        services.Configure<AuditOptions>(configuration.GetSection(AuditOptions.SectionName));
        services.Configure<GovernancePolicyDocument>(configuration.GetSection(GovernancePolicyDocument.SectionName));
        services.Configure<RiskRegisterOptions>(configuration.GetSection(RiskRegisterOptions.SectionName));
        services.Configure<ComplianceFrameworkCatalog>(configuration.GetSection(ComplianceFrameworkCatalog.SectionName));
        services.Configure<AgentGovernanceOptions>(configuration.GetSection(AgentGovernanceOptions.SectionName));

        return services;
    }
}
