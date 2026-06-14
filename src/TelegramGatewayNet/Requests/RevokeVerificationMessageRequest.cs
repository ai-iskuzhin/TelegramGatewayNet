using System.Text.Json.Serialization;

namespace TelegramGatewayNet.Requests
{
    /// <summary>
    /// Parameters for the <c>revokeVerificationMessage</c> method.
    /// </summary>
    public sealed class RevokeVerificationMessageRequest
    {
        /// <summary>Creates a new request for the given verification request id.</summary>
        /// <param name="requestId">The unique identifier of the request to revoke.</param>
        public RevokeVerificationMessageRequest(string requestId)
        {
            RequestId = requestId;
        }

        /// <summary>The unique identifier of the request whose verification message to revoke.</summary>
        [JsonPropertyName("request_id")]
        public string RequestId { get; set; }
    }
}
