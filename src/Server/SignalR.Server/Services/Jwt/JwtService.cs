using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SignalR.Server.Settings;

namespace SignalR.Server.Services.Jwt;

public class JwtService : IJwtService
{
    
    private readonly IOptions<JwtSettings> _options;

    public JwtService(IOptions<JwtSettings> options)
    {
        _options = options;
    }
    
    public Task<string> GenerateTokenAsync()
    {
        var securityToken =new JwtSecurityTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _options.Value.Issuer,
            Audience = _options.Value.Audience,
            Expires = DateTime.UtcNow.AddHours(1), // expires after 1 hour
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key)), SecurityAlgorithms.HmacSha256Signature)
        });

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(securityToken));
    }
}