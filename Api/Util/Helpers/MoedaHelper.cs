using System.Globalization;

namespace MenuFast.Api.Api.Util.Helpers {
    public class MoedaHelper {
        private static readonly CultureInfo CulturaBrasil = new("pt-BR");

        public static decimal ConverterParaDecimal(string? valor) {
            if(string.IsNullOrWhiteSpace(valor))
                return 0;

            if(decimal.TryParse(valor, NumberStyles.Currency, CulturaBrasil, out var resultado))
                return resultado;

            throw new FormatException("Valor monetário inválido.");
        }

        public static string FormatarReal(decimal valor) {
            return valor.ToString("C", CulturaBrasil);
        }
    }
}
