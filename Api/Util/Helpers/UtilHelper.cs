using MenuFast.Api.Api.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MenuFast.Api.Api.Util.Helpers {
    public static class UtilHelper {

        public static async Task<string> GerarCodigoProduto(MenuFastContext menuFastContext) {
            var ultimoCodigoGerado = await menuFastContext.Produtos.Select(p => p.Codigo).FirstOrDefaultAsync();
            if(ultimoCodigoGerado == null)
            {
                return "000100";
            }
            var codigoGerado = int.Parse(await ultimoCodigoGerado);

            return (codigoGerado + 1).ToString("D6");

        }
    }
}
