namespace MenuFast.Api.Api.Domain.Constantes {
    public class LinkEmail {

        public const string LinkRecuperarSenha = "https://localhost:7290/recuperar-senha?token={0}";
        public const string LinkConfirmarEmail = "https://localhost:7290/confirmar-email?token={0}";
        public const string LinkAlterarEmail = "https://localhost:7290/alterar-email?token={0}";

        public static char LinkConfirmarAlteracaoSenha { get; internal set; }
    }
}
