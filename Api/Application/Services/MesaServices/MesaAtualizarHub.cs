using MenuFast.Api.Api.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace MenuFast.Api.Api.Application.Services.MesaServices {
    public class MesaAtualizarHub {
        private readonly IHubContext<MesaHub> _hubContext;

        public MesaAtualizarHub(IHubContext<MesaHub> hubContext) {
            _hubContext = hubContext;
        }

        public async Task AtualizarMesaAsync(int lojaId,int mesaId,object mesa) {
            await _hubContext.Clients.Group($"loja-{lojaId}").SendAsync("MesaAtualizada",mesa);
        }
    }
}