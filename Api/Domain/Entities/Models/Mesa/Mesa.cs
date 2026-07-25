namespace MenuFast.Api.Api.Domain.Entities.Models.Mesa;

public class Mesa {
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public string? ImagemUrl { get; set; }
    public ICollection<Pedido.Pedido> Pedidos { get; set; } = new List<Pedido.Pedido>();
}
