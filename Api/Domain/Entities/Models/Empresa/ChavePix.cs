using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Domain.Entities.Models.Empresa;

public class ChavePix {
    public int Id { get; set; }
    public int ContaBancariaId { get; set; }
    public TipoChavePix Tipo { get; set; }
    public string Chave { get; set; } = string.Empty;
    public bool Principal { get; set; }
    public ContaBancaria? ContaBancaria { get; set; }
}