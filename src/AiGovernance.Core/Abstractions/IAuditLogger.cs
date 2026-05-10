using AiGovernance.Core.Models;

namespace AiGovernance.Core.Abstractions;

public interface IAuditLogger
{
    Task LogAsync(AuditEntry entry, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<AuditEntry>> GetByCorrelationIdAsync(string correlationId, CancellationToken cancellationToken);
}
