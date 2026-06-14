# TelegramGatewayNet

[![CI](https://github.com/ai-iskuzhin/TelegramGatewayNet/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/ai-iskuzhin/TelegramGatewayNet/actions/workflows/ci.yml)
[![Release](https://github.com/ai-iskuzhin/TelegramGatewayNet/actions/workflows/release.yml/badge.svg)](https://github.com/ai-iskuzhin/TelegramGatewayNet/actions/workflows/release.yml)
[![License](https://img.shields.io/github/license/ai-iskuzhin/TelegramGatewayNet?style=flat-square)](https://github.com/ai-iskuzhin/TelegramGatewayNet/blob/main/LICENSE)
[![Targets](https://img.shields.io/badge/targets-netstandard2.0%20%7C%20net8.0%20%7C%20net10.0-512BD4?logo=dotnet&style=flat-square)](https://dotnet.microsoft.com/)

| Package | Latest version | Downloads |
| :--- | :---: | :---: |
| `TelegramGatewayNet` | [![TelegramGatewayNet NuGet](https://img.shields.io/nuget/vpre/TelegramGatewayNet?logo=nuget&style=flat-square)](https://www.nuget.org/packages/TelegramGatewayNet) | [![TelegramGatewayNet Downloads](https://img.shields.io/nuget/dt/TelegramGatewayNet?style=flat-square)](https://www.nuget.org/packages/TelegramGatewayNet) |

A clean, dependency-light .NET SDK for the [Telegram Gateway API](https://core.telegram.org/gateway/api).
Use it to deliver verification codes (OTP) to users over Telegram — a cheaper, more secure
alternative to SMS.

Current status: latest preview is `0.1.0-preview.1`. The SDK surface covers every documented
Gateway API method plus delivery report (webhook) signature validation.

## Installation

```bash
dotnet add package TelegramGatewayNet --prerelease
```

The library multi-targets `netstandard2.0`, `net8.0`, and `net10.0`. The `netstandard2.0` asset
runs on .NET Framework 4.6.1+, .NET Core 2.0+, Mono, and Unity; modern consumers resolve the
`net8.0`/`net10.0` assets, which use the in-box `System.Text.Json` and carry no extra dependency.
Only the `netstandard2.0` asset pulls in the `System.Text.Json` package.

For local development you can reference the project directly:

```xml
<ProjectReference Include="src/TelegramGatewayNet/TelegramGatewayNet.csproj" />
```

## Quick Start

Obtain an access token from your [Telegram Gateway account settings](https://gateway.telegram.org/),
then:

```csharp
using TelegramGatewayNet;
using TelegramGatewayNet.Requests;

using var client = new TelegramGatewayClient("YOUR_GATEWAY_API_TOKEN");

var status = await client.SendVerificationMessageAsync(new SendVerificationMessageRequest("+391234567890")
{
    CodeLength = 6,                          // ignored if you set Code yourself
    Ttl = 60,                                // refunded if undelivered within 60s
    Payload = "my_payload_here",             // not shown to the user
    CallbackUrl = "https://my.webhook/auth"  // optional delivery reports
});

Console.WriteLine(status.RequestId);
```

The token is sent as an `Authorization: Bearer <token>` header on every request, and all calls are
made as `application/json` POSTs.

## Supported Methods

| Telegram method | Client API | Returns |
| --- | --- | --- |
| `sendVerificationMessage` | `SendVerificationMessageAsync` | `RequestStatus` |
| `checkSendAbility` | `CheckSendAbilityAsync` | `RequestStatus` |
| `checkVerificationStatus` | `CheckVerificationStatusAsync` | `RequestStatus` |
| `revokeVerificationMessage` | `RevokeVerificationMessageAsync` | `bool` |

## Checking Send Ability First

To verify a user can receive a code before sending one (and to avoid paying twice), call
`checkSendAbility`, then pass the returned `request_id` to `sendVerificationMessage`:

```csharp
var ability = await client.CheckSendAbilityAsync(new CheckSendAbilityRequest("+391234567890"));

var status = await client.SendVerificationMessageAsync(new SendVerificationMessageRequest("+391234567890")
{
    RequestId = ability.RequestId   // makes the send free of charge
});
```

## Verifying a User's Code

When you let Telegram generate the code, verify the user's input with `checkVerificationStatus`:

```csharp
using TelegramGatewayNet.Models;

var status = await client.CheckVerificationStatusAsync(new CheckVerificationStatusRequest(requestId)
{
    Code = userEnteredCode
});

bool valid = status.VerificationStatus?.Status == CodeVerificationStatus.CodeValid;
```

Even if you set the code yourself, calling this method after the user enters a code lets Telegram
track your verification conversion rate.

## Receiving Delivery Reports

When you pass a `CallbackUrl`, the Gateway API POSTs a `RequestStatus` JSON object to that URL.
Verify the integrity of each report using the `X-Request-Timestamp` and `X-Request-Signature`
headers:

```csharp
using TelegramGatewayNet.Webhooks;

var validator = new DeliveryReportValidator("YOUR_GATEWAY_API_TOKEN");

// rawBody is the exact, unparsed request body.
bool authentic = validator.IsValid(timestampHeader, rawBody, signatureHeader);

if (!authentic)
{
    // Reject the report — it did not originate from Telegram.
}
```

The validator computes the hex-encoded `HMAC-SHA256` of `timestamp + "\n" + rawBody`, keyed by the
`SHA256` of your API token, and compares it to the supplied signature in constant time. Also
check that the timestamp is recent to guard against replayed reports.

## Error Handling

API responses with `ok: false`, transport failures, and malformed payloads are surfaced as
`TelegramGatewayException`. For API-level errors, the `Error` property carries the code returned by
Telegram (for example `ACCESS_TOKEN_INVALID`):

```csharp
try
{
    await client.SendVerificationMessageAsync(new SendVerificationMessageRequest("+391234567890"));
}
catch (TelegramGatewayException ex)
{
    Console.WriteLine(ex.Error); // e.g. "ACCESS_TOKEN_INVALID", or null for transport errors
}
```

## Dependency Injection

The client accepts a caller-provided `HttpClient`, so it integrates with `IHttpClientFactory`:

```csharp
services.AddHttpClient("telegram-gateway");

services.AddSingleton<ITelegramGatewayClient>(sp =>
{
    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("telegram-gateway");
    return new TelegramGatewayClient(configuration["TelegramGateway:Token"]!, httpClient);
});
```

When you supply your own `HttpClient`, the SDK does not dispose it.

## Development

Run all tests:

```bash
dotnet test TelegramGatewayNet.sln
```

Pack the NuGet package:

```bash
dotnet pack src/TelegramGatewayNet/TelegramGatewayNet.csproj --configuration Release --output artifacts/packages
```

## Documentation

- [Release process](docs/release.md)
- [Changelog](CHANGELOG.md)
- [Telegram Gateway API reference](docs/telegram-gateway-api.md)

## License

[MIT](LICENSE)
