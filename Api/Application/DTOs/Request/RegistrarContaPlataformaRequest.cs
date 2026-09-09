namespace MenuFast.Api.Api.Application.DTOs.Request {
    public class RegistrarContaPlataformaRequest {
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
    }
}
