namespace MenuFast.Api.Api.Domain.Entities.Models.Financeiro {
    public class PagamentoVenda {
        public int Id { get; set; }
        public int VendaId { get; set; }
        public int FormaPagamentoId { get; set; }
        public decimal Valor { get; set; }
        public decimal Troco { get; set; }
        public Venda Venda { get; set; }
    }
}
