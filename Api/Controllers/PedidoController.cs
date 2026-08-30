using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Application.Services.ContextUser;
using MenuFast.Api.Api.Application.Services.PedidoServices;
using MenuFast.Api.Api.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuFast.Api.Api.Controllers;

[ApiController]
[Route("api/pedido")]
public class PedidoController : ControllerBase {
    private readonly PedidoService _pedidoService;
    private readonly UsuarioContextService _usuarioContextService;

    public PedidoController(PedidoService pedidoService, UsuarioContextService usuarioContextService) {
        _pedidoService = pedidoService;
        _usuarioContextService = usuarioContextService;
    }

    [HttpPost("cadastrar")]
    [Authorize]
    [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CadastrarPedido([FromBody] CriarPedidoRequest request) {
        var pedido = await _pedidoService.CriarPedidoAsync(request, _usuarioContextService.LojaId().Value, _usuarioContextService.FuncionarioId().Value);
        return Ok(pedido);
    }

    [HttpPost("{pedidoId}/itens")]
    [Authorize]
    [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AdicionarItens(int pedidoId, [FromBody] AdicionarItensPedidoRequest request) {
        var pedido = await _pedidoService.AdicionarItensAsync(pedidoId, request, _usuarioContextService.LojaId().Value);
        return Ok(pedido);
    }

    [HttpPut("{pedidoId}/item/{itemId}/quantidade")]
    [Authorize]
    [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AlterarQuantidadeItem(int pedidoId, int itemId, [FromBody] AlterarQuantidadeItemPedidoRequest request) {
        var pedido = await _pedidoService.AlterarQuantidadeItemAsync(pedidoId, itemId, request, _usuarioContextService.LojaId().Value);
        return Ok(pedido);
    }

    [HttpDelete("{pedidoId}/item/{itemId}")]
    [Authorize]
    [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RemoverItem(int pedidoId, int itemId) {
        var pedido = await _pedidoService.RemoverItemAsync(pedidoId, itemId, _usuarioContextService.LojaId().Value);
        return Ok(pedido);
    }

    [HttpPost("{pedidoId}/enviar")]
    [Authorize]
    [ProducesResponseType(typeof(PedidoProducaoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> EnviarPedido(int pedidoId) {
        var pedido = await _pedidoService.EnviarPedidoAsync(pedidoId, _usuarioContextService.LojaId().Value);
        return Ok(pedido);
    }

    [HttpPost("{pedidoId}/iniciar-producao")]
    [Authorize]
    [ProducesResponseType(typeof(PedidoProducaoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> IniciarProducaoPedido(int pedidoId) {
        var pedido = await _pedidoService.IniciarProducaoAsync(pedidoId, _usuarioContextService.LojaId().Value);
        return Ok(pedido);
    }

    [HttpPost("{pedidoId}/finalizar-producao")]
    [Authorize]
    [ProducesResponseType(typeof(PedidoProducaoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> FinalizarProducaoPedido(int pedidoId) {
        var pedido = await _pedidoService.FinalizarProducaoAsync(pedidoId, _usuarioContextService.LojaId().Value);
        return Ok(pedido);
    }

    [HttpPut("{pedidoId}/cancelar")]
    [Authorize]
    [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CancelarPedido(int pedidoId) {
        var pedido = await _pedidoService.CancelarAsync(pedidoId, _usuarioContextService.LojaId().Value);
        return Ok(pedido);
    }

    [HttpPut("{pedidoId}/finalizar")]
    [Authorize]
    [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> FinalizarPedido(int pedidoId) {
        var pedido = await _pedidoService.FinalizarPedidoAsync(pedidoId, _usuarioContextService.LojaId().Value);
        return Ok(pedido);
    }

    [HttpGet("buscar/{pedidoId}")]
    [Authorize]
    [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> BuscarPorId(int pedidoId) {
        var pedido = await _pedidoService.BuscarPorIdAsync(pedidoId, _usuarioContextService.LojaId().Value);
        return Ok(pedido);
    }

    [HttpGet("mesa/{mesaId}")]
    [Authorize]
    [ProducesResponseType(typeof(List<PedidoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ListarPorMesa(int mesaId) {
        var pedidos = await _pedidoService.ListarPorMesaAsync(mesaId, _usuarioContextService.LojaId().Value);
        return Ok(pedidos);
    }

    [HttpGet("mesa/{mesaId}/abertos")]
    [Authorize]
    [ProducesResponseType(typeof(List<PedidoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ListarPedidosAbertosMesa(int mesaId) {
        var pedidos = await _pedidoService.ListarPedidosAbertosMesaAsync(mesaId, _usuarioContextService.LojaId().Value);
        return Ok(pedidos);
    }

    [HttpGet("mesa/{mesaId}/ativos")]
    [Authorize]
    [ProducesResponseType(typeof(List<PedidoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ListarPedidosAtivosMesa(int mesaId) {
        var pedidos = await _pedidoService.ListarPedidosAtivosMesaAsync(mesaId, _usuarioContextService.LojaId().Value);
        return Ok(pedidos);
    }

    [HttpGet("mesa/{mesaId}/total")]
    [Authorize]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CalcularTotalMesa(int mesaId) {
        var total = await _pedidoService.CalcularTotalMesaAsync(mesaId, _usuarioContextService.LojaId().Value);
        return Ok(total);
    }

    [HttpGet("mesa/{mesaId}/total-ativo")]
    [Authorize]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CalcularTotalMesaAtiva(int mesaId) {
        var total = await _pedidoService.CalcularTotalMesaAtivaAsync(mesaId, _usuarioContextService.LojaId().Value);
        return Ok(total);
    }

    [HttpGet("listar")]
    [Authorize]
    [ProducesResponseType(typeof(List<PedidoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Listar() {
        var pedidos = await _pedidoService.ListarAsync(_usuarioContextService.LojaId().Value);
        return Ok(pedidos);
    }

    [HttpGet("listar/status/{status}")]
    [Authorize]
    [ProducesResponseType(typeof(List<PedidoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ListarPorStatus(StatusPedido status) {
        var pedidos = await _pedidoService.ListarPorStatusAsync(_usuarioContextService.LojaId().Value, status);
        return Ok(pedidos);
    }
}