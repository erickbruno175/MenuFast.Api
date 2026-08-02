using MenuFast.Api.Api.Domain.Entities.Models.Loja;
using MenuFast.Api.Api.Domain.Entities.Models.Mesa;
using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Domain.Entities.Models.Pedido;

public class Pedido {
    public int Id { get; set; }
    public int? MesaId { get; set; }
    public int? ClienteId { get; set; }
    public int? FuncionarioId { get; set; }
    public StatusPedido Status { get; set; }
    public TipoPedido TipoPedido { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Desconto { get; set; }
    public decimal TaxaServico { get; set; }
    public decimal TaxaEntrega { get; set; }
    public decimal Total { get; set; }
    public DateTime DataPedidoHora { get; set; }
    public string? Observacao { get; set; }
    public ICollection<ItemPedido> Itens { get; set; } = [ ];
    public ICollection<PagamentoPedido> Pagamentos { get; set; } = [ ];
    public Entrega? Entrega { get; set; }
    public Mesa.Mesa? Mesa { get; set; } = null!;
    public Funcionario.Funcionario Funcionario { get; set; }
    public int LojaId { get; set; }
    public Loja.Loja Loja { get; set; }
}