using System.Net;
using System.Text.Json;
using TelegramGatewayNet;
using TelegramGatewayNet.Models;
using TelegramGatewayNet.Requests;

namespace TelegramGatewayNet.Tests;

public sealed class TelegramGatewayClientTests
{
    private static TelegramGatewayClient CreateClient(HttpMessageHandler handler, string token = "TEST_TOKEN")
    {
        var httpClient = new HttpClient(handler);
        return new TelegramGatewayClient(token, httpClient, "https://example.test/");
    }

    [Fact]
    public async Task SendVerificationMessageAsync_PostsJsonWithBearerTokenToEndpoint()
    {
        using var handler = new RecordingHandler("""
            {
              "ok": true,
              "result": {
                "request_id": "abc123",
                "phone_number": "+391234567890",
                "request_cost": 0.01,
                "remaining_balance": 9.99,
                "delivery_status": { "status": "sent", "updated_at": 1700000000 }
              }
            }
            """);
        var client = CreateClient(handler);

        var result = await client.SendVerificationMessageAsync(new SendVerificationMessageRequest("+391234567890")
        {
            CodeLength = 6,
            Ttl = 60,
            Payload = "p1"
        });

        Assert.Equal("https://example.test/sendVerificationMessage", handler.RequestUri?.ToString());
        Assert.Equal(HttpMethod.Post, handler.Method);
        Assert.Equal("Bearer TEST_TOKEN", handler.AuthorizationHeader);

        Assert.True(result.Ok);
        Assert.Null(result.Error);
        var status = result.Value!;
        Assert.Equal("abc123", status.RequestId);
        Assert.Equal("+391234567890", status.PhoneNumber);
        Assert.Equal(0.01, status.RequestCost);
        Assert.Equal(9.99, status.RemainingBalance);
        Assert.NotNull(status.DeliveryStatus);
        Assert.Equal(MessageDeliveryStatus.Sent, status.DeliveryStatus!.Status);

        using var document = JsonDocument.Parse(handler.Body!);
        var root = document.RootElement;
        Assert.Equal("+391234567890", root.GetProperty("phone_number").GetString());
        Assert.Equal(6, root.GetProperty("code_length").GetInt32());
        Assert.Equal(60, root.GetProperty("ttl").GetInt32());
        Assert.Equal("p1", root.GetProperty("payload").GetString());
    }

    [Fact]
    public async Task SendVerificationMessageAsync_OmitsNullOptionalFields()
    {
        using var handler = new RecordingHandler("""
            { "ok": true, "result": { "request_id": "r", "phone_number": "+1", "request_cost": 0 } }
            """);
        var client = CreateClient(handler);

        await client.SendVerificationMessageAsync(new SendVerificationMessageRequest("+1"));

        using var document = JsonDocument.Parse(handler.Body!);
        var root = document.RootElement;
        Assert.True(root.TryGetProperty("phone_number", out _));
        Assert.False(root.TryGetProperty("code", out _));
        Assert.False(root.TryGetProperty("code_length", out _));
        Assert.False(root.TryGetProperty("ttl", out _));
        Assert.False(root.TryGetProperty("callback_url", out _));
    }

    [Fact]
    public async Task CheckVerificationStatusAsync_ParsesVerificationStatus()
    {
        using var handler = new RecordingHandler("""
            {
              "ok": true,
              "result": {
                "request_id": "abc123",
                "phone_number": "+391234567890",
                "request_cost": 0.0,
                "verification_status": {
                  "status": "code_max_attempts_exceeded",
                  "updated_at": 1700000123,
                  "code_entered": "000000"
                }
              }
            }
            """);
        var client = CreateClient(handler);

        var result = await client.CheckVerificationStatusAsync(new CheckVerificationStatusRequest("abc123")
        {
            Code = "123456"
        });

        Assert.True(result.Ok);
        var status = result.Value!;
        Assert.NotNull(status.VerificationStatus);
        Assert.Equal(CodeVerificationStatus.CodeMaxAttemptsExceeded, status.VerificationStatus!.Status);
        Assert.Equal("000000", status.VerificationStatus.CodeEntered);

        using var document = JsonDocument.Parse(handler.Body!);
        Assert.Equal("abc123", document.RootElement.GetProperty("request_id").GetString());
        Assert.Equal("123456", document.RootElement.GetProperty("code").GetString());
    }

    [Fact]
    public async Task RevokeVerificationMessageAsync_ReturnsBooleanResult()
    {
        using var handler = new RecordingHandler("""{ "ok": true, "result": true }""");
        var client = CreateClient(handler);

        var result = await client.RevokeVerificationMessageAsync(new RevokeVerificationMessageRequest("abc123"));

        Assert.True(result.Ok);
        Assert.True(result.Value);
        Assert.Equal("https://example.test/revokeVerificationMessage", handler.RequestUri?.ToString());
    }

    [Fact]
    public async Task UnsuccessfulResponse_ReturnsFailureWithErrorCode_DoesNotThrow()
    {
        using var handler = new RecordingHandler("""{ "ok": false, "error": "ACCESS_TOKEN_INVALID" }""");
        var client = CreateClient(handler);

        var result = await client.CheckSendAbilityAsync(new CheckSendAbilityRequest("+1"));

        Assert.False(result.Ok);
        Assert.Equal("ACCESS_TOKEN_INVALID", result.Error);
        Assert.Null(result.Value);
    }

    [Fact]
    public async Task MalformedResponse_ThrowsWithStatusCodeAndBody()
    {
        using var handler = new RecordingHandler("not json", HttpStatusCode.BadGateway);
        var client = CreateClient(handler);

        var ex = await Assert.ThrowsAsync<TelegramGatewayException>(() =>
            client.CheckSendAbilityAsync(new CheckSendAbilityRequest("+1")));

        Assert.Equal(502, ex.StatusCode);
        Assert.Equal("not json", ex.ResponseBody);
    }

    [Fact]
    public async Task TransportFailure_ThrowsWithoutStatusCode()
    {
        using var handler = new ThrowingHandler(new HttpRequestException("connection refused"));
        var client = CreateClient(handler);

        var ex = await Assert.ThrowsAsync<TelegramGatewayException>(() =>
            client.CheckSendAbilityAsync(new CheckSendAbilityRequest("+1")));

        Assert.Null(ex.StatusCode);
        Assert.Null(ex.ResponseBody);
        Assert.IsType<HttpRequestException>(ex.InnerException);
    }

    [Fact]
    public void Constructor_RejectsEmptyToken()
    {
        Assert.Throws<ArgumentException>(() => new TelegramGatewayClient(""));
    }

    [Fact]
    public async Task UnknownStatusValue_DeserializesToUnknown()
    {
        using var handler = new RecordingHandler("""
            {
              "ok": true,
              "result": {
                "request_id": "r", "phone_number": "+1", "request_cost": 0,
                "delivery_status": { "status": "teleported", "updated_at": 1 }
              }
            }
            """);
        var client = CreateClient(handler);

        var result = await client.CheckSendAbilityAsync(new CheckSendAbilityRequest("+1"));

        Assert.True(result.Ok);
        Assert.Equal(MessageDeliveryStatus.Unknown, result.Value!.DeliveryStatus!.Status);
    }
}
