using System.Threading.Tasks;

namespace GameInfoAPI.API.Abstractions
{
    public interface IAuthService
    {
        Task<bool> SendOTP(OtpSendRequest req);
        Task VerifyOTP(OtpVerifyRequest req);
    }
}