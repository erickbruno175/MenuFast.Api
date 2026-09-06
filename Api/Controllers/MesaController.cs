using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.Services.ContextApplication;
using MenuFast.Api.Api.Application.Services.MesaServices;
using MenuFast.Api.Api.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuFast.Api.Api.Controllers {
    [ApiController]
    [Route("api/mesa")]
    [Authorize]
    public class MesaController : ControllerBase {
        private readonly MesaService _mesaService;
        private readonly ApplicationContextService _applicationContext;

        public MesaController(
            MesaService mesaService,
            ApplicationContextService applicationContextService) {

            _mesaService = mesaService;
            _applicationContext = applicationContextService;
        }

        [HttpPost]
        [Route("cadastrar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CadastrarMesa([FromBody] MesaRequest request) {
            request.LojaId = _applicationContext.LojaId()!.Value;
            var mesa = await _mesaService.CadastrarMesa(request);
            return Ok(new{mensagem = "Mesa cadastrada com sucesso.",mesa});
        }

        [HttpPut]
        [Route("atualizar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AtualizarMesa(int id,[FromBody] MesaRequest request) {

            request.LojaId = _applicationContext.LojaId()!.Value;
            var mesa = await _mesaService.AtualizarMesa(id,request);
            if(mesa == null)return NotFound(new{mensagem = "Mesa não encontrada."});

            return Ok(new{mensagem = "Mesa atualizada com sucesso.",mesa});
        }

        [HttpGet]
        [Route("listar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarMesas() {
            var mesas = await _mesaService.ListarMesas(_applicationContext.LojaId().Value);
            return Ok(mesas);
        }

        [HttpPatch]
        [Route("alterar-status/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AlterarStatusMesa(int id,[FromBody] StatusMesa status) {
            var alterado = await _mesaService.AlterarStatusMesa(id,status);return Ok(new{mensagem = "Status da mesa alterado com sucesso."});
        }

        [HttpDelete]
        [Route("remover/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ExcluirMesa(int id) {
            await _mesaService.RemoverMesa(id);
            return Ok(new{mensagem = "Mesa excluída com sucesso."});
        }
    }
}