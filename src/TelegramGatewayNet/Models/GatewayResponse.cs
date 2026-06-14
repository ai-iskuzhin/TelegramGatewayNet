using System.Text.Json.Serialization;

namespace TelegramGatewayNet.Models
{
    /// <summary>
    /// The envelope returned by every Telegram Gateway API method. When <see cref="Ok"/> is
    /// <c>true</c> the payload is in <see cref="Result"/>; otherwise <see cref="Error"/> describes
    /// the failure (for example <c>ACCESS_TOKEN_INVALID</c>).
    /// </summary>
    /// <typeparam name="T">The type of the <c>result</c> payload.</typeparam>
    public sealed class GatewayResponse<T>
    {
        /// <summary>Whether the request was successful.</summary>
        [JsonPropertyName("ok")]
        public bool Ok { get; set; }

        /// <summary>The result payload, present when <see cref="Ok"/> is <c>true</c>.</summary>
        [JsonPropertyName("result")]
        public T? Result { get; set; }

        /// <summary>The error code, present when <see cref="Ok"/> is <c>false</c>.</summary>
        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }
}
