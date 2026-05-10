# Security Policy

## Supported Versions

This project is currently a reference implementation. Security fixes should target the default branch.

## Reporting A Vulnerability

Please do not open a public issue for secrets, authentication bypasses, data exposure, or provider-integration vulnerabilities.

Use GitHub private vulnerability reporting if it is enabled for the repository. If it is not enabled yet, contact the repository maintainers privately.

## Secret Handling

- Do not commit `.env` files.
- Do not commit API keys or provider credentials.
- Use environment variables or a secret manager for `OPENAI_API_KEY`.
- Treat prompts, outputs, audit logs, and metadata as potentially sensitive.

## Production Warning

Enterprise and commercial use are allowed under the MIT license, but an unmodified deployment should not be described as enterprise-ready, certified, or compliant. Before production or enterprise use, add:

- authentication and authorization
- persistent encrypted audit storage
- tenant isolation
- rate limiting
- secret management
- monitoring and alerting
- incident response workflow
- legal and compliance review
