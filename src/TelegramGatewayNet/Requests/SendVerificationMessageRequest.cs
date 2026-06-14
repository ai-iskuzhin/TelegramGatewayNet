using System.Text.Json.Serialization;

namespace TelegramGatewayNet.Requests
{
    /// <summary>
    /// Parameters for the <c>sendVerificationMessage</c> method.
    /// </summary>
    public sealed class SendVerificationMessageRequest
    {
        /// <summary>
        /// Creates a new request for the given phone number.
        /// </summary>
        /// <param name="phoneNumber">The recipient phone number in E.164 format (for example <c>+391234567890</c>).</param>
        public SendVerificationMessageRequest(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        /// <summary>The phone number to which to send a verification message, in E.164 format.</summary>
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Optional. The unique identifier of a previous request from <c>checkSendAbility</c>.
        /// If provided, this request will be free of charge.
        /// </summary>
        [JsonPropertyName("request_id")]
        public string? RequestId { get; set; }

        /// <summary>
        /// Optional. Username of the verified Telegram channel from which the code will be sent.
        /// The channel must be owned by the same account that owns the API token.
        /// </summary>
        [JsonPropertyName("sender_username")]
        public string? SenderUsername { get; set; }

        /// <summary>
        /// Optional. The verification code to use. Only fully numeric strings between 4 and 8
        /// characters are supported. If set, <see cref="CodeLength"/> is ignored.
        /// </summary>
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        /// <summary>
        /// Optional. The length of the verification code if Telegram should generate it (4 to 8).
        /// Only relevant when <see cref="Code"/> is not set.
        /// </summary>
        [JsonPropertyName("code_length")]
        public int? CodeLength { get; set; }

        /// <summary>Optional. An HTTPS URL (0-256 bytes) to receive delivery reports for the message.</summary>
        [JsonPropertyName("callback_url")]
        public string? CallbackUrl { get; set; }

        /// <summary>Optional. Custom payload (0-128 bytes), not shown to the user.</summary>
        [JsonPropertyName("payload")]
        public string? Payload { get; set; }

        /// <summary>
        /// Optional. Time-to-live in seconds before the message expires (30 to 3600). If the message
        /// is not delivered or read within this time, the request fee is refunded automatically.
        /// </summary>
        [JsonPropertyName("ttl")]
        public int? Ttl { get; set; }
    }
}
