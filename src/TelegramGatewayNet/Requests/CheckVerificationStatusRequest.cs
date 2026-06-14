using System.Text.Json.Serialization;

namespace TelegramGatewayNet.Requests
{
    /// <summary>
    /// Parameters for the <c>checkVerificationStatus</c> method.
    /// </summary>
    public sealed class CheckVerificationStatusRequest
    {
        /// <summary>Creates a new request for the given verification request id.</summary>
        /// <param name="requestId">The unique identifier of the verification request to check.</param>
        public CheckVerificationStatusRequest(string requestId)
        {
            RequestId = requestId;
        }

        /// <summary>The unique identifier of the verification request whose status to check.</summary>
        [JsonPropertyName("request_id")]
        public string RequestId { get; set; }

        /// <summary>
        /// Optional. The code entered by the user. If provided, the method checks whether the code
        /// is valid for the relevant request.
        /// </summary>
        [JsonPropertyName("code")]
        public string? Code { get; set; }
    }
}
