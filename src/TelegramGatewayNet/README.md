# TelegramGatewayNet

Dependency-light .NET SDK for the [Telegram Gateway API](https://core.telegram.org/gateway/api) —
deliver verification codes (OTP) to users over Telegram.

## Install

```bash
dotnet add package TelegramGatewayNet --prerelease
```

Multi-targets `netstandard2.0`, `net8.0`, and `net10.0` — runs on .NET Framework 4.6.1+, .NET Core,
and modern .NET. Only the `netstandard2.0` asset depends on the `System.Text.Json` package.

## Quick Start

```csharp
using TelegramGatewayNet;
using TelegramGatewayNet.Requests;

using var client = new TelegramGatewayClient("YOUR_GATEWAY_API_TOKEN");

var result = await client.SendVerificationMessageAsync(new SendVerificationMessageRequest("+391234567890")
{
    CodeLength = 6,
    Ttl = 60,
    Payload = "my_payload_here",
    CallbackUrl = "https://my.webhook.here/auth"
});

if (result.Ok)
    Console.WriteLine(result.Value.RequestId);
else
    Console.WriteLine(result.Error);
```

## Supported API

| Method | Client API |
| --- | --- |
| `sendVerificationMessage` | `SendVerificationMessageAsync` |
| `checkSendAbility` | `CheckSendAbilityAsync` |
| `checkVerificationStatus` | `CheckVerificationStatusAsync` |
| `revokeVerificationMessage` | `RevokeVerificationMessageAsync` |

It also includes a `DeliveryReportValidator` for verifying the `X-Request-Signature` of delivery
report webhooks.

All methods return a `GatewayResult<T>`.

## Verifying a Code

```csharp
var result = await client.CheckVerificationStatusAsync(new CheckVerificationStatusRequest(requestId)
{
    Code = userEnteredCode
});

bool valid = result.Ok
    && result.Value.VerificationStatus?.Status == CodeVerificationStatus.CodeValid;
```

## Error Handling

An `ok: false` API response is a normal outcome, returned as a failed `GatewayResult<T>` whose
`Error` carries the API error code (for example `ACCESS_TOKEN_INVALID`) — it is not thrown. Only
transport failures and malformed responses throw `TelegramGatewayException` (which exposes
`StatusCode` and `ResponseBody` for diagnosis).

## Repository

Source, issue tracking, and full documentation:

https://github.com/ai-iskuzhin/TelegramGatewayNet
