using MenuFast.Api.Api.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MenuFast.Api.Api.Util.Helpers {
    public static class UtilHelper {

        public static  string GerarCodigoProduto(MenuFastContext menuFastContext) {
            string ultimoCodigoGerado = menuFastContext.Produtos.Select(p => p.Codigo).FirstOrDefault();
            if(ultimoCodigoGerado == null)
            {
                return "000100";
            }
            var codigoGerado = int.Parse(ultimoCodigoGerado);

            return (codigoGerado + 1).ToString("D6");

        }
    }
}
