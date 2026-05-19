namespace Application.Services
{
    public interface IGameDetails
    {
        Task<bool> SendOTP(string Email);
        Task<string?> VerifyOTP(string Email,string OTP);
    }
}