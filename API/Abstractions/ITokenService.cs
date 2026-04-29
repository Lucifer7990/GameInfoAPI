public interface ITokenService
{
    string GenerateJwt(string email, string role);
}