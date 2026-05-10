# Custom AI Connector

This example shows how to add an OpenAI Responses-style connector behind the governance pipeline.

Copy `OpenAiResponsesConnector.cs` into your infrastructure project, then register it:

```csharp
builder.Services.AddHttpClient<IAiConnector, OpenAiResponsesConnector>();
```

Example configuration:

```json
{
  "AiConnector": {
    "Provider": "OpenAI",
    "Model": "your-model-id",
    "Endpoint": "https://api.openai.com/v1/responses",
    "ApiKey": "",
    "RequestTimeoutSeconds": 30
  }
}
```

Keep secrets out of JSON in real deployments:

```powershell
$env:AiConnector__ApiKey = "your-openai-api-key"
```
