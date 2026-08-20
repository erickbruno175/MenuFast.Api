using System.Text.RegularExpressions;

namespace MenuFast.Api.Api.Util.Helpers {
    public static class DocumentoHelper {
        public static string SomenteNumeros(string? valor) {
            if(string.IsNullOrWhiteSpace(valor))
                return string.Empty;

            return Regex.Replace(valor, @"\D", "");
        }

        public static string RemoverMascaraTelefone(string? telefone) {
            return SomenteNumeros(telefone);
        }

        public static bool ValidarCpf(string? cpf) {
            cpf = SomenteNumeros(cpf);

            if(cpf.Length != 11)return false;
            if(cpf.Distinct().Count() == 1)return false;

            int soma = 0;

            for(int i = 0; i < 9; i++)
            {
                soma += (cpf [ i ] - '0') * (10 - i);
            }

            int resto = soma % 11;
            int primeiroDigito = resto < 2 ? 0 : 11 - resto;

            if(primeiroDigito != cpf [ 9 ] - '0')
                return false;

            soma = 0;

            for(int i = 0; i < 10; i++)
            {
                soma += (cpf [ i ] - '0') * (11 - i);
            }

            resto = soma % 11;
            int segundoDigito = resto < 2 ? 0 : 11 - resto;

            return segundoDigito == cpf [ 10 ] - '0';
        }

        public static bool ValidarCnpj(string? cnpj) {
            cnpj = SomenteNumeros(cnpj);

            if(cnpj.Length != 14)
                return false;

            if(cnpj.Distinct().Count() == 1)
                return false;

            int [ ] pesosPrimeiro = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma = 0;

            for(int i = 0; i < 12; i++)
            {
                soma += (cnpj [ i ] - '0') * pesosPrimeiro [ i ];
            }

            int resto = soma % 11;
            int primeiroDigito = resto < 2 ? 0 : 11 - resto;

            if(primeiroDigito != cnpj [ 12 ] - '0')
                return false;

            int [ ] pesosSegundo = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            soma = 0;

            for(int i = 0; i < 13; i++)
            {
                soma += (cnpj [ i ] - '0') * pesosSegundo [ i ];
            }

            resto = soma % 11;
            int segundoDigito = resto < 2 ? 0 : 11 - resto;

            return segundoDigito == cnpj [ 13 ] - '0';
        }

        public static bool Validar(string? documento) {
            documento = SomenteNumeros(documento);

            return documento.Length switch
            {
                11 => ValidarCpf(documento),
                14 => ValidarCnpj(documento),
                _ => false
            };
        }

        public static string RemoverCaracteresEspeciais(string? valor) { if(string.IsNullOrWhiteSpace(valor)) return string.Empty; return Regex.Replace(valor, @"\D", ""); }
    }
}