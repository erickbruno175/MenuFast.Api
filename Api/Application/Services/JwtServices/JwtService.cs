using DocumentFormat.OpenXml.Office2016.Drawing.Command;
using MenuFast.Api.Api.Domain.Entities.Models.Loja;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MenuFast.Api.Api.Application.Services.Security;

public class JwtService {
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration) {
        _configuration = configuration;
    }


    public string GerarToken(int funcionarioId,string email,string perfil,string nome,string lojaId) {
        var chave = _configuration [ "Jwt:Key" ];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave!));
        var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256
        );var claims = new [ ]{
        new Claim("id", funcionarioId.ToString()),
        new Claim("lojaId", lojaId),
        new Claim(JwtRegisteredClaimNames.UniqueName, email),
        new Claim(ClaimTypes.Role, perfil),
        new Claim("nome", nome)};

        var token = new JwtSecurityToken(
            issuer: _configuration [ "Jwt:Issuer" ],
            audience: _configuration [ "Jwt:Audience" ],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}