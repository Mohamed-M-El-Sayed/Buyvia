namespace OnlineStore.Application.Contracts.Services.Verification
{
    public interface IPhoneVerificationService
    {
        Task SendOtpAsync(
        string phoneNumber,
        CancellationToken cancellationToken = default);
        Task<bool> VerifyOtpAsync(
        string phoneNumber,
        string otp,
        CancellationToken cancellationToken = default);
    }

}
