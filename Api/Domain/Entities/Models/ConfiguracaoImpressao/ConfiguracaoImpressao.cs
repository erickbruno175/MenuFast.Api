namespace MenuFast.Api.Api.Domain.Entities.Models.ConfiguracaoImpressao {
    public class ConfiguracaoImpressao {
        public int Id { get; set; }
        public int? LojaId { get; set; }
        public bool ImprimirPedidoAoEnviar { get; set; }
        public bool ImprimirPedidoCozinha { get; set; }
        public bool ImprimirPedidoBar { get; set; }
        public string? ImpressoraPadrao { get; set; }
        public int LarguraPapel { get; set; } = 80;
        public bool MostrarNomeLoja { get; set; } = true;
        public bool MostrarCnpj { get; set; } = true;
        public bool MostrarEndereco { get; set; } = true;
        public bool MostrarTelefone { get; set; } = true;
        public string? MensagemRodape { get; set; }
        public Loja. Loja Loja { get; set; } = null!;
    }
}
