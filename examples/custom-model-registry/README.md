# Custom Model Registry

This example replaces the JSON model registry with code-backed model profiles.

Copy `TenantAwareModelRegistry.cs` into your infrastructure project, then register it:

```csharp
builder.Services.AddSingleton<IModelRegistry, TenantAwareModelRegistry>();
```

Use this pattern when model approval depends on a tenant, database, remote config service, or enterprise catalog.
