namespace MenuFast.Api.Api.Application.DTOs.Request {
    public class RedefinirSenhaRequest {
        public string Token { get; set; } = string.Empty;
        public string NovaSenha { get; set; } = string.Empty;
    }
}
