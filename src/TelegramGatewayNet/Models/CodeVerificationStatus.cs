namespace TelegramGatewayNet.Models
{
    /// <summary>
    /// The current status of the verification process for a code.
    /// </summary>
    public enum CodeVerificationStatus
    {
        /// <summary>The status reported by Telegram is not recognized by this SDK version.</summary>
        Unknown = 0,

        /// <summary>The code entered by the user is correct.</summary>
        CodeValid,

        /// <summary>The code entered by the user is incorrect.</summary>
        CodeInvalid,

        /// <summary>The maximum number of attempts to enter the code has been exceeded.</summary>
        CodeMaxAttemptsExceeded,

        /// <summary>The code has expired and can no longer be used for verification.</summary>
        Expired
    }
}
