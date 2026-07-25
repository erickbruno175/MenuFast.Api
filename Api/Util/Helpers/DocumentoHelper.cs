using System.Text.RegularExpressions;

namespace MenuFast.Api.Api.Util.Helpers {
    public class DocumentoHelper {

        public static string RemoverCaracteresEspeciais(string? valor) {
            if(string.IsNullOrWhiteSpace(valor))
             return string.Empty;
            
            return Regex.Replace(valor, @"\D", "");

        }

        public static string RemoverMascaraTelefone(string? telefone) {
            if(string.IsNullOrWhiteSpace(telefone))
                return string.Empty;

            return Regex.Replace(telefone, @"\D", "");
        }
}
