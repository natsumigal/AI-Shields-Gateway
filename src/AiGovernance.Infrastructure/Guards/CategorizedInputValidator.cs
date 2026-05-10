using AiGovernance.Core.Abstractions;
using AiGovernance.Core.Configuration;
using AiGovernance.Core.Models;
using Microsoft.Extensions.Options;

namespace AiGovernance.Infrastructure.Guards;

public sealed class CategorizedInputValidator : IInputValidator
{
    private readonly IOptionsMonitor<InputValidatorOptions> _options;

    public CategorizedInputValidator(IOptionsMonitor<InputValidatorOptions> options)
    {
        _options = options;
    }

    public Task<InputValidationResult> ValidateAsync(GovernanceContext context, CancellationToken cancellationToken)
    {
        var options = _options.CurrentValue;
        var prompt = context.Request.Prompt ?? "";
        var result = new InputValidationResult();

        if (string.IsNullOrWhiteSpace(prompt))
        {
            AddFinding(result, "RequiredInput", "Error", "Critical", 1.0m, "Prompt is required.", "");
        }

        if (prompt.Length > options.MaxPromptLength)
        {
            AddFinding(result, "PromptLength", "Error", "High", 0.85m, $"Prompt is longer than {options.MaxPromptLength} characters.", "");
        }

        foreach (var blockedTerm in options.BlockedTerms.Where(term => Contains(prompt, term)))
        {
            AddFinding(result, "BlockedTerm", "Error", "Critical", 1.0m, "Prompt contains a globally blocked term.", blockedTerm);
        }

        foreach (var category in options.Categories)
        {
            var matchedTerm = category.Terms.FirstOrDefault(term => Contains(prompt, term));
            if (matchedTerm is null)
            {
                continue;
            }

            AddFinding(
                result,
                category.Category,
                category.Severity,
                category.RiskLevel,
                category.RiskScore,
                category.Description,
                matchedTerm);
        }

        result.IsValid = !result.Findings.Any(f =>
            f.Severity.Equals("Error", StringComparison.OrdinalIgnoreCase));

        if (result.Findings.Count > 0)
        {
            result.AppliedControls.Add("InputValidator.CategoryRules");
        }

        return Task.FromResult(result);
    }

    private static bool Contains(string value, string term)
    {
        return !string.IsNullOrWhiteSpace(term)
            && value.Contains(term, StringComparison.OrdinalIgnoreCase);
    }

    private static void AddFinding(
        InputValidationResult result,
        string category,
        string severity,
        string riskLevel,
        decimal riskScore,
        string message,
        string matchedTerm)
    {
        result.Findings.Add(new InputValidationFinding
        {
            Category = category,
            Severity = severity,
            RiskLevel = riskLevel,
            RiskScore = riskScore,
            Message = message,
            MatchedTerm = matchedTerm
        });
    }
}
