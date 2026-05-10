using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using AiGovernance.Core.Abstractions;
using AiGovernance.Core.Models;
using AiGovernance.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace AiGovernance.Examples.CustomAiConnector;

public sealed class OpenAiResponsesConnector : IAiConnector
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly HttpClient _httpClient;
    private readonly AiConnectorOptions _options;

    public OpenAiResponsesConnector(HttpClient httpClient, IOptions<AiConnectorOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<AiCompletion> CompleteAsync(GovernanceContext context, CancellationToken cancellationToken)
    {
        var endpoint = string.IsNullOrWhiteSpace(_options.Endpoint)
            ? "https://api.openai.com/v1/responses"
            : _options.Endpoint;

        var model = context.Model?.ModelId ?? context.Request.ModelId ?? _options.Model;
        var prompt = context.Input?.SanitizedPrompt ?? context.Request.Prompt;

        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        if (!string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);
        }
        request.Content = JsonContent.Create(new
        {
            model,
            input = prompt,
            instructions = "Follow the gateway policy decision. Do not reveal secrets or unsafe instructions.",
            metadata = new
            {
                correlationId = context.CorrelationId,
                tenantId = context.Request.TenantId,
                userId = context.Request.UserId,
                purpose = context.Request.Purpose,
                riskLevel = context.Risk?.Level.ToString(),
                policyId = context.Policy?.PolicyId
            }
        }, options: JsonOptions);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        response.EnsureSuccessStatusCode();

        return new AiCompletion(
            ExtractOutputText(body),
            "OpenAI",
            model,
            new Dictionary<string, string>
            {
                ["correlationId"] = context.CorrelationId,
                ["statusCode"] = ((int)response.StatusCode).ToString()
            });
    }

    private static string ExtractOutputText(string body)
    {
        using var document = JsonDocument.Parse(body);
        var root = document.RootElement;

        if (root.TryGetProperty("output_text", out var outputText))
        {
            return outputText.GetString() ?? string.Empty;
        }

        return body;
    }
}
