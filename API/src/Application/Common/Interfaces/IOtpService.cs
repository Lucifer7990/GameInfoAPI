public interface IOtpService
{
    string Generate();
    string Hash(string otp);
}