using Microsoft.AspNetCore.Identity;

namespace MenuFast.Api.Api.Helpers;

public static class PasswordHelper {
    private static readonly PasswordHasher<string> Hasher = new();


    public static string CriarHash(string senha) {
        return Hasher.HashPassword(null!,senha);
    }

    public static bool ValidarSenha(string senha,string senhaHash) {
        var resultado = Hasher.VerifyHashedPassword(null!,senhaHash,senha);

        return resultado == PasswordVerificationResult.Success;
    }
}