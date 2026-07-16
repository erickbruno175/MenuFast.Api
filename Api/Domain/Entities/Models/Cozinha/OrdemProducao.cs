using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Domain.Entities.Models.Cozinha {
    public class OrdemProducao {
            public Guid Id { get; set; }
            public Guid PedidoId { get; set; }
            public Guid? FuncionarioId { get; set; }
            public StatusPedido Status { get; set; }
            public int Prioridade { get; set; }
            public DateTime DataEntrada { get; set; }
            public DateTime? InicioPreparo { get; set; }
            public DateTime? FimPreparo { get; set; }
            public string? Observacao { get; set; }
        
    }
}
