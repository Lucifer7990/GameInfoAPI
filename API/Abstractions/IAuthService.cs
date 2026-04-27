namespace GameInfoAPI.API.Abstractions
{
    public interface IAuthService
    {
        Task<bool> SendOTP(string Email);
        Task<string?> VerifyOTP(string Email,string OTP);
    }
}