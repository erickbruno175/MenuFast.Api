using Microsoft.AspNetCore.Identity;

namespace MenuFast.Api.Api.Util.Helpers;

public static class SegurancaHelper {
    private static readonly PasswordHasher<string> Hasher = new();


    public static string CriarHash(string senha) {
        return Hasher.HashPassword(null!,senha);
    }

    public static bool ValidarSenha(string senha,string senhaHash) {
        var resultado = Hasher.VerifyHashedPassword(null!,senhaHash,senha);

        return resultado == PasswordVerificationResult.Success;
    }

    public static bool VerificaExpiracaoSenha(DateTime? dataExpiracao) {
        if(!dataExpiracao.HasValue) return false;

        return DateTime.Now > dataExpiracao.Value;
    }
}