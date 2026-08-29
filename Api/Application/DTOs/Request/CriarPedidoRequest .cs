using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Application.DTOs.Request;

public class CriarPedidoRequest {
    public int? MesaId { get; set; }
    public int? ClienteId { get; set; }
    public TipoPedido TipoPedido { get; set; }
    public string? Observacao { get; set; }

    public List<ItemPedidoRequest> Itens { get; set; } = [ ];
}

public class AdicionarItensPedidoRequest {
    public List<ItemPedidoRequest> Itens { get; set; } = [ ];
}

public class ItemPedidoRequest {
    public int ProdutoId { get; set; }
    public decimal Quantidade { get; set; }
    public decimal Desconto { get; set; }
    public string? Observacao { get; set; }
}

public class AlterarQuantidadeItemPedidoRequest {
    public decimal Quantidade { get; set; }
}
