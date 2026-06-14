# Changelog

All notable changes to `TelegramGatewayNet` will be documented in this file.

The project uses Semantic Versioning. Versions below `1.0.0` are preview releases and may include public API changes while the SDK contracts are validated against the live Telegram Gateway API.

## Unreleased

No changes yet.

## 1.0.0

First stable release. The public API has been validated against the live Telegram Gateway API and is now covered by Semantic Versioning guarantees. No API changes since `0.1.0-preview.3`.

Public surface:

- `TelegramGatewayClient` / `ITelegramGatewayClient` covering `sendVerificationMessage`, `checkSendAbility`, `checkVerificationStatus`, and `revokeVerificationMessage`, each returning `GatewayResult<T>`.
- Result-based error model: `ok: false` is returned as data via `GatewayResult<T>.Error`; only transport failures and malformed responses throw `TelegramGatewayException` (which exposes `StatusCode` and `ResponseBody`).
- Typed request/response models, forward-compatible `snake_case` status enums, and `DeliveryReportValidator` for HMAC-SHA256 webhook signature verification.
- Multi-targets `netstandard2.0`, `net8.0`, and `net10.0`; `System.Text.Json` is a `netstandard2.0`-only dependency.

## 0.1.0-preview.3

### Added

- `SendVerificationMessageRequest` constructor overloads for each code-selection mode: `(phoneNumber)` (Telegram generates a default-length code), `(phoneNumber, int codeLength)` (Telegram generates the given length), and `(phoneNumber, string code)` (use your own code).

### Changed

- **Result-based error model (breaking).** Client methods now return `GatewayResult<T>` instead of the bare payload. An `ok: false` API response (for example `PHONE_NUMBER_INVALID`) is a normal result reported via `GatewayResult<T>.Error` and is no longer thrown.
- `TelegramGatewayException` is now thrown only for failures that prevent obtaining a valid response — transport errors and malformed/unexpected response bodies. It exposes `StatusCode` and `ResponseBody` for diagnosis, and no longer carries an `Error` code (read `GatewayResult<T>.Error` instead). A cancelled token surfaces as the standard `OperationCanceledException`.

### Removed

- Removed the GitHub Packages publishing workflow. Packages are published to NuGet.org only.

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
