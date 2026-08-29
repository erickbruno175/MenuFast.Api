using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Application.DTOs.Response;

public class PedidoResponse {
    public int Id { get; set; }
    public int LojaId { get; set; }
    public int? MesaId { get; set; }
    public int? ClienteId { get; set; }
    public int? FuncionarioId { get; set; }
    public StatusPedido Status { get; set; }
    public TipoPedido TipoPedido { get; set; }
    public DateTime DataPedidoHora { get; set; }
    public string? Observacao { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Desconto { get; set; }
    public decimal TaxaServico { get; set; }
    public decimal TaxaEntrega { get; set; }
    public decimal Total { get; set; }

    public ICollection<ItemPedidoResponse> Itens { get; set; } = [ ];
}

public class ItemPedidoResponse {
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public decimal Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal Desconto { get; set; }
    public decimal Total { get; set; }
    public string? Observacao { get; set; }
    public string Nome { get; set; }
}



public class ItemProducaoResponse {
    public int ItemPedidoId { get; set; }
    public int ProdutoId { get; set; }
    public decimal Quantidade { get; set; }
    public string? Observacao { get; set; }
}



public class PedidoProducaoResponse {
    public int PedidoId { get; set; }
    public int LojaId { get; set; }
    public int? MesaId { get; set; }
    public TipoPedido TipoPedido { get; set; }
    public DateTime DataPedidoHora { get; set; }
    public string? Observacao { get; set; }

    public List<ItemPedidoProducaoResponse> Itens { get; set; } = [ ];
}

public class ItemPedidoProducaoResponse {
    public int ItemPedidoId { get; set; }
    public int ProdutoId { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
    public string? Observacao { get; set; }
}