using System;

namespace TelegramGatewayNet
{
    /// <summary>
    /// Thrown when a Telegram Gateway API call cannot produce a valid result — that is, a
    /// transport-level failure (network, DNS, TLS, timeout) or a malformed/unexpected response
    /// body.
    /// <para>
    /// Note: a normal <c>ok: false</c> API response (for example <c>PHONE_NUMBER_INVALID</c>) is
    /// <b>not</b> thrown; it is returned as a failed
    /// <see cref="TelegramGatewayNet.Models.GatewayResult{T}"/> with its <c>Error</c> set.
    /// </para>
    /// </summary>
    public sealed class TelegramGatewayException : Exception
    {
        /// <summary>
        /// The HTTP status code of the response, when one was received. <c>null</c> for
        /// transport-level failures where no response arrived.
        /// </summary>
        public int? StatusCode { get; }

        /// <summary>
        /// The raw response body (possibly truncated), when one was received and could be read.
        /// Useful for diagnosing unexpected or malformed responses. <c>null</c> for
        /// transport-level failures.
        /// </summary>
        public string? ResponseBody { get; }

        /// <summary>Creates an exception for a transport-level failure (no response received).</summary>
        /// <param name="message">A human-readable description of the failure.</param>
        /// <param name="innerException">The underlying exception, if any.</param>
        public TelegramGatewayException(string message, Exception? innerException)
            : base(message, innerException)
        {
        }

        /// <summary>Creates an exception for a malformed or unexpected response.</summary>
        /// <param name="message">A human-readable description of the failure.</param>
        /// <param name="statusCode">The HTTP status code of the response.</param>
        /// <param name="responseBody">The raw response body (possibly truncated).</param>
        /// <param name="innerException">The underlying exception, if any.</param>
        public TelegramGatewayException(string message, int statusCode, string? responseBody, Exception? innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ResponseBody = responseBody;
        }
    }
}
