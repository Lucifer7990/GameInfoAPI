public interface ITokenService
{
    string  GenerateUserToken(string identity,string username,string email);
}