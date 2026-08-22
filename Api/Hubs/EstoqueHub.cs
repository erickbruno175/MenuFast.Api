using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace MenuFast.Api.Api.Hubs {
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class EstoqueHub:Hub {
        private string GetDebuggerDisplay() {
            return ToString();
        }
    }
}
