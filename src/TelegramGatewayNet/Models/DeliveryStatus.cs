using System.Text.Json.Serialization;
using TelegramGatewayNet.Serialization;

namespace TelegramGatewayNet.Models
{
    /// <summary>
    /// Represents the delivery status of a verification message.
    /// </summary>
    public sealed class DeliveryStatus
    {
        /// <summary>The current status of the message.</summary>
        [JsonPropertyName("status")]
        [JsonConverter(typeof(SnakeCaseEnumConverter<MessageDeliveryStatus>))]
        public MessageDeliveryStatus Status { get; set; }

        /// <summary>The Unix timestamp (seconds) when the status was last updated.</summary>
        [JsonPropertyName("updated_at")]
        public long UpdatedAt { get; set; }
    }
}
