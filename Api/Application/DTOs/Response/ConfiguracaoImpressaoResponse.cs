namespace MenuFast.Api.Api.Application.DTOs.Response {
    public class ConfiguracaoImpressaoResponse {
        public int Id { get; set; }

        public bool ImprimirPedidoAoEnviar { get; set; }

        public bool ImprimirPedidoCozinha { get; set; }

        public bool ImprimirPedidoBar { get; set; }

        public string? ImpressoraPadrao { get; set; }

        public int LarguraPapel { get; set; }

        public bool MostrarNomeLoja { get; set; }

        public bool MostrarCnpj { get; set; }

        public bool MostrarEndereco { get; set; }

        public bool MostrarTelefone { get; set; }

        public string? MensagemRodape { get; set; }
    }
}
