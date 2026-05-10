# Contributing

Thanks for helping improve AI Governance Gateway.

This project is a reference implementation. Contributions should keep the code easy to understand while moving the foundation closer to production use.

## Good Contributions

- improve the governance pipeline without hiding the flow
- add tests for allow, block, human review, audit, and connector behavior
- improve framework mappings with clear evidence links
- add provider connectors behind `IAiConnector`
- improve docs, diagrams, examples, and learning material
- add production hardening in small, explainable steps

## Design Rules

- Keep controllers thin.
- Put governance behavior behind interfaces in `AiGovernance.Core`.
- Put implementations in `AiGovernance.Infrastructure`.
- Keep policy/risk rules config-driven where practical.
- Do not commit API keys, secrets, personal data, or real customer prompts.
- Do not claim enterprise readiness, certification, or legal compliance without deployment-specific evidence and review.
- Prefer small pull requests with tests.

## Local Setup

```powershell
dotnet restore
dotnet test AiGovernance.sln
dotnet run --project src\AiGovernance.Api --urls http://localhost:5000
```

Swagger opens at:

```text
http://localhost:5000/swagger
```

## Pull Request Checklist

- The solution builds.
- Tests pass.
- New behavior has tests.
- README or docs are updated when behavior changes.
- No generated `bin`, `obj`, logs, or secrets are included.
- Compliance wording avoids claiming certification unless there is external proof.
- Enterprise wording allows use but avoids unsupported enterprise-ready claims.
