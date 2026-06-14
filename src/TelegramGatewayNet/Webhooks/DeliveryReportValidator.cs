using System;
using System.Security.Cryptography;
using System.Text;

namespace TelegramGatewayNet.Webhooks
{
    /// <summary>
    /// Validates the integrity of delivery reports posted by the Telegram Gateway API to a
    /// configured <c>callback_url</c>.
    /// <para>
    /// The signature is the hex-encoded <c>HMAC-SHA256</c> of the data-check-string, keyed by the
    /// <c>SHA256</c> hash of the API token. The data-check-string is
    /// <c>X-Request-Timestamp + "\n" + rawBody</c>.
    /// </para>
    /// </summary>
    public sealed class DeliveryReportValidator
    {
        private readonly byte[] _secretKey;

        /// <summary>Creates a validator for the given API token.</summary>
        /// <param name="apiToken">The Gateway API token used to sign reports.</param>
        public DeliveryReportValidator(string apiToken)
        {
            if (string.IsNullOrEmpty(apiToken))
            {
                throw new ArgumentException("API token must not be null or empty.", nameof(apiToken));
            }

            using (var sha256 = SHA256.Create())
            {
                _secretKey = sha256.ComputeHash(Encoding.UTF8.GetBytes(apiToken));
            }
        }

        /// <summary>
        /// Computes the expected signature for a report.
        /// </summary>
        /// <param name="timestamp">The value of the <c>X-Request-Timestamp</c> header.</param>
        /// <param name="rawBody">The raw POST body of the request, exactly as received.</param>
        /// <returns>The lowercase hexadecimal HMAC-SHA256 signature.</returns>
        public string ComputeSignature(string timestamp, string rawBody)
        {
            if (timestamp == null)
            {
                throw new ArgumentNullException(nameof(timestamp));
            }
            if (rawBody == null)
            {
                throw new ArgumentNullException(nameof(rawBody));
            }

            byte[] data = Encoding.UTF8.GetBytes(timestamp + "\n" + rawBody);
            using (var hmac = new HMACSHA256(_secretKey))
            {
                byte[] hash = hmac.ComputeHash(data);
                return ToHex(hash);
            }
        }

        /// <summary>
        /// Verifies the signature of a received report using a constant-time comparison.
        /// </summary>
        /// <param name="timestamp">The value of the <c>X-Request-Timestamp</c> header.</param>
        /// <param name="rawBody">The raw POST body of the request, exactly as received.</param>
        /// <param name="signature">The value of the <c>X-Request-Signature</c> header.</param>
        /// <returns><c>true</c> if the signature is valid; otherwise <c>false</c>.</returns>
        public bool IsValid(string timestamp, string rawBody, string signature)
        {
            if (string.IsNullOrEmpty(signature))
            {
                return false;
            }

            string expected = ComputeSignature(timestamp, rawBody);
            return FixedTimeEquals(expected, signature);
        }

        private static string ToHex(byte[] bytes)
        {
            var sb = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        private static bool FixedTimeEquals(string a, string b)
        {
            // Compare case-insensitively in constant time relative to the expected length.
            if (a.Length != b.Length)
            {
                return false;
            }

            int diff = 0;
            for (int i = 0; i < a.Length; i++)
            {
                diff |= char.ToLowerInvariant(a[i]) ^ char.ToLowerInvariant(b[i]);
            }
            return diff == 0;
        }
    }
}
