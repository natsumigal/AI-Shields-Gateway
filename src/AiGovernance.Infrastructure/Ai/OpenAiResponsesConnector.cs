using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using AiGovernance.Core.Abstractions;
using AiGovernance.Core.Configuration;
using AiGovernance.Core.Models;
using Microsoft.Extensions.Options;

namespace AiGovernance.Infrastructure.Ai;

public sealed class OpenAiResponsesConnector : IAiConnector
{
    private readonly HttpClient _httpClient;
    private readonly IOptionsMonitor<AiConnectorOptions> _options;

    public OpenAiResponsesConnector(HttpClient httpClient, IOptionsMonitor<AiConnectorOptions> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    public async Task<AiConnectorResponse> GenerateAsync(GovernanceContext context, CancellationToken cancellationToken)
    {
        var options = _options.CurrentValue;
        var apiKey = Environment.GetEnvironmentVariable(options.ApiKeyEnvironmentVariable);

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                $"OpenAI connector is enabled, but environment variable '{options.ApiKeyEnvironmentVariable}' is missing.");
        }

        using var request = CreateRequest(context, options, apiKey);
        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"OpenAI Responses API call failed with status {(int)response.StatusCode}: {responseBody}");
        }

        var text = ExtractOutputText(responseBody);

        return new AiConnectorResponse
        {
            Provider = "OpenAI",
            Model = options.Model,
            Content = text,
            Metadata =
            {
                ["mode"] = options.Mode,
                ["endpoint"] = ResolveEndpoint(options),
                ["tenantId"] = context.Request.TenantId
            }
        };
    }

    private HttpRequestMessage CreateRequest(GovernanceContext context, AiConnectorOptions options, string apiKey)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, ResolveEndpoint(options));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        AddOptionalHeader(request, "OpenAI-Organization", options.OrganizationIdEnvironmentVariable);
        AddOptionalHeader(request, "OpenAI-Project", options.ProjectIdEnvironmentVariable);

        var payload = new Dictionary<string, object?>
        {
            ["model"] = options.Model,
            ["instructions"] = options.SystemInstruction,
            ["input"] = context.Request.Prompt,
            ["max_output_tokens"] = options.MaxOutputTokens,
            ["metadata"] = new Dictionary<string, string>
            {
                ["correlation_id"] = context.CorrelationId,
                ["tenant_id"] = context.Request.TenantId,
                ["user_id"] = context.Request.UserId,
                ["purpose"] = context.Request.Purpose
            }
        };

        if (options.Temperature is not null)
        {
            payload["temperature"] = options.Temperature.Value;
        }

        request.Content = JsonContent.Create(payload);
        return request;
    }

    private static void AddOptionalHeader(HttpRequestMessage request, string headerName, string environmentVariableName)
    {
        if (string.IsNullOrWhiteSpace(environmentVariableName))
        {
            return;
        }

        var value = Environment.GetEnvironmentVariable(environmentVariableName);
        if (!string.IsNullOrWhiteSpace(value))
        {
            request.Headers.TryAddWithoutValidation(headerName, value);
        }
    }

    private static string ResolveEndpoint(AiConnectorOptions options)
    {
        return string.IsNullOrWhiteSpace(options.Endpoint)
            ? "https://api.openai.com/v1/responses"
            : options.Endpoint;
    }

    private static string ExtractOutputText(string responseBody)
    {
        var root = JsonNode.Parse(responseBody)
            ?? throw new InvalidOperationException("OpenAI response was empty.");

        var directText = root["output_text"]?.GetValue<string>();
        if (!string.IsNullOrWhiteSpace(directText))
        {
            return directText;
        }

        var output = root["output"]?.AsArray();
        if (output is null)
        {
            return "";
        }

        var parts = new List<string>();

        foreach (var item in output)
        {
            var content = item?["content"]?.AsArray();
            if (content is null)
            {
                continue;
            }

            foreach (var contentItem in content)
            {
                var text = contentItem?["text"]?.GetValue<string>();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    parts.Add(text);
                }
            }
        }

        return string.Join(Environment.NewLine, parts);
    }
}
