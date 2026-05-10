using AiGovernance.Api.Models;
using AiGovernance.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AiGovernance.Api.Controllers;

[ApiController]
[Route("api/process")]
public sealed class ProcessController : ControllerBase
{
    private readonly IGovernancePipeline _pipeline;

    public ProcessController(IGovernancePipeline pipeline)
    {
        _pipeline = pipeline;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiProcessResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiProcessResponse>> Process(
        ApiProcessRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _pipeline.ProcessAsync(request.ToGovernanceRequest(), cancellationToken);
        return Ok(ApiProcessResponse.FromGovernanceResponse(response));
    }
}
