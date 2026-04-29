public interface IMessageSender
{
    Task SendOtpAsync(string to, string otp, int expiryMinutes);
}