public interface ITokenService
{
    string GenerateUserToken(string identity, string username, string email, int expiresMin);
    string GenerateAdminToken(string identity, string username, string email, int expiresMin);

}