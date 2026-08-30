using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Domain.Enum;
using MenuFast.Api.Api.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace MenuFast.Api.Api.Application.Services.KdsServices;

public class KdsService {
    private readonly IHubContext<KdsHub> _hubContext;

    public KdsService(IHubContext<KdsHub> hubContext) {
        _hubContext = hubContext;
    }

    public async Task EnviarPedidoAsync(PedidoProducaoResponse pedido) {
        await _hubContext.Clients.All.SendAsync("PedidoEnviado", pedido);
    }

    public async Task AtualizarStatusAsync(int lojaId, int id, StatusPedido status) {
        await _hubContext.Clients.All.SendAsync(
            "StatusPedidoAtualizado",
            new
            {
                PedidoId = id,
                LojaId = lojaId,
                Status = status
            });
    }
}