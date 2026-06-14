using System.Threading;
using System.Threading.Tasks;
using TelegramGatewayNet.Models;
using TelegramGatewayNet.Requests;

namespace TelegramGatewayNet
{
    /// <summary>
    /// A client for the Telegram Gateway API (<c>https://gatewayapi.telegram.org</c>).
    /// </summary>
    public interface ITelegramGatewayClient
    {
        /// <summary>
        /// Sends a verification message. Charges apply per successful delivery, except when sending
        /// to your own phone number. On success, returns a <see cref="RequestStatus"/>.
        /// </summary>
        /// <param name="request">The send parameters.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        Task<RequestStatus> SendVerificationMessageAsync(
            SendVerificationMessageRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks the ability to send a verification message to a phone number. If sending is
        /// possible a fee applies; pass the returned <c>request_id</c> to
        /// <see cref="SendVerificationMessageAsync"/> to avoid being charged twice.
        /// </summary>
        /// <param name="request">The check parameters.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        Task<RequestStatus> CheckSendAbilityAsync(
            CheckSendAbilityRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks the status of a previously sent verification message, optionally validating the
        /// code entered by the user. On success, returns a <see cref="RequestStatus"/>.
        /// </summary>
        /// <param name="request">The check parameters.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        Task<RequestStatus> CheckVerificationStatusAsync(
            CheckVerificationStatusRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Revokes a previously sent verification message. Returns <c>true</c> if the revocation
        /// request was received; this does not guarantee the message will be deleted (a message
        /// that has already been delivered or read will not be removed).
        /// </summary>
        /// <param name="request">The revoke parameters.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        Task<bool> RevokeVerificationMessageAsync(
            RevokeVerificationMessageRequest request,
            CancellationToken cancellationToken = default);
    }
}
