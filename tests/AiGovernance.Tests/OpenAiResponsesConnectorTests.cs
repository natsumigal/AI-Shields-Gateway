using System.Net;
using System.Text.Json.Nodes;
using AiGovernance.Core.Configuration;
using AiGovernance.Core.Models;
using AiGovernance.Infrastructure.Ai;
using Microsoft.Extensions.Options;

namespace AiGovernance.Tests;

public sealed class OpenAiResponsesConnectorTests
{
    [Fact]
    public async Task GenerateAsync_SendsResponsesApiRequestAndReadsOutputText()
    {
        const string apiKeyVariable = "OPENAI_API_KEY_TEST";
        var previousValue = Environment.GetEnvironmentVariable(apiKeyVariable);
        Environment.SetEnvironmentVariable(apiKeyVariable, "test-key");

        try
        {
            var handler = new CapturingHandler("""{"output_text":"Hello from the real connector path."}""");
            var connector = new OpenAiResponsesConnector(
                new HttpClient(handler),
                new TestOptionsMonitor<AiConnectorOptions>(CreateOptions(apiKeyVariable)));

            var response = await connector.GenerateAsync(CreateContext(), CancellationToken.None);

            Assert.Equal("OpenAI", response.Provider);
            Assert.Equal("Hello from the real connector path.", response.Content);
            Assert.Equal(HttpMethod.Post, handler.Request?.Method);
            Assert.Equal("Bearer test-key", handler.Request?.Headers.Authorization?.ToString());

            var requestBody = handler.RequestBody;
            var payload = JsonNode.Parse(requestBody)!;

            Assert.Equal("gpt-5.2", payload["model"]!.GetValue<string>());
            Assert.Equal("Explain audit logging.", payload["input"]!.GetValue<string>());
            Assert.Equal("test-correlation-001", payload["metadata"]!["correlation_id"]!.GetValue<string>());
        }
        finally
        {
            Environment.SetEnvironmentVariable(apiKeyVariable, previousValue);
        }
    }

    [Fact]
    public async Task GenerateAsync_WhenApiKeyMissing_ThrowsClearError()
    {
        const string apiKeyVariable = "OPENAI_API_KEY_MISSING_TEST";
        Environment.SetEnvironmentVariable(apiKeyVariable, null);

        var connector = new OpenAiResponsesConnector(
            new HttpClient(new CapturingHandler("""{"output_text":"unused"}""")),
            new TestOptionsMonitor<AiConnectorOptions>(CreateOptions(apiKeyVariable)));

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            connector.GenerateAsync(CreateContext(), CancellationToken.None));

        Assert.Contains(apiKeyVariable, exception.Message);
    }

    private static AiConnectorOptions CreateOptions(string apiKeyVariable)
    {
        return new AiConnectorOptions
        {
            Mode = "OpenAI",
            Provider = "OpenAI",
            Model = "gpt-5.2",
            Endpoint = "https://api.openai.com/v1/responses",
            ApiKeyEnvironmentVariable = apiKeyVariable,
            SystemInstruction = "You are governed by AI safety policy.",
            MaxOutputTokens = 300,
            Temperature = 0.1m
        };
    }

    private static GovernanceContext CreateContext()
    {
        return new GovernanceContext(new GovernanceRequest
        {
            Prompt = "Explain audit logging.",
            UserId = "test-user",
            TenantId = "test-tenant",
            Purpose = "mock-api-test",
            CorrelationId = "test-correlation-001"
        });
    }

    private sealed class CapturingHandler : HttpMessageHandler
    {
        private readonly string _responseBody;

        public CapturingHandler(string responseBody)
        {
            _responseBody = responseBody;
        }

        public HttpRequestMessage? Request { get; private set; }

        public string RequestBody { get; private set; } = "";

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Request = request;
            RequestBody = request.Content is null
                ? ""
                : await request.Content.ReadAsStringAsync(cancellationToken);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_responseBody)
            };
        }
    }

    private sealed class TestOptionsMonitor<T> : IOptionsMonitor<T>
    {
        public TestOptionsMonitor(T currentValue)
        {
            CurrentValue = currentValue;
        }

        public T CurrentValue { get; }

        public T Get(string? name)
        {
            return CurrentValue;
        }

        public IDisposable? OnChange(Action<T, string?> listener)
        {
            return null;
        }
    }
}
