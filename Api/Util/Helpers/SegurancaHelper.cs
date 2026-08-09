using BCrypt.Net;

namespace MenuFast.Api.Api.Util.Helpers;

public static class SegurancaHelper {
    public static string CriarHash(string senha) {
        return BCrypt.Net.BCrypt.HashPassword(senha);
    }

    public static bool ValidarSenha(string senha, string senhaHash) {
        if(string.IsNullOrWhiteSpace(senhaHash))
            return false;

        try
        {
            return BCrypt.Net.BCrypt.Verify(senha, senhaHash);
        }
        catch(SaltParseException)
        {
            return false;
        }
    }

    public static bool VerificaExpiracaoSenha(DateTime? dataExpiracao) {
        if(!dataExpiracao.HasValue)
            return false;

        return DateTime.Now > dataExpiracao.Value;
    }
}