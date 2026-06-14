# Changelog

All notable changes to `TelegramGatewayNet` will be documented in this file.

The project uses Semantic Versioning. Versions below `1.0.0` are preview releases and may include public API changes while the SDK contracts are validated against the live Telegram Gateway API.

## Unreleased

No changes yet.

## 0.1.0-preview.2

### Changed

- Multi-target the package: `netstandard2.0;net8.0;net10.0`. The `System.Text.Json` dependency is now conditional on `netstandard2.0` only — `net8.0` and `net10.0` consumers use the in-box version and carry no extra dependency.

## 0.1.0-preview.1

### Added

- Repository skeleton: `netstandard2.0` SDK library targeting broad .NET compatibility, with a `net10.0` test project.
- `TelegramGatewayClient` / `ITelegramGatewayClient` with support for:
  - `sendVerificationMessage` (`SendVerificationMessageAsync`)
  - `checkSendAbility` (`CheckSendAbilityAsync`)
  - `checkVerificationStatus` (`CheckVerificationStatusAsync`)
  - `revokeVerificationMessage` (`RevokeVerificationMessageAsync`)
- Typed request and response models using `System.Text.Json`, including `RequestStatus`, `DeliveryStatus`, and `VerificationStatus`.
- Strongly typed `MessageDeliveryStatus` and `CodeVerificationStatus` enums with a forward-compatible `snake_case` converter that maps unknown values to `Unknown`.
- `DeliveryReportValidator` for verifying the `X-Request-Signature` HMAC-SHA256 signature of delivery report webhooks, with constant-time comparison.
- `TelegramGatewayException` carrying the API error code for `ok: false`, transport, and parse failures.
- Unit tests for client request shape, response parsing, error handling, enum fallback, and webhook signature validation.
- GitHub Actions workflows for CI, release creation, NuGet publishing, and GitHub Packages publishing.
- `README.md`, package README, `LICENSE`, and release documentation.
