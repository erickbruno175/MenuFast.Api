using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DocumentFormat.OpenXml.Office2016.Drawing.Command;
using Microsoft.IdentityModel.Tokens;

namespace MenuFast.Api.Api.Application.Services.Security;

public class JwtService {
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration) {
        _configuration = configuration;
    }


    public string GerarToken(Guid funcionarioId, string login, string perfil, string funcao, string nome) {
        var chave = _configuration [ "Jwt:Key" ];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new [ ]
        {
            new Claim(JwtRegisteredClaimNames.Sub,funcionarioId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName,login),
            new Claim(ClaimTypes.Role,perfil),
            new Claim(nome , "nome"),
            new Claim(funcao , "funcao"),

        };
        var token = new JwtSecurityToken(
            issuer: _configuration [ "Jwt:Issuer" ],
            audience: _configuration [ "Jwt:Audience" ],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: credentials
        );


        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}