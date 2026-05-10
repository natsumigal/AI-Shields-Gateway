using System.Collections.Concurrent;
using AiGovernance.Core.Abstractions;
using AiGovernance.Core.Configuration;
using AiGovernance.Core.Models;
using Microsoft.Extensions.Options;

namespace AiGovernance.Infrastructure.Audit;

public sealed class InMemoryAuditLogger : IAuditLogger
{
    private readonly ConcurrentDictionary<string, List<AuditEntry>> _events = new();
    private readonly IOptionsMonitor<AuditOptions> _options;

    public InMemoryAuditLogger(IOptionsMonitor<AuditOptions> options)
    {
        _options = options;
    }

    public Task LogAsync(AuditEntry entry, CancellationToken cancellationToken)
    {
        if (!_options.CurrentValue.Enabled)
        {
            return Task.CompletedTask;
        }

        var entries = _events.GetOrAdd(entry.CorrelationId, _ => []);

        lock (entries)
        {
            entries.Add(entry);
        }

        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<AuditEntry>> GetByCorrelationIdAsync(string correlationId, CancellationToken cancellationToken)
    {
        if (!_events.TryGetValue(correlationId, out var entries))
        {
            return Task.FromResult<IReadOnlyCollection<AuditEntry>>(Array.Empty<AuditEntry>());
        }

        lock (entries)
        {
            return Task.FromResult<IReadOnlyCollection<AuditEntry>>(entries.ToArray());
        }
    }
}
