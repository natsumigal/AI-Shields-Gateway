using System.Text.RegularExpressions;
using AiGovernance.Core.Abstractions;
using AiGovernance.Core.Configuration;
using AiGovernance.Core.Models;
using Microsoft.Extensions.Options;

namespace AiGovernance.Infrastructure.Guards;

public sealed class RegexOutputGuard : IOutputGuard
{
    private readonly IOptionsMonitor<OutputGuardOptions> _options;

    public RegexOutputGuard(IOptionsMonitor<OutputGuardOptions> options)
    {
        _options = options;
    }

    public Task<OutputGuardResult> SanitizeAsync(GovernanceContext context, AiConnectorResponse aiResponse, CancellationToken cancellationToken)
    {
        var options = _options.CurrentValue;
        var content = aiResponse.Content;
        var result = new OutputGuardResult { Content = content };

        foreach (var blockedTerm in options.BlockedOutputTerms)
        {
            if (!content.Contains(blockedTerm, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            content = Regex.Replace(content, Regex.Escape(blockedTerm), options.ReplacementText, RegexOptions.IgnoreCase);
            result.Findings.Add($"Blocked output term redacted: {blockedTerm}");
        }

        foreach (var pattern in options.RedactionPatterns)
        {
            var updated = Regex.Replace(content, pattern, options.ReplacementText, RegexOptions.IgnoreCase);
            if (!string.Equals(updated, content, StringComparison.Ordinal))
            {
                result.Findings.Add($"Sensitive output pattern redacted: {pattern}");
                content = updated;
            }
        }

        result.Content = content;
        result.AppliedControls.Add("OutputGuard.RedactionRules");

        return Task.FromResult(result);
    }
}
