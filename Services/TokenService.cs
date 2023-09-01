
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAspNet.Services;
public class TokenService
{
	public string Create(User user)
	{
		var handler = new JwtSecurityTokenHandler();

		var key = Encoding.ASCII.GetBytes(Configuration.PrivateKey);

		//assinar token
		var credentials = new SigningCredentials(
			key:new SymmetricSecurityKey(key),
			algorithm:SecurityAlgorithms.HmacSha256);

		var tokenDescription = new SecurityTokenDescriptor 
		{
			SigningCredentials = credentials,
			Expires = DateTime.UtcNow.AddHours(2),
			Subject = GenerateClaims(user),
		};

		var token = handler.CreateToken(tokenDescription);
		return handler.WriteToken(token);
	}

	private static ClaimsIdentity GenerateClaims(User user)
	{
		var ci = new ClaimsIdentity();

		ci.AddClaim(new Claim("id", user.Id.ToString()));
        ci.AddClaim(new Claim(ClaimTypes.Name, user.Email));
        ci.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        ci.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
        ci.AddClaim(new Claim("image", user.Image.ToString()));

		foreach (var role in user.Roles)
		{
            ci.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        return ci;
	}
}
