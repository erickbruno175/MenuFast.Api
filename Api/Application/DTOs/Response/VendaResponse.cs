using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Application.DTOs.Response;

public class VendaResponse {
    public int Id { get; set; }
    public int LojaId { get; set; }
    public decimal ValorBruto { get; set; }
    public decimal Desconto { get; set; }
    public decimal Acrescimo { get; set; }
    public decimal ValorTotal { get; set; }
    public DateTime DataVenda { get; set; }
    public StatusPagamento Status { get; set; }
    public List<int> Pedidos { get; set; } = [ ];
    public List<PagamentoVendaResponse> Pagamentos { get; set; } = [ ];
    public int? FuncionarioId { get; internal set; }
    public StatusPagamento StatusPagamento { get; internal set; }
    public int PedidoId { get; internal set; }
}

public class PagamentoVendaResponse {
    public int Id { get; set; }
    public int FormaPagamentoId { get; set; }
    public decimal Valor { get; set; }
    public decimal Troco { get; set; }
}
