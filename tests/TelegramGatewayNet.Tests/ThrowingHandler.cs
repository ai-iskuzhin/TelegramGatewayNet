namespace TelegramGatewayNet.Tests;

/// <summary>
/// A test <see cref="HttpMessageHandler"/> that always throws, simulating a transport-level
/// failure (no response received).
/// </summary>
internal sealed class ThrowingHandler : HttpMessageHandler
{
    private readonly Exception _exception;

    public ThrowingHandler(Exception exception)
    {
        _exception = exception;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        throw _exception;
    }
}
