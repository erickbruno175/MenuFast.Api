namespace MenuFast.Api.Api.Application.DTOs.Request;

public class ConfirmarPagamentoRequest {
    public int? PedidoId { get; set; }
    public int? MesaId { get; set; }
    public List<PagamentoRequest> Pagamentos { get; set; } = [ ];
}

public class PagamentoRequest {
    public int FormaPagamentoId { get; set; }
    public decimal Valor { get; set; }
}
