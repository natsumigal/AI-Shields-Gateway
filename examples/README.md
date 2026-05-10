# Examples

These examples show common ways to customize the gateway.

They are intentionally small and are not included in the main solution build. Copy the example you want into your own project or into `src/AiGovernance.Infrastructure`, then register it in dependency injection.

| Example | What it shows |
|---|---|
| `custom-policy-engine` | Replace policy decisions with stricter local rules |
| `custom-ai-connector` | Add an OpenAI Responses-style HTTP connector |
| `custom-model-registry` | Replace JSON model registry with an in-memory tenant-aware registry |

Typical registration pattern:

```csharp
builder.Services.AddScoped<IPolicyEngine, StrictPolicyEngine>();
builder.Services.AddHttpClient<IAiConnector, OpenAiResponsesConnector>();
builder.Services.AddSingleton<IModelRegistry, TenantAwareModelRegistry>();
```

Use one implementation per interface in your final app.
