namespace TelegramGatewayNet.Models
{
    /// <summary>
    /// The outcome of a Telegram Gateway API call. An <c>ok: false</c> response (for example
    /// <c>PHONE_NUMBER_INVALID</c>) is a normal, expected result and is reported here via
    /// <see cref="Error"/> rather than thrown. Only failures that prevent obtaining a valid
    /// response — transport errors and malformed payloads — are thrown as
    /// <see cref="TelegramGatewayException"/>.
    /// </summary>
    /// <typeparam name="T">The type of the successful result payload.</typeparam>
    public sealed class GatewayResult<T>
    {
        private GatewayResult(bool ok, T? value, string? error)
        {
            Ok = ok;
            Value = value;
            Error = error;
        }

        /// <summary>Whether the API reported success (<c>ok: true</c>).</summary>
        public bool Ok { get; }

        /// <summary>The result payload. Non-null when <see cref="Ok"/> is <c>true</c>.</summary>
        public T? Value { get; }

        /// <summary>
        /// The API error code (for example <c>ACCESS_TOKEN_INVALID</c>). Non-null when
        /// <see cref="Ok"/> is <c>false</c>.
        /// </summary>
        public string? Error { get; }

        /// <summary>Creates a successful result.</summary>
        /// <param name="value">The result payload.</param>
        public static GatewayResult<T> Success(T value) => new GatewayResult<T>(true, value, null);

        /// <summary>Creates a failed result.</summary>
        /// <param name="error">The API error code.</param>
        public static GatewayResult<T> Failure(string error) => new GatewayResult<T>(false, default, error);
    }
}
