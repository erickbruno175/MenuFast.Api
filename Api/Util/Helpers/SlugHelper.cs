using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MenuFast.Api.Api.Util.Helpers;

public static class SlugHelper {
    public static string RemoverAcentos(string texto) {
        var normalized = texto.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach(var c in normalized)
        {
            if(CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    public static string GerarSlug(string texto) {
        texto = RemoverAcentos(texto)
            .ToLowerInvariant()
            .Trim();

        texto = Regex.Replace(texto, @"[^a-z0-9\s-]", "");
        texto = Regex.Replace(texto, @"\s+", "-");
        texto = Regex.Replace(texto, @"-+", "-");

        var prefixo = Guid.NewGuid()
            .ToString("N")
            .Substring(0, 6);
            
        return $"{texto}-{prefixo}";
    }
}