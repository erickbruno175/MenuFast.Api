namespace MenuFast.Api.Api.Application.DTOs.Request {
    public class LoginRequest {
        public string Email { get; set; }
        public string Senha { get; set; }
    }

    public class AlterarSenhaRequest {
        public string NovaSenha { get; set; } = string.Empty;
        public string ConfirmarSenha { get; set; } = string.Empty;
    }

    public class RedefinirSenhaRequest {
        public string Token { get; set; } = string.Empty;
        public string NovaSenha { get; set; } = string.Empty;
        public string ConfirmarSenha { get; set; } = string.Empty;
    }
}
