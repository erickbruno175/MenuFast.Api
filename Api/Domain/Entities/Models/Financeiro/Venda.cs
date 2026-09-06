using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Domain.Entities.Models.Financeiro {
    public class Venda {
        public int Id { get; set; }
        public int LojaId { get; set; }
        public decimal ValorBruto { get; set; }
        public decimal Desconto { get; set; }
        public decimal Acrescimo { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime DataVenda { get; set; }
        public int? FuncionarioId { get; set; }
        public StatusPagamento StatusPagamento { get; set; }
        public ICollection<PagamentoVenda> Pagamentos { get; set; } = [ ];
        public ICollection<Pedido.Pedido> Pedidos { get; set; } = [ ];

     
    }
}

