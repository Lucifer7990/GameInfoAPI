using System.Security.Cryptography;
using System.Text;

public class OtpService : IOtpService
{
    private readonly int _length = 4;

    public string Generate()
    {
        int max = (int)Math.Pow(10, _length);          
        int otp = RandomNumberGenerator.GetInt32(max); 
        return otp.ToString($"D{_length}");            
    }

    public string Hash(string otp)
    {
        byte[] sourceBytes = Encoding.UTF8.GetBytes(otp);
        byte[] hashBytes = SHA256.HashData(sourceBytes);
        return Convert.ToHexString(hashBytes);        
    }
}