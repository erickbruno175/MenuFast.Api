using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Application.Services.ContextApplication;
using MenuFast.Api.Api.Application.Services.Email;
using MenuFast.Api.Api.Application.Services.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MenuFast.Api.Api.Controllers {
    [ApiController]
    [Route("api/seguranca")]
    public class SegurancaController : ControllerBase {

        private readonly SegurancaService _service;
        private readonly ApplicationContextService _applicationContextService;
        private readonly MenuService _menuService;
        public SegurancaController(SegurancaService service , ApplicationContextService applicationContextService , MenuService menuService) {
            _service = service;
            _applicationContextService = applicationContextService;
            _menuService = menuService;
        }

        [HttpPost("autenticar")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Autenticar([FromBody] LoginRequest request) {
            var usuario = await _service.AutenticarFuncionario(request);
            return Ok(usuario);
        }

        [HttpDelete("deslogar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<OkResult> Desloga() {
            await _service.Desloga();
            return Ok();
        }

        [HttpPost("recuperar-senha")]
        public async Task<IActionResult> RecuperarSenha(string email) {
            await _service.EsqueciSenha(email);
            return Ok(new { Mensagem = "E-mail de recuperação de senha enviado com sucesso! Um link para criar uma nova senha foi enviado para o seu e-mail.." });
        }
        [HttpPut("alterar-senha")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<OkResult> AlterarSenha([FromBody] AlterarSenhaRequest request) {
            await _service.AlterarSenhaAsync(_applicationContextService.FuncionarioId().Value,request.NovaSenha,request.ConfirmarSenha);
            return Ok();
        }

        [HttpPut("redefinir-senha")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<OkResult> RedefinirSenha([FromBody] RedefinirSenhaRequest request) {
            await _service.RedefinirSenhaAsync(request.Token,request.NovaSenha,request.ConfirmarSenha);
            return Ok();
        }
        [HttpGet]
        [Route("perfis")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListarPerfis() {
            var resultado = await _service.ListarPerfis();
            return Ok(resultado);
        }

        [HttpGet]
        [Route("permissoes")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListarPermissoes() {
            var resultado = await _service.ListarPermissoes();
            return Ok(resultado);
        }

        [HttpGet]
        [Route("perfil/{perfilId}/permissoes")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ObterPermissoesDoPerfil(int perfilId) {
            var resultado = await _service.ObterPermissoesDoPerfil(perfilId);
            return Ok(resultado);
        }

        [HttpPut]
        [Route("perfil/{perfilId}/permissoes")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtualizarPermissoesPerfil(int perfilId,[FromBody] List<int> permissoesIds) {
            await _service.AtualizarPermissoesPerfil(perfilId,permissoesIds);
            return Ok(new{mensagem = "Permissões do perfil atualizadas com sucesso."});
        }

        [HttpGet]
        [Route("menu/permissoes")]
        [Authorize]
        public async Task<IActionResult> ConsultarMenuPermisseos() {
            var funcionarioId = _applicationContextService.FuncionarioId();
            if(!funcionarioId.HasValue)return Unauthorized();
            var menu = await _menuService.ObterMenuAsync(funcionarioId.Value);

            return Ok(menu);
        }

        [HttpGet]
        [Route("teste-claims")]
        [Authorize]
        public IActionResult TestarClaims() {var claims = User.Claims.Select(x => new{x.Type,x.Value});
            return Ok(claims);
        }



    }
}
