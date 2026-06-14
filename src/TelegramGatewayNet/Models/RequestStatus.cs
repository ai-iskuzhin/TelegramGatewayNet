using System.Text.Json.Serialization;

namespace TelegramGatewayNet.Models
{
    /// <summary>
    /// Represents the status of a verification message request. Returned by
    /// <c>sendVerificationMessage</c>, <c>checkSendAbility</c> and <c>checkVerificationStatus</c>,
    /// and also delivered to a configured <c>callback_url</c> as a delivery report.
    /// </summary>
    public sealed class RequestStatus
    {
        /// <summary>Unique identifier of the verification request.</summary>
        [JsonPropertyName("request_id")]
        public string RequestId { get; set; } = string.Empty;

        /// <summary>The phone number to which the verification code was sent, in E.164 format.</summary>
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>Total request cost incurred by either <c>checkSendAbility</c> or <c>sendVerificationMessage</c>.</summary>
        [JsonPropertyName("request_cost")]
        public double RequestCost { get; set; }

        /// <summary>Optional. If <c>true</c>, the request fee was refunded.</summary>
        [JsonPropertyName("is_refunded")]
        public bool? IsRefunded { get; set; }

        /// <summary>Optional. Remaining balance in credits. Returned only for requests that incur a charge.</summary>
        [JsonPropertyName("remaining_balance")]
        public double? RemainingBalance { get; set; }

        /// <summary>Optional. The current message delivery status. Returned only if a verification message was sent.</summary>
        [JsonPropertyName("delivery_status")]
        public DeliveryStatus? DeliveryStatus { get; set; }

        /// <summary>Optional. The current status of the verification process.</summary>
        [JsonPropertyName("verification_status")]
        public VerificationStatus? VerificationStatus { get; set; }

        /// <summary>Optional. Custom payload if it was provided in the request.</summary>
        [JsonPropertyName("payload")]
        public string? Payload { get; set; }
    }
}
