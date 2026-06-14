using System.Text.Json.Serialization;

namespace TelegramGatewayNet.Requests
{
    /// <summary>
    /// Parameters for the <c>checkSendAbility</c> method.
    /// </summary>
    public sealed class CheckSendAbilityRequest
    {
        /// <summary>Creates a new request for the given phone number.</summary>
        /// <param name="phoneNumber">The phone number to check, in E.164 format.</param>
        public CheckSendAbilityRequest(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        /// <summary>The phone number for which to check the ability to send, in E.164 format.</summary>
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }
    }
}
