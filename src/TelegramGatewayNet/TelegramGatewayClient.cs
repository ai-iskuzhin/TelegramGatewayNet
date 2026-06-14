using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TelegramGatewayNet.Models;
using TelegramGatewayNet.Requests;

namespace TelegramGatewayNet
{
    /// <summary>
    /// Default <see cref="ITelegramGatewayClient"/> implementation backed by <see cref="HttpClient"/>.
    /// Requests are sent as <c>application/json</c> POST calls with a bearer token.
    /// </summary>
    public sealed class TelegramGatewayClient : ITelegramGatewayClient, IDisposable
    {
        /// <summary>The default base URL of the Telegram Gateway API.</summary>
        public const string DefaultBaseUrl = "https://gatewayapi.telegram.org/";

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = null
        };

        private readonly HttpClient _httpClient;
        private readonly bool _ownsHttpClient;

        /// <summary>
        /// Creates a client that owns its own <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="accessToken">The Gateway API access token from your account settings.</param>
        /// <param name="baseUrl">Optional override of the API base URL.</param>
        public TelegramGatewayClient(string accessToken, string baseUrl = DefaultBaseUrl)
            : this(accessToken, new HttpClient(), baseUrl, ownsHttpClient: true)
        {
        }

        /// <summary>
        /// Creates a client using a caller-provided <see cref="HttpClient"/> (recommended for
        /// dependency injection and <c>IHttpClientFactory</c> scenarios). The client is not disposed.
        /// </summary>
        /// <param name="accessToken">The Gateway API access token from your account settings.</param>
        /// <param name="httpClient">The HTTP client to use for requests.</param>
        /// <param name="baseUrl">Optional override of the API base URL.</param>
        public TelegramGatewayClient(string accessToken, HttpClient httpClient, string baseUrl = DefaultBaseUrl)
            : this(accessToken, httpClient, baseUrl, ownsHttpClient: false)
        {
        }

        private TelegramGatewayClient(string accessToken, HttpClient httpClient, string baseUrl, bool ownsHttpClient)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentException("Access token must not be null or empty.", nameof(accessToken));
            }
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentException("Base URL must not be null or empty.", nameof(baseUrl));
            }

            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _ownsHttpClient = ownsHttpClient;

            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(baseUrl.EndsWith("/", StringComparison.Ordinal) ? baseUrl : baseUrl + "/");
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        /// <inheritdoc />
        public Task<RequestStatus> SendVerificationMessageAsync(
            SendVerificationMessageRequest request,
            CancellationToken cancellationToken = default)
        {
            return InvokeAsync<RequestStatus>("sendVerificationMessage", request, cancellationToken);
        }

        /// <inheritdoc />
        public Task<RequestStatus> CheckSendAbilityAsync(
            CheckSendAbilityRequest request,
            CancellationToken cancellationToken = default)
        {
            return InvokeAsync<RequestStatus>("checkSendAbility", request, cancellationToken);
        }

        /// <inheritdoc />
        public Task<RequestStatus> CheckVerificationStatusAsync(
            CheckVerificationStatusRequest request,
            CancellationToken cancellationToken = default)
        {
            return InvokeAsync<RequestStatus>("checkVerificationStatus", request, cancellationToken);
        }

        /// <inheritdoc />
        public Task<bool> RevokeVerificationMessageAsync(
            RevokeVerificationMessageRequest request,
            CancellationToken cancellationToken = default)
        {
            return InvokeAsync<bool>("revokeVerificationMessage", request, cancellationToken);
        }

        private async Task<TResult> InvokeAsync<TResult>(string method, object request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            string requestJson = JsonSerializer.Serialize(request, request.GetType(), JsonOptions);
            using (var content = new StringContent(requestJson, Encoding.UTF8, "application/json"))
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, method) { Content = content })
            {
                HttpResponseMessage httpResponse;
                try
                {
                    httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
                }
                catch (HttpRequestException ex)
                {
                    throw new TelegramGatewayException($"HTTP request to '{method}' failed: {ex.Message}", ex);
                }

                using (httpResponse)
                {
#if NET5_0_OR_GREATER
                    string body = await httpResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
#else
                    string body = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
#endif
                    GatewayResponse<TResult>? response;
                    try
                    {
                        response = JsonSerializer.Deserialize<GatewayResponse<TResult>>(body, JsonOptions);
                    }
                    catch (JsonException ex)
                    {
                        throw new TelegramGatewayException(
                            $"Failed to parse Telegram Gateway response (HTTP {(int)httpResponse.StatusCode}): {ex.Message}", ex);
                    }

                    if (response == null)
                    {
                        throw new TelegramGatewayException(
                            $"Telegram Gateway returned an empty response (HTTP {(int)httpResponse.StatusCode}).", null);
                    }

                    if (!response.Ok)
                    {
                        throw new TelegramGatewayException(response.Error ?? "UNKNOWN_ERROR");
                    }

                    return response.Result!;
                }
            }
        }

        /// <summary>Disposes the underlying <see cref="HttpClient"/> if it is owned by this client.</summary>
        public void Dispose()
        {
            if (_ownsHttpClient)
            {
                _httpClient.Dispose();
            }
        }
    }
}
