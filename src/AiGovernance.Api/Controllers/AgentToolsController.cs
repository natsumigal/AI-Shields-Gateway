using AiGovernance.Api.Models;
using AiGovernance.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AiGovernance.Api.Controllers;

[ApiController]
[Route("api/agents/tool-evaluate")]
public sealed class AgentToolsController : ControllerBase
{
    private readonly IAgentToolGovernanceService _toolGovernanceService;

    public AgentToolsController(IAgentToolGovernanceService toolGovernanceService)
    {
        _toolGovernanceService = toolGovernanceService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AgentToolEvaluateResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AgentToolEvaluateResponse>> Evaluate(
        AgentToolEvaluateRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _toolGovernanceService.EvaluateAsync(
            request.ToGovernanceRequest(),
            cancellationToken);

        return Ok(AgentToolEvaluateResponse.FromGovernanceResponse(response));
    }
}
