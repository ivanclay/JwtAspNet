
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JwtAspNet.Services;
public class TokenService
{
	public string Create()
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
			Expires = DateTime.UtcNow.AddHours(2)
		};

		var token = handler.CreateToken(tokenDescription);
		return handler.WriteToken(token);
	}
}
