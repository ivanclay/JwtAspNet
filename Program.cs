using JwtAspNet.Extensions;
using JwtAspNet.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<TokenService>();

builder.Services.AddAuthentication(x => 
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.PrivateKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

builder.Services.AddAuthorization( x =>
{
    x.AddPolicy("Admin", p => p.RequireRole("admin"));
});


var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();


app.MapGet("/login", (TokenService service) => {

    var user = new User
    (
        1,
        "Test",
        "test@test.com",
        "https://minhaimagem.com",
        "password123",
        new[] { "student", "premium" }
    );
    return service.Create(user);
});

app.MapGet("/restrito", (ClaimsPrincipal user) => new
{
    id = user.Id(),
    image = user.Image(),
    name = user.Name(),
    email = user.Email(),
    givenName = user.GivenName(),
})
.RequireAuthorization();

app.MapGet("/admin", () => "Você tem acesso").RequireAuthorization("admin");


app.Run();
