namespace TelegramGatewayNet.Models
{
    /// <summary>
    /// The current status of a verification message.
    /// </summary>
    public enum MessageDeliveryStatus
    {
        /// <summary>The status reported by Telegram is not recognized by this SDK version.</summary>
        Unknown = 0,

        /// <summary>The message has been sent to the recipient's device(s).</summary>
        Sent,

        /// <summary>The message has been delivered to the recipient's device(s).</summary>
        Delivered,

        /// <summary>The message has been read by the recipient.</summary>
        Read,

        /// <summary>The message has expired without being delivered or read.</summary>
        Expired,

        /// <summary>The message has been revoked.</summary>
        Revoked
    }
}
