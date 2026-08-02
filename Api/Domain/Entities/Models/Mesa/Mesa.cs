using MenuFast.Api.Api.Domain.Entities.Models.Loja;
using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Domain.Entities.Models.Mesa;

public class Mesa {
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public int LojaId { get; set; }
    public Loja.Loja Loja { get; set; }
    public StatusMesa StatusMesa { get; set; }
    public ICollection<Pedido.Pedido> Pedidos { get; set; } = new List<Pedido.Pedido>();
}
