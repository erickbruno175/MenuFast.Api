using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Application.Services.ContextApplication;
using MenuFast.Api.Api.Application.Services.PedidoServices;
using MenuFast.Api.Api.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuFast.Api.Api.Controllers {
    [ApiController]
    [Route("api/pedido")]
    [Authorize]
    public class PedidoController : ControllerBase {
        private readonly PedidoService _pedidoService;
        private readonly ApplicationContextService _applicationContextService;

        public PedidoController(PedidoService pedidoService,ApplicationContextService applicationContextService) {

            _pedidoService = pedidoService;
            _applicationContextService = applicationContextService;
        }

        [HttpPost]
        [Route("cadastrar")]
        [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CadastrarPedido([FromBody] CriarPedidoRequest request) {
            var pedido = await _pedidoService.CriarPedidoAsync(request,_applicationContextService.LojaId().Value,_applicationContextService.FuncionarioId().Value);
            return Ok(pedido);
        }

        [HttpPost]
        [Route("{pedidoId}/itens")]
        [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AdicionarItens(int pedidoId,[FromBody] AdicionarItensPedidoRequest request) {
            var pedido = await _pedidoService.AdicionarItensAsync(pedidoId,request,_applicationContextService.LojaId().Value);
            return Ok(pedido);
        }

        [HttpPut]
        [Route("{pedidoId}/item/{itemId}/quantidade")]
        [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AlterarQuantidadeItem(int pedidoId,int itemId,[FromBody] AlterarQuantidadeItemPedidoRequest request) {
            var pedido = await _pedidoService.AlterarQuantidadeItemAsync(pedidoId,itemId,request,_applicationContextService.LojaId().Value);
            return Ok(pedido);
        }

        [HttpDelete]
        [Route("{pedidoId}/item/{itemId}")]
        [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RemoverItem(int pedidoId,int itemId) {
            var pedido = await _pedidoService.RemoverItemAsync(pedidoId,itemId,_applicationContextService.LojaId().Value);
            return Ok(pedido);
        }

        [HttpPost]
        [Route("{pedidoId}/enviar")]
        [ProducesResponseType(typeof(PedidoProducaoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> EnviarPedido(int pedidoId) {
            var pedido = await _pedidoService.EnviarPedidoAsync(pedidoId,_applicationContextService.LojaId().Value);
            return Ok(pedido);
        }

        [HttpPost]
        [Route("{pedidoId}/iniciar-producao")]
        [ProducesResponseType(typeof(PedidoProducaoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> IniciarProducaoPedido(int pedidoId) {
            var pedido = await _pedidoService.IniciarProducaoAsync(pedidoId,_applicationContextService.LojaId().Value);
            return Ok(pedido);
        }

        [HttpPost]
        [Route("{pedidoId}/finalizar-producao")]
        [ProducesResponseType(typeof(PedidoProducaoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> FinalizarProducaoPedido(int pedidoId) {
            var pedido = await _pedidoService.FinalizarProducaoAsync(pedidoId,_applicationContextService.LojaId().Value);
            return Ok(pedido);
        }

        [HttpPut]
        [Route("{pedidoId}/cancelar")]
        [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CancelarPedido(int pedidoId) {
            var pedido = await _pedidoService.CancelarAsync(pedidoId,_applicationContextService.LojaId().Value);
            return Ok(pedido);
        }

        [HttpPut]
        [Route("iniciar-fechamento")]
        [ProducesResponseType(typeof(List<PedidoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> IniciarFechamento([FromQuery] int? pedidoId,[FromQuery] int? mesaId) {
            var pedidos = await _pedidoService.IniciarFechamentoAsync(_applicationContextService.LojaId().Value,pedidoId,mesaId);
            return Ok(pedidos);
        }

        [HttpGet]
        [Route("buscar/{pedidoId}")]
        [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> BuscarPorId(int pedidoId) {
            var pedido = await _pedidoService.BuscarPorIdAsync(pedidoId,_applicationContextService.LojaId().Value);
            return Ok(pedido);
        }

        [HttpGet]
        [Route("mesa/{mesaId}")]
        [ProducesResponseType(typeof(List<PedidoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListarPorMesa(int mesaId) {
            var pedidos = await _pedidoService.ListarPorMesaAsync(mesaId,_applicationContextService.LojaId().Value);
            return Ok(pedidos);
        }

        [HttpGet]
        [Route("mesa/{mesaId}/abertos")]
        [ProducesResponseType(typeof(List<PedidoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListarPedidosAbertosMesa(int mesaId) {
            var pedidos = await _pedidoService.ListarPedidosAbertosMesaAsync(mesaId,_applicationContextService.LojaId().Value);
            return Ok(pedidos);
        }

        [HttpGet]
        [Route("mesa/{mesaId}/ativos")]
        [ProducesResponseType(typeof(List<PedidoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListarPedidosAtivosMesa(int mesaId) {

            var pedidos = await _pedidoService.ListarPedidosAtivosMesaAsync(mesaId,_applicationContextService.LojaId().Value);
            return Ok(pedidos);
        }

        [HttpGet]
        [Route("mesa/{mesaId}/total")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CalcularTotalMesa(int mesaId) {
            var total = await _pedidoService.CalcularTotalMesaAsync(mesaId,_applicationContextService.LojaId().Value);
            return Ok(total);
        }

        [HttpGet]
        [Route("mesa/{mesaId}/total-ativo")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CalcularTotalMesaAtiva(int mesaId) {
            var total = await _pedidoService.CalcularTotalMesaAtivaAsync(mesaId,_applicationContextService.LojaId().Value);
            return Ok(total);
        }

        [HttpGet]
        [Route("listar")]
        [ProducesResponseType(typeof(List<PedidoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Listar() {
            var pedidos = await _pedidoService.ListarAsync(_applicationContextService.LojaId().Value);
            return Ok(pedidos);
        }

        [HttpGet]
        [Route("listar/status/{status}")]
        [ProducesResponseType(typeof(List<PedidoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListarPorStatus(StatusPedido status) {
            var pedidos = await _pedidoService.ListarPorStatusAsync(_applicationContextService.LojaId().Value,status);
            return Ok(pedidos);
        }

        [HttpPost]
        [Route("transferir-pedidos-outra-mesa/{mesaOrigemId}/{mesaDestinoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> TransferirPedidosMesa(int mesaOrigemId,int mesaDestinoId) {
            await _pedidoService.TransferirPedidosMesaAsync(mesaOrigemId,mesaDestinoId,_applicationContextService.LojaId().Value);
            return Ok();
        }
    }
}