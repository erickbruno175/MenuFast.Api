using DocumentFormat.OpenXml.InkML;
using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Application.Services.Email;
using MenuFast.Api.Api.Application.Services.Redis;
using MenuFast.Api.Api.Application.Services.Security;
using MenuFast.Api.Api.Domain.Constantes;
using MenuFast.Api.Api.Domain.Entities.Models.Funcionario;
using MenuFast.Api.Api.Domain.Entities.Models.Seguranca;
using MenuFast.Api.Api.Domain.Enum;
using MenuFast.Api.Api.Persistence.Context;
using MenuFast.Api.Api.Util.Helpers;
using MenuFast.Api.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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
        private readonly IConfiguration _configuration;

        public SegurancaService(MenuFastContext menuFastContext, JwtService jwtService, RedisService redisService, ILogger<SegurancaService> logger, IHttpContextAccessor httpContextAccessor, EmailService emailService, IConfiguration configuration) {
            _menuFastContext = menuFastContext;
            _jwtService = jwtService;
            _redisService = redisService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<LoginResponse> AutenticarFuncionario(LoginRequest loginRequest) {
            try
            {
                var hoje = DateTime.Now;
                var configuracao = await _menuFastContext.ConfiguracoesSeguranca.FirstOrDefaultAsync();

                var maxTentativas = configuracao?.MaxTentativasLogin ?? 5;
                var tempoBloqueio = configuracao?.TempoBloqueioMinutos ?? 30;

                var funcionario = await _menuFastContext.Funcionarios
                    .Include(x => x.Loja)
                    .Include(x => x.Perfil).
                    FirstOrDefaultAsync(x => x.Email == loginRequest.Email && x.PerfilId.HasValue &&
                        new [ ] { 1, 2, 3 }.Contains(x.PerfilId.Value));

                if(funcionario == null) { throw new BusinessLogicException("Usuário inválido."); }

                if(!funcionario.Ativo) { throw new BusinessLogicException("Usuário não está ativo."); }

                if(SegurancaHelper.VerificaExpiracaoSenha(funcionario.DataExpiracaoSenha))
                { throw new BusinessLogicException("Senha expirada, favor redefinir a senha."); }


                if(funcionario.Bloqueado == true)
                {
                    if(funcionario.DataBloqueio.HasValue && funcionario.DataBloqueio.Value > hoje)
                    {
                        var minutosRestantes = Math.Ceiling((funcionario.DataBloqueio.Value - hoje).TotalMinutes);

                        throw new BusinessLogicException(
                            $"Usuário bloqueado temporariamente. " +
                            $"Tente novamente em aproximadamente " +
                            $"{minutosRestantes} minuto(s)."
                        );
                    }

                    funcionario.Bloqueado = false;
                    funcionario.DataBloqueio = null;
                    funcionario.TentativasLogin = 0;
                    await _menuFastContext.SaveChangesAsync();
                }

                if(!SegurancaHelper.ValidarSenha(loginRequest.Senha, funcionario.SenhaHash))
                {
                    funcionario.TentativasLogin = (funcionario.TentativasLogin ?? 0) + 1;

                    _logger.LogWarning(
                        "Senha inválida para o usuário {Email}. " +
                        "Tentativa {Tentativa} de {MaxTentativas}. Data: {Data}",
                        funcionario.Email,
                        funcionario.TentativasLogin,
                        maxTentativas,
                        hoje
                    );


                    if(funcionario.TentativasLogin >= maxTentativas)
                    {
                        funcionario.Bloqueado = true;
                        funcionario.DataBloqueio = hoje.AddMinutes(tempoBloqueio);

                        await _menuFastContext.SaveChangesAsync();

                        throw new BusinessLogicException($"Usuário bloqueado por {tempoBloqueio} minutos.");
                    }

                    await _menuFastContext.SaveChangesAsync();
                    throw new BusinessLogicException($"Senha inválida. Tentativa {funcionario.TentativasLogin} de {maxTentativas}.");
                }

                var estarFechado = await EstabelecimentoEstaFechado(funcionario.LojaId.Value);

                if(estarFechado && funcionario.PerfilId != (int)PerfilUsuario.Administrador)
                { throw new BusinessLogicException("Opa, hoje estamos fechados. Abriremos amanha"); }

                funcionario.Bloqueado = false;
                funcionario.TentativasLogin = 0;
                funcionario.DataUltimoLogin = hoje;
                funcionario.DataBloqueio = null;

                var token = _jwtService.GerarToken(
                    funcionario.Id,
                    funcionario.Email,
                    funcionario.Perfil.Descricao,
                    funcionario.Nome,
                    funcionario.LojaId.ToString()
                );

                if(token == null)
                {
                    throw new BusinessLogicException("Token não pode ser gerado.");
                }

                _logger.LogInformation(
                    "Login realizado com sucesso para o usuário {Email}. Data: {Data}",
                    funcionario.Email,
                    hoje
                );


                var dadosAcesso = new InformacaoAcesso
                {
                    Ip = _httpContextAccessor.HttpContext?
                        .Connection
                        .RemoteIpAddress?
                        .ToString(),

                    Dispositivo = _httpContextAccessor.HttpContext?
                        .Request
                        .Headers [ "User-Agent" ]
                        .ToString()
                };

                var historico = new Domain.Entities.Models.Seguranca.HistoricoAcesso
                {
                    DataLogin = hoje,
                    DataLogout = null,
                    FuncionarioId = funcionario.Id,
                    SessaoAtiva = true,
                    Token = token,
                    Dispositivo = dadosAcesso.Dispositivo,
                    Ip = dadosAcesso.Ip,
                    TipoAcesso = TipoAcesso.Login,
                    LojaId = funcionario.LojaId.Value
                };

                _menuFastContext.HistoricoAcessos.Add(historico);

                await _menuFastContext.SaveChangesAsync();


                await _redisService.SetAsync(
                    $"usuario-logado:{funcionario.Id}",
                    new
                    {
                        funcionario.Id,
                        funcionario.Nome,
                        funcionario.PerfilId,
                        funcionario.Email,
                        token = token,
                        lojaId = funcionario.LojaId.Value,
                    },
                    TimeSpan.FromHours(8)
                );

                return new LoginResponse { Token = token, Nome = funcionario.Nome, PerfilId = funcionario.PerfilId.Value };
            }
            catch(BusinessLogicException ex)
            {
                _logger.LogWarning(
                    "Erro de negócio ao autenticar funcionário: {Mensagem}. Data: {Data}",
                    ex.Message,
                    DateTime.UtcNow
                );
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro inesperado ao autenticar funcionário. Data: {Data}",
                    DateTime.UtcNow);

                throw new BusinessLogicException("Ocorreu um erro inesperado ao autenticar o funcionário.");
            }
        }
        public async Task Desloga() {
            var httpContext = _httpContextAccessor.HttpContext;

            if(httpContext == null) throw new BusinessLogicException("Contexto HTTP não encontrado.");

            var authorization = httpContext.Request.Headers.Authorization.ToString();

            if(string.IsNullOrWhiteSpace(authorization)) throw new BusinessLogicException("Authorization não informado.");

            if(!authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)) throw new BusinessLogicException("Authorization inválido.");

            var token = authorization [ "Bearer ".Length.. ].Trim();

            if(string.IsNullOrWhiteSpace(token)) throw new BusinessLogicException("Token não informado.");

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

            var tempoRestante = jwt.ValidTo - DateTime.UtcNow;

            // Coloca o token na blacklist até o vencimento
            if(tempoRestante > TimeSpan.Zero)
            {
                await _redisService.SetAsync($"blacklist:{token}", "logout", tempoRestante);
            }

            var historico = await _menuFastContext.HistoricoAcessos.FirstOrDefaultAsync(x => x.Token == token && x.SessaoAtiva);

            if(historico == null)
                return;

            await _redisService.RemoveAsync($"usuario-logado:{historico.FuncionarioId}");

            historico.DataLogout = DateTime.Now;
            historico.SessaoAtiva = false;
            historico.TipoAcesso = TipoAcesso.Logout;
            historico.Ip = httpContext.Connection.RemoteIpAddress?.ToString();
            historico.Dispositivo = httpContext.Request.Headers [ "User-Agent" ].ToString();

            await _menuFastContext.SaveChangesAsync();
        }

        public async Task EsqueciSenha(string email) {
            try
            {
                var funcionario = await _menuFastContext.Funcionarios.FirstOrDefaultAsync(f => f.Email == email);

                if(funcionario == null) throw new BusinessLogicException("Não encontramos nenhum usuário cadastrado com este e-mail.");

                var template = await _menuFastContext.TemplatesEmail.FirstOrDefaultAsync(e => e.Nome == "RECUPERAÇÃO DE SENHA");

                if(template == null) throw new BusinessLogicException("Não foi possível localizar o modelo de e-mail para recuperação de senha.");

                var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

                var tokenRedefinicao = new TokenRedefinicaoSenha
                {
                    FuncionarioId = funcionario.Id,
                    Token = token,
                    DataCriacao = DateTime.UtcNow,
                    DataExpiracao = DateTime.UtcNow.AddHours(1),
                    Usado = false
                };

                _menuFastContext.TokenRedefinicaoSenhas.Add(tokenRedefinicao);

                await _menuFastContext.SaveChangesAsync();

                var link = $"{LinkEmail.LinkRecuperarSenha}?token={Uri.EscapeDataString(token)}";


                var conteudo = template.Conteudo
                    .Replace("{NOME_USUARIO}", funcionario.Nome)
                    .Replace("{LINK_REDEFINICAO}", link)
                    .Replace("{LOGO_MENUFAST}", funcionario.Loja?.Logo);

                await _emailService.EnviarAsync(email, template.Assunto, conteudo);
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

                throw new BusinessLogicException("Não foi possível enviar o e-mail de recuperação de senha. Tente novamente.");
            }
        }

        private async Task<bool> EstabelecimentoEstaFechado(int lojaId) {
            var agora = DateTime.Now;
            var horario = await _menuFastContext.HorariosFuncionamento.FirstOrDefaultAsync(X => X.LojaId == lojaId && X.DiaSemana == agora.DayOfWeek);
            if(horario == null) return true;
            if(horario.Fechado) return true;

            var horaAtual = agora.TimeOfDay;

            return horaAtual < horario.HoraAbertura || horaAtual > horario.HoraFechamento;
        }
        public async Task AlterarSenhaAsync(int funcionarioId, string novaSenha, string confirmarSenha) {
            if(string.IsNullOrWhiteSpace(novaSenha)) throw new BusinessLogicException("Informe a nova senha.");

            if(novaSenha != confirmarSenha) throw new BusinessLogicException("As senhas não coincidem.");

            if(novaSenha.Length < 8) throw new BusinessLogicException("A senha deve ter no mínimo 8 caracteres.");

            var funcionario = await _menuFastContext.Funcionarios.FirstOrDefaultAsync(x => x.Id == funcionarioId);

            if(funcionario == null) throw new BusinessLogicException("Funcionário não encontrado.");
            funcionario.SenhaHash = SegurancaHelper.CriarHash(novaSenha);
            await _menuFastContext.SaveChangesAsync();
        }

        public async Task RedefinirSenhaAsync(string token,string novaSenha,string confirmarSenha) {
            var tokenRedefinicao = await _menuFastContext.TokenRedefinicaoSenhas.FirstOrDefaultAsync(x => x.Token == token);

            if(tokenRedefinicao == null)throw new BusinessLogicException("Link de redefinição inválido.");

            if(tokenRedefinicao.Usado) throw new BusinessLogicException("Este link de redefinição já foi utilizado.");

            if(tokenRedefinicao.DataExpiracao <= DateTime.UtcNow)throw new BusinessLogicException("Este link de redefinição expirou.");

            if(novaSenha != confirmarSenha)throw new BusinessLogicException("As senhas não coincidem.");

            if(novaSenha.Length < 8)throw new BusinessLogicException("A senha deve ter no mínimo 8 caracteres.");

            var funcionario = await _menuFastContext.Funcionarios.FirstOrDefaultAsync(x => x.Id == tokenRedefinicao.FuncionarioId);

            if(funcionario == null)throw new BusinessLogicException("Funcionário não encontrado.");

            funcionario.SenhaHash = SegurancaHelper.CriarHash(novaSenha);
            tokenRedefinicao.Usado = true;

            await _menuFastContext.SaveChangesAsync();
        }


        public async Task<ICollection<Perfil>> ListarPerfis() {
            return await _menuFastContext.Perfis
                .OrderBy(p=> p.Nome).ToListAsync();
        }

        public async Task<ICollection<Permissao>> ListarPermissoes() {
            return await _menuFastContext.Permissoes
                .OrderBy(p => p.Descricao).ToListAsync();
        }

        public async Task<IEnumerable<int>> ObterPermissoesDoPerfil(int perfilId) {
            return await _menuFastContext.PerfilPermissoes
                .Where(x => x.PerfilId == perfilId)
                .Select(x => x.PermissaoId)
                .ToListAsync();
        }

        public async Task AtualizarPermissoesPerfil(int perfilId,List<int> permissoesIds) {
            var perfilExiste = await _menuFastContext.Perfis.AnyAsync(x => x.Id == perfilId);

            if(!perfilExiste)throw new Exception("Perfil não encontrado.");
            var permissoesAtuais = await _menuFastContext.PerfilPermissoes.Where(x => x.PerfilId == perfilId).ToListAsync();

            _menuFastContext.PerfilPermissoes.RemoveRange(permissoesAtuais);

            var novasPermissoes = permissoesIds
                .Distinct()
                .Select(permissaoId => new PerfilPermissao
                {
                    PerfilId = perfilId,
                    PermissaoId = permissaoId
                });

            await _menuFastContext.PerfilPermissoes.AddRangeAsync(novasPermissoes);

            await _menuFastContext.SaveChangesAsync();
        }
    }
}




