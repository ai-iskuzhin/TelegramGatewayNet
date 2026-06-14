using System;

namespace TelegramGatewayNet
{
    /// <summary>
    /// Thrown when the Telegram Gateway API returns an unsuccessful response (<c>ok: false</c>)
    /// or when an HTTP-level error prevents a successful call.
    /// </summary>
    public sealed class TelegramGatewayException : Exception
    {
        /// <summary>
        /// The error code returned by the API in the <c>error</c> field (for example
        /// <c>ACCESS_TOKEN_INVALID</c>), or <c>null</c> for transport-level failures.
        /// </summary>
        public string? Error { get; }

        /// <summary>Creates an exception describing an API-level error.</summary>
        /// <param name="error">The error code from the API <c>error</c> field.</param>
        public TelegramGatewayException(string error)
            : base($"Telegram Gateway API returned an error: {error}")
        {
            Error = error;
        }

        /// <summary>Creates an exception with a custom message and optional inner exception.</summary>
        /// <param name="message">A human-readable description of the failure.</param>
        /// <param name="innerException">The underlying exception, if any.</param>
        public TelegramGatewayException(string message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
