using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Application.Services.ContextUser;
using MenuFast.Api.Api.Application.Services.Email;
using MenuFast.Api.Api.Application.Services.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuFast.Api.Api.Controllers {
    [ApiController]
    [Route("api/seguranca")]
    public class SegurancaControllers : ControllerBase {

        private readonly SegurancaService _service;
        public SegurancaControllers(SegurancaService service) {
            _service = service;
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
            await _service.RedefinirSenhas(email);
            return Ok(new { Mensagem = "E-mail de recuperação de senha enviado com sucesso." });
        }

    }
}
