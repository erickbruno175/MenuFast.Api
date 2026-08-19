namespace MenuFast.Api.Api.Application.DTOs.Response {
    public class DetalheProdutos {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string  FotoProduto { get; set; }
        public  string Preco { get; set; }
        public string Codigo { get; internal set; }
        public string? Descricao { get; set; }
        public bool Ativo { get; internal set; }
    }

    public class CategoriaResponse {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

}
