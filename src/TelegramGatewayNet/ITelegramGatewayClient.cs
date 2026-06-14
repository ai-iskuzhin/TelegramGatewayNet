using System.Threading;
using System.Threading.Tasks;
using TelegramGatewayNet.Models;
using TelegramGatewayNet.Requests;

namespace TelegramGatewayNet
{
    /// <summary>
    /// A client for the Telegram Gateway API (<c>https://gatewayapi.telegram.org</c>).
    /// <para>
    /// Methods return a <see cref="GatewayResult{T}"/>: an <c>ok: false</c> API response is a normal
    /// outcome reported via <see cref="GatewayResult{T}.Error"/>, not an exception. Only transport
    /// failures and malformed responses throw <see cref="TelegramGatewayException"/>.
    /// </para>
    /// </summary>
    public interface ITelegramGatewayClient
    {
        /// <summary>
        /// Sends a verification message. Charges apply per successful delivery, except when sending
        /// to your own phone number.
        /// </summary>
        /// <param name="request">The send parameters.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        Task<GatewayResult<RequestStatus>> SendVerificationMessageAsync(
            SendVerificationMessageRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks the ability to send a verification message to a phone number. If sending is
        /// possible a fee applies; pass the returned <c>request_id</c> to
        /// <see cref="SendVerificationMessageAsync"/> to avoid being charged twice.
        /// </summary>
        /// <param name="request">The check parameters.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        Task<GatewayResult<RequestStatus>> CheckSendAbilityAsync(
            CheckSendAbilityRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks the status of a previously sent verification message, optionally validating the
        /// code entered by the user.
        /// </summary>
        /// <param name="request">The check parameters.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        Task<GatewayResult<RequestStatus>> CheckVerificationStatusAsync(
            CheckVerificationStatusRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Revokes a previously sent verification message. A successful result carries <c>true</c>
        /// if the revocation request was received; this does not guarantee the message will be
        /// deleted (a message that has already been delivered or read will not be removed).
        /// </summary>
        /// <param name="request">The revoke parameters.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        Task<GatewayResult<bool>> RevokeVerificationMessageAsync(
            RevokeVerificationMessageRequest request,
            CancellationToken cancellationToken = default);
    }
}
