using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Application.Services.ContextUser;
using MenuFast.Api.Api.Application.Services.VendaService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuFast.Api.Api.Controllers {
    [ApiController]
    [Route("api/venda")]
    [Authorize]
    public class VendaController : ControllerBase {
        private readonly VendaService _vendaService;
        private readonly UsuarioContextService _usuarioContext;

        public VendaController(
            VendaService vendaService,
            UsuarioContextService usuarioContextService) {

            _vendaService = vendaService;
            _usuarioContext = usuarioContextService;
        }

        [HttpPost]
        [Route("finalizar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(VendaResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> FinalizarVenda([FromBody] ConfirmarPagamentoRequest request) {
            var lojaId = _usuarioContext.LojaId()!.Value;
            var venda = await _vendaService.FinalizarVendaAsync(lojaId, request);

            return Ok(new { mensagem = "Venda finalizada com sucesso.", venda });
        }

        [HttpGet]
        [Route("{vendaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(VendaResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> BuscarPorId(int vendaId) {
            var lojaId = _usuarioContext.LojaId()!.Value;
            var venda = await _vendaService.BuscarPorIdAsync(vendaId, lojaId);

            return Ok(venda);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<VendaResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar() {
            var lojaId = _usuarioContext.LojaId()!.Value;
            var vendas = await _vendaService.ListarAsync(lojaId);

            return Ok(vendas);
        }

        [HttpGet]
        [Route("periodo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<VendaResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarPorPeriodo(DateTime inicio, DateTime fim) {
            var lojaId = _usuarioContext.LojaId()!.Value;
            var vendas = await _vendaService.ListarPorPeriodoAsync(lojaId, inicio, fim);

            return Ok(vendas);
        }

        [HttpGet]
        [Route("pedido/{pedidoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarPorPedido(int pedidoId) {
            var lojaId = _usuarioContext.LojaId()!.Value;
            var vendas = await _vendaService.ListarPorPedidoAsync(lojaId, pedidoId);

            return Ok(vendas);
        }

        [HttpGet]
        [Route("mesa/{mesaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<VendaResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarPorMesa(int mesaId) {
            var lojaId = _usuarioContext.LojaId()!.Value;
            var vendas = await _vendaService.ListarPorMesaAsync(lojaId, mesaId);

            return Ok(vendas);
        }

        [HttpGet]
        [Route("calcular-total")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CalcularTotal(int? pedidoId, int? mesaId) {
            var lojaId = _usuarioContext.LojaId()!.Value;

            var request = new ConfirmarPagamentoRequest
            {
                PedidoId = pedidoId,
                MesaId = mesaId
            };

            var total = await _vendaService.CalcularTotalAsync(lojaId, request);

            return Ok(new { total });
        }

        [HttpPost]
        [Route("{vendaId}/cancelar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(VendaResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CancelarVenda(int vendaId) {
            var lojaId = _usuarioContext.LojaId()!.Value;
            var venda = await _vendaService.CancelarVendaAsync(vendaId, lojaId);

            return Ok(new { mensagem = "Venda cancelada com sucesso.", venda });
        }

        [HttpPost]
        [Route("{vendaId}/estornar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(VendaResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> EstornarVenda(int vendaId) {
            var lojaId = _usuarioContext.LojaId()!.Value;
            var venda = await _vendaService.EstornarVendaAsync(vendaId, lojaId);

            return Ok(new { mensagem = "Venda estornada com sucesso.", venda });
        }
    }
}
