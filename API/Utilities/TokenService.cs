using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class TokenService : ITokenService
{
    private readonly string Key = Environment.GetEnvironmentVariable("SECRATE_KEY") ?? throw new InvalidOperationException("SECRATE_KEY is Required");

    public string GenerateUserToken(string identity,string username,string email)
    {

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, identity),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, "User"),
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}