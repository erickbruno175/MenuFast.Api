namespace MenuFast.Api.Api.Application.DTOs.Response {
    public class DetalheProdutosResponse {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string? FotoProduto { get; set; }
        public string Preco { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public string? Tamanho { get; set; }
        public int? QuantidadeEstoque { get; set; }
        public int? EstoqueMinimo { get; set; }
        public string? StatusEstoque { get; set; }
        public bool EnviaParaProducao { get; set; }
    }
    public class CategoriaResponse {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
    public class AlertaEstoqueResponse {
        public string Produto { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;
    }
}
