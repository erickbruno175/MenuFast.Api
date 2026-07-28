using MenuFast.Api.Api.Domain.Entities.Models.Empresa;
using MenuFast.Api.Api.Domain.Entities.Models.Funcionario;
using MenuFast.Api.Api.Domain.Entities.Models.Pedido;

public class Entregador {
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string MarcaMoto { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string Cor {  get; set; } = string.Empty;
    public DateTime Ano {  get; set; } 
    public string Placa {  get; set; } = string.Empty;
    public ICollection<Entrega> Entregas { get; set; } = [ ];

    public int FuncionarioId { get; set; }
    public Funcionario Funcionario { get; set; }
    public int LojaId { get; set; }
    public Loja Loja { get; set; }


}