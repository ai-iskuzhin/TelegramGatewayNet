using System.Text.Json.Serialization;
using TelegramGatewayNet.Serialization;

namespace TelegramGatewayNet.Models
{
    /// <summary>
    /// Represents the verification status of a code.
    /// </summary>
    public sealed class VerificationStatus
    {
        /// <summary>The current status of the verification process.</summary>
        [JsonPropertyName("status")]
        [JsonConverter(typeof(SnakeCaseEnumConverter<CodeVerificationStatus>))]
        public CodeVerificationStatus Status { get; set; }

        /// <summary>The Unix timestamp (seconds) when the status was last updated.</summary>
        [JsonPropertyName("updated_at")]
        public long UpdatedAt { get; set; }

        /// <summary>Optional. The code entered by the user.</summary>
        [JsonPropertyName("code_entered")]
        public string? CodeEntered { get; set; }
    }
}
