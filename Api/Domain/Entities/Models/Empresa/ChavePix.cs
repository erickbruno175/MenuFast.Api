using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Domain.Entities.Models.Empresa;

public class ChavePix {
    public Guid Id { get; set; }
    public Guid ContaBancariaId { get; set; }
    public TipoChavePix Tipo { get; set; }
    public string Chave { get; set; } = string.Empty;
    public bool Principal { get; set; }
    public ContaBancaria? ContaBancaria { get; set; }
}