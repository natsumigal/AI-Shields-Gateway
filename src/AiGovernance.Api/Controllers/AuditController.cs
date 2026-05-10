using AiGovernance.Core.Abstractions;
using AiGovernance.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace AiGovernance.Api.Controllers;

[ApiController]
[Route("v1/audit")]
public sealed class AuditController : ControllerBase
{
    private readonly IAuditLogger _auditLogger;

    public AuditController(IAuditLogger auditLogger)
    {
        _auditLogger = auditLogger;
    }

    [HttpGet("{correlationId}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<AuditEntry>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<AuditEntry>>> GetByCorrelationId(
        string correlationId,
        CancellationToken cancellationToken)
    {
        var entries = await _auditLogger.GetByCorrelationIdAsync(correlationId, cancellationToken);
        return Ok(entries);
    }
}
