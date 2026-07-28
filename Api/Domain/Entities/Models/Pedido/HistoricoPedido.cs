using MenuFast.Api.Api.Domain.Entities.Models.Empresa;
using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Domain.Entities.Models.Pedido {
    public class HistoricoPedido {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int FuncionarioId { get; set; }
        public AcaoHistoricoPedido Acao { get; set; } 
        public string? Observacao { get; set; }
        public DateTime Data { get; set; }
        public int UsuarioId { get; set; }
        public Pedido Pedido { get; set; } = null!;
        public int LojaId { get; set; }
        public Loja Loja { get; set; }
    }
}
