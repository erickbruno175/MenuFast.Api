using DocumentFormat.OpenXml.InkML;
using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Application.Services.Email;
using MenuFast.Api.Api.Application.Services.Redis;
using MenuFast.Api.Api.Application.Services.Security;
using MenuFast.Api.Api.Domain.Constantes;
using MenuFast.Api.Api.Domain.Entities.Models.Funcionario;
using MenuFast.Api.Api.Domain.Enum;
using MenuFast.Api.Api.Persistence.Context;
using MenuFast.Api.Api.Util.Helpers;
using MenuFast.Api.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace MenuFast.Api.Api.Application.Services.Seguranca {

    public record InformacaoAcesso {
        public string? Ip { get; set; }
        public string? Dispositivo { get; set; }
        public string? Navegador { get; set; }
        public string? SistemaOperacional { get; set; }
    }
    public class SegurancaService {

        private readonly MenuFastContext _menuFastContext;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly RedisService _redisService;
        public readonly ILogger<SegurancaService> _logger;
        public readonly EmailService _emailService;
        public SegurancaService(MenuFastContext menuFastContext, JwtService jwtService, RedisService redisService, ILogger<SegurancaService> logger, IHttpContextAccessor httpContextAccessor, EmailService emailService) {
            _menuFastContext = menuFastContext;
            _jwtService = jwtService;
            _redisService = redisService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

        public async Task<LoginResponse> AutenticarFuncionario(LoginRequest loginRequest) {
            try
            {
                var hoje = DateTime.Now;
                var funcionario = _menuFastContext.Funcionarios.Include(x => x.Perfil).FirstOrDefault(x =>x.SenhaHash == loginRequest.Senha &&new [ ] { 1, 2, 3 }.Contains(x.PerfilId.Value));

                if(funcionario == null) { throw new BusinessLogicException("Usaurio  invalido"); }
                if(!funcionario.Ativo) { throw new BusinessLogicException("Usaurio não esta ativo"); }
                if(SegurancaHelper.VerificaExpiracaoSenha(funcionario.DataExpiracaoSenha)) { throw new BusinessLogicException("Senha expirada, favor redefinir a senha."); }

                if(funcionario.Bloqueado.HasValue)
                {
                    if(funcionario.DataBloqueio.HasValue && funcionario.DataBloqueio.Value > hoje) { throw new BusinessLogicException($" Usuario Bloquedo temporariamente, estar desbloquado apos {funcionario.DataBloqueio}"); }
                }
                if(!SegurancaHelper.ValidarSenha(loginRequest.Senha, funcionario.SenhaHash))
                {
                    funcionario.TentativasLogin++;
                    if(funcionario.TentativasLogin >= _menuFastContext.ConfiguracoesSeguranca.FirstOrDefault()?.MaxTentativasLogin)
                    {
                        funcionario.Bloqueado = true;
                        funcionario.DataBloqueio = DateTime.Now.AddMinutes(_menuFastContext.ConfiguracoesSeguranca.FirstOrDefault()?.TempoBloqueioMinutos ?? 30);
                        _menuFastContext.SaveChangesAsync();

                        throw new BusinessLogicException($"Usuário bloqueado por {_menuFastContext.ConfiguracoesSeguranca.FirstOrDefault()?.TempoBloqueioMinutos ?? 30} minutos.");
                    }
                    await _menuFastContext.SaveChangesAsync();

                    throw new BusinessLogicException(
                        $"Senha inválida. Tentativa {funcionario.TentativasLogin} de {_menuFastContext.ConfiguracoesSeguranca.FirstOrDefault()?.MaxTentativasLogin ?? 5}.");

                }

                _logger.LogWarning($"Tentativa de login inválida para o usuário {funcionario.Login}." +
                 $" Tentativa {funcionario.TentativasLogin} de " +
                 $"{_menuFastContext.ConfiguracoesSeguranca.FirstOrDefault()?.MaxTentativasLogin ?? 5}." +
                 $"Data: {hoje}");

                funcionario.Bloqueado = false;
                funcionario.TentativasLogin = 0;
                funcionario.DataUltimoLogin = hoje;
                funcionario.DataBloqueio = null;

                var token = _jwtService.GerarToken(funcionario.Id, funcionario.Login, funcionario.Perfil.Descricao, funcionario.Nome);
                if(token == null) { throw new BusinessLogicException("Token não pode ser gerado"); }

                var dadosAcesso = new InformacaoAcesso
                {
                    Ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString(),
                    Dispositivo = _httpContextAccessor?.HttpContext?.Request.Headers [ "User-Agent" ]
                };
                _menuFastContext.HistoricoAcessos.AddAsync(new Domain.Entities.Models.Seguranca.HistoricoAcesso
                {
                    DataLogin = hoje,
                    DataLogout = null,
                    FuncionarioId = funcionario.Id,
                    SessaoAtiva = true,
                    Token = token,
                    Dispositivo = dadosAcesso.Dispositivo,
                    Ip = dadosAcesso.Ip,
                    TipoAcesso = TipoAcesso.Login


                });

                await _redisService.SetAsync($"id usuaurio logado - {funcionario.Id}",
                    new
                    {
                        funcionario.Id,
                        funcionario.Nome,
                        funcionario.PerfilId,
                        funcionario.Email,

                    }, TimeSpan.FromHours(8)

                     );


                _menuFastContext.SaveChangesAsync();
                return new LoginResponse
                {
                    Token = token,
                    Nome = funcionario.Nome,
                    PerfilId = funcionario.PerfilId.Value,
                };
            }
            catch(BusinessLogicException ex)
            {
                _logger.LogError($"Erro de negócio ao autenticar funcionário: {ex.Message}"
                    + $"Data: {DateTime.UtcNow}", $"Tipo de log:{TipoLog.ErroLogin}");
                throw new BusinessLogicException("Ocorreu um erro de negócio ao autenticar o funcionário.");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Erro inesperado ao autenticar funcionário: {ex.Message}"
                    + $"Data: {DateTime.UtcNow}", $"Tipo de log:{TipoLog.ErroLogin}");
                throw new BusinessLogicException("Ocorreu um erro inesperado ao autenticar o funcionário.");

            }
        }
        public async Task Desloga() {
            var httpContext = _httpContextAccessor.HttpContext;

            var token = httpContext?.Request.Headers [ "Authorization" ].ToString().Replace("Bearer ", "");

            if(string.IsNullOrWhiteSpace(token)) return;

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

            var tempoRestante = jwt.ValidTo - DateTime.UtcNow;

            if(tempoRestante > TimeSpan.Zero)
            {
                await _redisService.SetAsync($"blacklist:{token}", "logout", tempoRestante);
            }

            var usuarioIdClaim = httpContext?.User?.FindFirst("id")?.Value;

            if(!int.TryParse(usuarioIdClaim, out var funcionarioId))
            {
                throw new BusinessLogicException(
                    "Não foi possível identificar o usuário.!");
            }

            var historico = await _menuFastContext.HistoricoAcessos
                .FirstOrDefaultAsync(x =>
                    x.Token == token &&
                    x.SessaoAtiva);

            if(historico == null)
                return;

            historico.FuncionarioId = funcionarioId;
            historico.DataLogout = DateTime.Now;
            historico.SessaoAtiva = false;
            historico.TipoAcesso = TipoAcesso.Logout;

            historico.Ip = httpContext
                .Connection
                .RemoteIpAddress?
                .ToString();

            historico.Dispositivo = httpContext
                .Request
                .Headers [ "User-Agent" ]
                .ToString();

            await _menuFastContext.SaveChangesAsync();
        }

        public async Task RedefinirSenhas(string email) {
            try
            {
                var funcionario = await _menuFastContext.Funcionarios
                    .FirstOrDefaultAsync(f => f.Email == email);

                if(funcionario == null)
                    throw new BusinessLogicException(
                        "Não encontramos nenhum usuário cadastrado com este e-mail."
                    );

                var template = await _menuFastContext.TemplatesEmail
                    .FirstOrDefaultAsync(e => e.Nome == "RECUPERAÇÃO DE SENHA");

                if(template == null)
                    throw new BusinessLogicException(
                        "Não foi possível localizar o modelo de e-mail para recuperação de senha."
                    );

                var conteudo = template.Conteudo
                    .Replace("{NOME_USUARIO}", funcionario.Nome)
                    .Replace("{LINK_REDEFINICAO}", LinkEmail.LinkRecuperarSenha);

                await _emailService.EnviarAsync(
                    email,
                    template.Assunto,
                    conteudo
                );
            }
            catch(BusinessLogicException)
            {
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError(
                    $"Erro inesperado ao redefinir senha: {ex.Message} " +
                    $"Data: {DateTime.UtcNow}",
                    $"Tipo de log: {TipoLog.ErroEnvioEmail.GetDisplayName()}"
                );

                throw new BusinessLogicException(
                    "Não foi possível enviar o e-mail de recuperação de senha. Tente novamente."
                );
            }
        }
    }


}

