# Custom Policy Engine

This example replaces the default configurable policy engine with a stricter implementation.

Copy `StrictPolicyEngine.cs` into your infrastructure project, then register it:

```csharp
builder.Services.AddScoped<IPolicyEngine, StrictPolicyEngine>();
```

Use this pattern when policy decisions require code, not just JSON thresholds.
