using TelegramGatewayNet.Webhooks;

namespace TelegramGatewayNet.Tests;

public sealed class DeliveryReportValidatorTests
{
    private const string Token = "TEST_TOKEN";
    private const string Timestamp = "1700000000";
    private const string Body = """{"request_id":"abc"}""";

    // Independently computed: hex(HMAC-SHA256(timestamp + "\n" + body, SHA256(token))).
    private const string KnownSignature = "ba63fd756bd9eefdac399bc598347c4754b01546d595bfe332b8e8d54aee773e";

    [Fact]
    public void ComputeSignature_MatchesReferenceImplementation()
    {
        var validator = new DeliveryReportValidator(Token);

        Assert.Equal(KnownSignature, validator.ComputeSignature(Timestamp, Body));
    }

    [Fact]
    public void IsValid_ReturnsTrueForCorrectSignature()
    {
        var validator = new DeliveryReportValidator(Token);

        Assert.True(validator.IsValid(Timestamp, Body, KnownSignature));
    }

    [Fact]
    public void IsValid_IsCaseInsensitiveOnHex()
    {
        var validator = new DeliveryReportValidator(Token);

        Assert.True(validator.IsValid(Timestamp, Body, KnownSignature.ToUpperInvariant()));
    }

    [Fact]
    public void IsValid_ReturnsFalseForTamperedBody()
    {
        var validator = new DeliveryReportValidator(Token);

        Assert.False(validator.IsValid(Timestamp, """{"request_id":"xyz"}""", KnownSignature));
    }

    [Fact]
    public void IsValid_ReturnsFalseForWrongToken()
    {
        var validator = new DeliveryReportValidator("DIFFERENT_TOKEN");

        Assert.False(validator.IsValid(Timestamp, Body, KnownSignature));
    }

    [Fact]
    public void IsValid_ReturnsFalseForEmptySignature()
    {
        var validator = new DeliveryReportValidator(Token);

        Assert.False(validator.IsValid(Timestamp, Body, ""));
    }
}
