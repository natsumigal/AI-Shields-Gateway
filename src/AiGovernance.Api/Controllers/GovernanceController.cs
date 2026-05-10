using AiGovernance.Api.Models;
using AiGovernance.Core.Abstractions;
using AiGovernance.Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AiGovernance.Api.Controllers;

[ApiController]
[Route("v1/governance")]
public sealed class GovernanceController : ControllerBase
{
    private readonly IGovernancePipeline _pipeline;
    private readonly IOptionsMonitor<GovernancePolicyDocument> _governancePolicy;
    private readonly IOptionsMonitor<RiskRegisterOptions> _riskRegister;
    private readonly IOptionsMonitor<ComplianceFrameworkCatalog> _frameworkCatalog;

    public GovernanceController(
        IGovernancePipeline pipeline,
        IOptionsMonitor<GovernancePolicyDocument> governancePolicy,
        IOptionsMonitor<RiskRegisterOptions> riskRegister,
        IOptionsMonitor<ComplianceFrameworkCatalog> frameworkCatalog)
    {
        _pipeline = pipeline;
        _governancePolicy = governancePolicy;
        _riskRegister = riskRegister;
        _frameworkCatalog = frameworkCatalog;
    }

    [HttpPost("evaluate")]
    [ProducesResponseType(typeof(ApiProcessResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiProcessResponse>> Evaluate(
        ApiProcessRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _pipeline.ProcessAsync(request.ToGovernanceRequest(), cancellationToken);
        return Ok(ApiProcessResponse.FromGovernanceResponse(response));
    }

    [HttpGet("policy")]
    [ProducesResponseType(typeof(GovernancePolicyDocument), StatusCodes.Status200OK)]
    public ActionResult<GovernancePolicyDocument> GetPolicy()
    {
        return Ok(_governancePolicy.CurrentValue);
    }

    [HttpGet("risk-register")]
    [ProducesResponseType(typeof(RiskRegisterOptions), StatusCodes.Status200OK)]
    public ActionResult<RiskRegisterOptions> GetRiskRegister()
    {
        return Ok(_riskRegister.CurrentValue);
    }

    [HttpGet("frameworks")]
    [ProducesResponseType(typeof(ComplianceFrameworkCatalog), StatusCodes.Status200OK)]
    public ActionResult<ComplianceFrameworkCatalog> GetFrameworkAlignment()
    {
        return Ok(_frameworkCatalog.CurrentValue);
    }
}
