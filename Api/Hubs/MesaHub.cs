using Microsoft.AspNetCore.SignalR;

namespace MenuFast.Api.Api.Hubs {
    public class MesaHub : Hub {
        public async Task EntrarNaLoja(int lojaId) {
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                $"loja-{lojaId}"
            );
        }

        public async Task SairDaLoja(int lojaId) {
            await Groups.RemoveFromGroupAsync(
                Context.ConnectionId,
                $"loja-{lojaId}"
            );
        }
    }
}