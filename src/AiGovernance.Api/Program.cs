using AiGovernance.Api.Configuration;
using AiGovernance.Core.Configuration;
using AiGovernance.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("Configuration/input-validator.json", optional: false, reloadOnChange: true)
    .AddJsonFile("Configuration/blocked-terms.json", optional: false, reloadOnChange: true)
    .AddJsonFile("Configuration/risk-engine.json", optional: false, reloadOnChange: true)
    .AddJsonFile("Configuration/policy-engine.json", optional: false, reloadOnChange: true)
    .AddJsonFile("Configuration/ai-connector.json", optional: false, reloadOnChange: true)
    .AddJsonFile("Configuration/output-guard.json", optional: false, reloadOnChange: true)
    .AddJsonFile("Configuration/audit.json", optional: false, reloadOnChange: true)
    .AddJsonFile("Configuration/governance-policy.json", optional: false, reloadOnChange: true)
    .AddJsonFile("Configuration/risk-register.json", optional: false, reloadOnChange: true)
    .AddJsonFile("Configuration/compliance-frameworks.json", optional: false, reloadOnChange: true)
    .AddJsonFile("Configuration/agent-governance.json", optional: false, reloadOnChange: true);

builder.Services.ConfigureGovernanceOptions(builder.Configuration);
builder.Services.AddGovernanceServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}
