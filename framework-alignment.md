# Framework Alignment

This project provides an implementation-oriented alignment catalog. It helps you explain which gateway controls support which framework expectations. It does not certify ISO/IEC 42001 compliance, satisfy every EU AI Act duty, or replace legal/security review.

## Included Frameworks

| Framework | Project Support |
| --- | --- |
| NIST AI RMF 1.0 | Govern, Map, Measure, and Manage mapped to policy, risk register, guards, policy decisions, and audit evidence. |
| NIST AI 600-1 | Generative AI risks mapped to prompt injection, sensitive data, harmful content, output filtering, and incident evidence support. |
| ISO/IEC 42001:2023 | AI management system support through policy, risk treatment, monitoring, and evidence. |
| OWASP LLM Top 10 2025 | LLM01 through LLM10 mapped to gateway controls or tracked gaps. |
| NIST CSF 2.0 | Govern, Identify, Protect, Detect, Respond, and Recover mapped to security controls and evidence. |
| EU AI Act GPAI | Supports evidence tracking for GPAI obligations, but provider duties need external documentation and legal ownership. |

## How To Inspect Alignment

Run the API and call:

```http
GET http://localhost:5000/v1/governance/frameworks
```

Each item shows:

- framework id and source URL
- mapped components
- requirement title and description
- evidence files or endpoints
- status: `Implemented`, `PartiallyImplemented`, or `GapTracked`

## Practical Gaps To Close Before Production

- Add durable audit storage instead of in-memory logging.
- Add authentication, authorization, tenant isolation, rate limits, and model budget controls.
- Add adversarial test sets for jailbreaks, bias, harmful content, sensitive data, and misinformation.
- Add SBOM/dependency scanning and provider due diligence.
- Add incident runbooks, alerting, reporting, and recovery steps.
- If you provide or significantly modify a GPAI model, add technical documentation, copyright policy evidence, training-content summary, systemic risk evaluation, and incident reporting workflow.
