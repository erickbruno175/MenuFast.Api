using DocumentFormat.OpenXml.InkML;
using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Application.Services.Redis;
using MenuFast.Api.Api.Application.Services.Security;
using MenuFast.Api.Api.Domain.Entities.Models.Funcionario;
using MenuFast.Api.Api.Domain.Enum;
using MenuFast.Api.Api.Persistence.Context;
using MenuFast.Api.Api.Util.Helpers;
using MenuFast.Api.Middlewares;
using Microsoft.EntityFrameworkCore;

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
        private const int MAX_TENTATIVAS_LOGIN = 5;
        private const int TEMPO_BLOQUEIO_MINUTOS = 30;
        public SegurancaService(MenuFastContext menuFastContext, JwtService jwtService, RedisService redisService, IHttpContextAccessor httpContextAccessor) {
            _menuFastContext = menuFastContext;
            jwtService = _jwtService;
            redisService = _redisService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LoginResponse> AutenticarFuncionario(LoginRequest loginRequest) {

            var hoje = DateTime.Now;
            var funcionario = _menuFastContext.Funcionarios
                .Include(x => x.Funcao)
                .Include(x => x.Perfil).FirstOrDefault(x => x.SenhaHash == loginRequest.Senha && new int [ ] { 1, 2, 3 }.Contains(x.PerfilId));

            if(funcionario == null) { throw new BusinessLogicException("Usaurio  invalido"); }
            if(!funcionario.Ativo) { throw new BusinessLogicException("Usaurio não esta ativo"); }
            if(funcionario.Bloqueado)
            {
                if(funcionario.DataBloqueio.HasValue && funcionario.DataBloqueio.Value > hoje) { throw new BusinessLogicException($" Usuario Bloquedo temporariamente, estar desbloquado apos {funcionario.DataBloqueio}"); }
            }
            if(!PasswordHelper.ValidarSenha(loginRequest.Senha, funcionario.SenhaHash))
            {
                funcionario.TentativasLogin++;
                if(funcionario.TentativasLogin >= MAX_TENTATIVAS_LOGIN)
                {
                    funcionario.Bloqueado = true;
                    funcionario.DataBloqueio = DateTime.Now.AddMinutes(TEMPO_BLOQUEIO_MINUTOS);
                    _menuFastContext.SaveChangesAsync();

                    throw new BusinessLogicException($"Usuário bloqueado por {TEMPO_BLOQUEIO_MINUTOS} minutos.");
                }
                await _menuFastContext.SaveChangesAsync();

                throw new BusinessLogicException(
                    $"Senha inválida. Tentativa {funcionario.TentativasLogin} de {MAX_TENTATIVAS_LOGIN}.");

            }

            funcionario.Bloqueado = false;
            funcionario.TentativasLogin = 0;
            funcionario.DataUltimoLogin = hoje;
            funcionario.DataBloqueio = null;

            var token = _jwtService.GerarToken(funcionario.Id, funcionario.Login, funcionario.Perfil.Descricao, funcionario.Funcao.Descricao, funcionario.Nome);
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

            });

            await _redisService.SetAsync($"id usaurio logado - {funcionario.Id}",
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
                PerfilId = funcionario.PerfilId,
                FuncaoId = funcionario.FuncaoId
            };


        }

    }
}
