namespace Application.Services
{
    public interface IAuthService
    {
        Task<bool> SendOTP(string Email);
        Task<string?> VerifyOTP(string Email,string OTP);
        Task<string?> VerifyOTPTemp();

        string AdminAuth();
        
    }
}