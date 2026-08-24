using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using OnlineStore.Application.Contracts.Services.Verification;

namespace OnlineStore.Infrastructure.Services.Verification
{
    public class AuthevoPhoneVerificationService(
        HttpClient httpClient,
        IOptions<AuthevoOptions> options)
        : IPhoneVerificationService
    {
        private const string SendOtpEndpoint = "/v1/otp/send";
        private const string VerifyOtpEndpoint = "/v1/otp/verify";

        private readonly string _secretKey = options.Value.SecretKey;

        public async Task SendOtpAsync(string phoneNumber, CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                SendOtpEndpoint);

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _secretKey);

            request.Content = JsonContent.Create(new
            {
                phone = phoneNumber
            });

            var response = await httpClient.SendAsync(
                request,
                cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Authevo returned {(int)response.StatusCode} " +
                    $"({response.StatusCode}). Response: {responseBody}");
            }
        }

        public async Task<bool> VerifyOtpAsync(
            string phoneNumber,
            string otp,
            CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                VerifyOtpEndpoint);

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _secretKey);

            request.Content = JsonContent.Create(new
            {
                phone = phoneNumber,
                code = otp
            });

            var response = await httpClient.SendAsync(
                request,
                cancellationToken);

            return response.IsSuccessStatusCode;
        }
    }
}