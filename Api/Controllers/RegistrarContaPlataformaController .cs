using Microsoft.AspNetCore.Mvc;
using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.Services.RegistrarContaPlataforma;

namespace MenuFast.Api.Api.Controllers {
    [ApiController]
    [Route("api/plataforma")]
    public class RegistrarContaPlataformaController : ControllerBase {
        private readonly RegistrarContaPlataforma _registrarContaPlataforma;

        public RegistrarContaPlataformaController(RegistrarContaPlataforma registrarContaPlataforma) {
            _registrarContaPlataforma = registrarContaPlataforma;
        }

        [HttpPost("registrar/{lojaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registrar(
            [FromBody] RegistrarContaPlataformaRequest request,
            [FromRoute] int lojaId) {
            await _registrarContaPlataforma.RegistrarAsync(request, lojaId);

            return Ok(new
            {
                mensagem = "Conta registrada com sucesso.",
                error = false
            });
        }

        [HttpDelete("cancelar/{lojaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Cancelar([FromRoute] int lojaId) {
            await _registrarContaPlataforma.CancelarAsync(lojaId);

            return Ok(new
            {
                mensagem = "Conta cancelada com sucesso.",
                error = false
            });
        }
    }
}