using AiGovernance.Api.Models;
using AiGovernance.Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AiGovernance.Api.Controllers;

[ApiController]
public sealed class HealthController : ControllerBase
{
    private readonly IOptionsMonitor<InputValidatorOptions> _inputValidatorOptions;
    private readonly IOptionsMonitor<PolicyEngineOptions> _policyEngineOptions;
    private readonly IOptionsMonitor<ComplianceFrameworkCatalog> _frameworkCatalog;
    private readonly IOptionsMonitor<AgentGovernanceOptions> _agentGovernanceOptions;

    public HealthController(
        IOptionsMonitor<InputValidatorOptions> inputValidatorOptions,
        IOptionsMonitor<PolicyEngineOptions> policyEngineOptions,
        IOptionsMonitor<ComplianceFrameworkCatalog> frameworkCatalog,
        IOptionsMonitor<AgentGovernanceOptions> agentGovernanceOptions)
    {
        _inputValidatorOptions = inputValidatorOptions;
        _policyEngineOptions = policyEngineOptions;
        _frameworkCatalog = frameworkCatalog;
        _agentGovernanceOptions = agentGovernanceOptions;
    }

    [HttpGet("healthz")]
    [ProducesResponseType(typeof(HealthResponse), StatusCodes.Status200OK)]
    public ActionResult<HealthResponse> Get()
    {
        return Ok(new HealthResponse
        {
            Status = "Healthy",
            CheckedAt = DateTimeOffset.UtcNow
        });
    }

    [HttpGet("healthz/live")]
    [ProducesResponseType(typeof(HealthResponse), StatusCodes.Status200OK)]
    public ActionResult<HealthResponse> Live()
    {
        return Ok(new HealthResponse
        {
            Status = "Live",
            CheckedAt = DateTimeOffset.UtcNow
        });
    }

    [HttpGet("healthz/ready")]
    [ProducesResponseType(typeof(ReadinessResponse), StatusCodes.Status200OK)]
    public ActionResult<ReadinessResponse> Ready()
    {
        var response = new ReadinessResponse
        {
            Status = "Ready",
            CheckedAt = DateTimeOffset.UtcNow,
            Checks =
            {
                ["inputValidator.categories"] = _inputValidatorOptions.CurrentValue.Categories.Count.ToString(),
                ["policyEngine.blockCategories"] = _policyEngineOptions.CurrentValue.BlockCategories.Count.ToString(),
                ["complianceFrameworks.count"] = _frameworkCatalog.CurrentValue.Frameworks.Count.ToString(),
                ["agentGovernance.enabled"] = _agentGovernanceOptions.CurrentValue.Enabled.ToString(),
                ["agentGovernance.policyPaths"] = _agentGovernanceOptions.CurrentValue.PolicyPaths.Count.ToString()
            }
        };

        return Ok(response);
    }
}
