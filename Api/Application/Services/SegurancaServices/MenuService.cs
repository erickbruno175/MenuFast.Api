using MenuFast.Api.Api.Application.Responses.Menu;
using MenuFast.Api.Api.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MenuFast.Api.Api.Application.Services.Seguranca;

public class MenuService {
    private readonly MenuFastContext _context;

    public MenuService(MenuFastContext context) {
        _context = context;
    }

    public async Task<List<MenuItemResponse>> ObterMenuAsync(
        int funcionarioId,
        CancellationToken cancellationToken = default) {
        var perfilId = await _context.Funcionarios
            .AsNoTracking()
            .Where(f => f.Id == funcionarioId && f.Ativo)
            .Select(f => f.PerfilId)
            .FirstOrDefaultAsync(cancellationToken);

        if(perfilId == 0)
            return [ ];

        var permissoes = await _context.PerfilPermissoes
            .AsNoTracking()
            .Where(pp => pp.PerfilId == perfilId)
            .Select(pp => pp.Permissao.Codigo)
            .ToListAsync(cancellationToken);

        var menu = CriarMenu();

        return FiltrarMenu(menu, permissoes);
    }

    private static List<MenuItemResponse> CriarMenu() {
        return
        [
           
            new()
            {
                Nome = "Dashboard",
                Icone = "dashboard",
                Rota = "/dashboard",
                Permissao = "DASHBOARD_VISUALIZAR"
            },

            new()
            {
                Nome = "Vendas",
                Icone = "point_of_sale",
                Filhos =
                [
                    new() {
                        Nome = "Venda Balcão",
                        Icone = "storefront",
                        Rota = "/balcao",
                        Permissao = "PEDIDO_CRIAR"
                    },
                    new() {
                        Nome = "Venda Mesa",
                        Icone = "table_restaurant",
                        Rota = "/vendas/mesa",
                        Permissao = "MESA_VISUALIZAR"
                    },
                    new() {
                        Nome = "Delivery",
                        Icone = "delivery_dining",
                        Rota = "/delivery",
                        Permissao = "DELIVERY_VISUALIZAR"
                    },
                    new() {
                        Nome = "Pedidos",
                        Icone = "receipt_long",
                        Rota = "/pedidos",
                        Permissao = "PEDIDO_VISUALIZAR"
                    }
                ]
            },

            new()
            {
                Nome = "Catálogo",
                Icone = "inventory_2",
                Filhos =
                [
                    new()
                    {
                        Nome = "Produtos",
                        Icone = "inventory_2",
                        Rota = "/produtos",
                        Permissao = "PRODUTO_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Categorias",
                        Icone = "category",
                        Rota = "/categorias",
                        Permissao = "CATEGORIA_VISUALIZAR"
                    },


                    new()
                    {
                        Nome = "Cardápio",
                        Icone = "menu_book",
                        Rota = "/cardapios",
                        Permissao = "CARDAPIO_VISUALIZAR"
                    }
                ]
            },

    

            new()
            {
                Nome = "Estoque",
                Icone = "warehouse",
                Rota = "/estoque",
                Permissao = "ESTOQUE_VISUALIZAR"
            },

            new()
            {
                Nome = "Clientes",
                Icone = "people",
                Filhos =
                [
                    new()
                    {
                        Nome = "Clientes",
                        Icone = "person",
                        Rota = "/clientes",
                        Permissao = "CLIENTE_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Pedidos",
                        Icone = "receipt_long",
                        Rota = "/clientes/pedidos",
                        Permissao = "CLIENTE_VISUALIZAR_PEDIDOS"
                    },

                    new()
                    {
                        Nome = "Histórico",
                        Icone = "history",
                        Rota = "/clientes/historico",
                        Permissao = "CLIENTE_VISUALIZAR_HISTORICO"
                    }
                ]
            },

      
            new()
            {
                Nome = "Caixa",
                Icone = "point_of_sale",
                Rota = "/caixa",
                Permissao = "CAIXA_VISUALIZAR"
            },

            new()
            {
                Nome = "Financeiro",
                Icone = "payments",
                Filhos =
                [
                    new()
                    {
                        Nome = "Financeiro",
                        Icone = "account_balance",
                        Rota = "/financeiro",
                        Permissao = "FINANCEIRO_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Contas a pagar",
                        Icone = "trending_down",
                        Rota = "/financeiro/contas-pagar",
                        Permissao = "FINANCEIRO_VISUALIZAR_CONTAS_PAGAR"
                    }
                ]
            },

            new()
            {
                Nome = "Formas de pagamento",
                Icone = "credit_card",
                Rota = "/formas-pagamento",
                Permissao = "PAGAMENTO_VISUALIZAR"
            },
            new()
            {
                Nome = "Funcionários",
                Icone = "badge",
                Rota = "/funcionarios",
                Permissao = "FUNCIONARIO_VISUALIZAR"
            },


            new()
            {
                Nome = "Segurança",
                Icone = "security",
                Filhos =
                [

                    new()
                    {
                        Nome = "Permissões",
                        Icone = "lock",
                        Rota = "/permissoes",
                        Permissao = "PERMISSAO_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Configurações",
                        Icone = "settings",
                        Rota = "/configuracoes-seguranca",
                        Permissao = "CONFIGURACAO_SEGURANCA_VISUALIZAR"
                    }
                ]
            },

            new()
            {
                Nome = "Relatórios",
                Icone = "assessment",
                Filhos =
                [
                    new()
                    {
                        Nome = "Vendas",
                        Icone = "bar_chart",
                        Rota = "/relatorios/vendas",
                        Permissao = "RELATORIO_VENDAS"
                    },

                    new()
                    {
                        Nome = "Pedidos",
                        Icone = "receipt_long",
                        Rota = "/relatorios/pedidos",
                        Permissao = "RELATORIO_PEDIDOS"
                    },

                    new()
                    {
                        Nome = "Caixa",
                        Icone = "point_of_sale",
                        Rota = "/relatorios/caixa",
                        Permissao = "RELATORIO_CAIXA"
                    },

                    new()
                    {
                        Nome = "Estoque",
                        Icone = "inventory",
                        Rota = "/relatorios/estoque",
                        Permissao = "RELATORIO_ESTOQUE"
                    },

                    new()
                    {
                        Nome = "Produtos",
                        Icone = "inventory_2",
                        Rota = "/relatorios/produtos",
                        Permissao = "RELATORIO_PRODUTOS"
                    },

                    new()
                    {
                        Nome = "Clientes",
                        Icone = "people",
                        Rota = "/relatorios/clientes",
                        Permissao = "RELATORIO_CLIENTES"
                    },

                    new()
                    {
                        Nome = "Funcionários",
                        Icone = "badge",
                        Rota = "/relatorios/funcionarios",
                        Permissao = "RELATORIO_FUNCIONARIOS"
                    },

                    new()
                    {
                        Nome = "Entregas",
                        Icone = "local_shipping",
                        Rota = "/relatorios/entregas",
                        Permissao = "RELATORIO_ENTREGAS"
                    },

                    new()
                    {
                        Nome = "Financeiro",
                        Icone = "payments",
                        Rota = "/relatorios/financeiro",
                        Permissao = "RELATORIO_FINANCEIRO"
                    }
                ]
            },

         

            new()
            {
                Nome = "Loja",
                Icone = "store",
                Filhos =
                [
                    new()
                    {
                        Nome = "Dados da loja",
                        Icone = "store",
                        Rota = "/loja",
                        Permissao = "LOJA_VISUALIZAR"
                    }
                ]
            },

           

            new()
            {
                Nome = "Configurações",
                Icone = "settings",
                Filhos =
                [
                    new()
                    {
                        Nome = "Geral",
                        Icone = "settings",
                        Rota = "/configuracoes",
                        Permissao = "CONFIGURACAO_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Impressão",
                        Icone = "print",
                        Rota = "/configuracoes/impressao",
                        Permissao = "CONFIGURACAO_IMPRESSAO"
                    },

                    new()
                    {
                        Nome = "Horário de funcionamento",
                        Icone = "schedule",
                        Rota = "/configuracoes/horario",
                        Permissao = "CONFIGURACAO_HORARIO_FUNCIONAMENTO"
                    },

                    new()
                    {
                        Nome = "WhatsApp / Bot",
                        Icone = "smartphone",
                        Rota = "/configuracoes/whatsapp",
                        Permissao = "CONFIGURACAO_WHATSAPP_VISUALIZAR"
                    }
                ]
            },

            new()
            {
                Nome = "Históricos",
                Icone = "history",
                Filhos =
                [
                    new()
                    {
                        Nome = "Pedidos",
                        Icone = "receipt_long",
                        Rota = "/historicos/pedidos",
                        Permissao = "HISTORICO_PEDIDO_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Caixa",
                        Icone = "point_of_sale",
                        Rota = "/historicos/caixa",
                        Permissao = "HISTORICO_CAIXA_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Estoque",
                        Icone = "inventory",
                        Rota = "/historicos/estoque",
                        Permissao = "HISTORICO_ESTOQUE_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Funcionários",
                        Icone = "badge",
                        Rota = "/historicos/funcionarios",
                        Permissao = "HISTORICO_FUNCIONARIO_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Acessos",
                        Icone = "login",
                        Rota = "/historicos/acessos",
                        Permissao = "HISTORICO_ACESSO_VISUALIZAR"
                    }
                ]
            }
        ];
    }

    private static List<MenuItemResponse> FiltrarMenu(
        List<MenuItemResponse> menu,
        List<string> permissoes) {
        var resultado = new List<MenuItemResponse>();

        foreach(var item in menu)
        {
            var filhos = item.Filhos ?? [ ];

            if(filhos.Count > 0)
                filhos = FiltrarMenu(filhos, permissoes);

            var possuiPermissao =
                string.IsNullOrWhiteSpace(item.Permissao) ||
                permissoes.Contains(item.Permissao);

            var possuiFilhos = filhos.Count > 0;

            if(possuiFilhos)
            {
                resultado.Add(new MenuItemResponse
                {
                    Nome = item.Nome,
                    Icone = item.Icone,
                    Rota = item.Rota,
                    Permissao = item.Permissao,
                    Filhos = filhos
                });

                continue;
            }

            if(!string.IsNullOrWhiteSpace(item.Rota))
            {
                if(!possuiPermissao)
                    continue;

                resultado.Add(new MenuItemResponse
                {
                    Nome = item.Nome,
                    Icone = item.Icone,
                    Rota = item.Rota,
                    Permissao = item.Permissao,
                    Filhos = [ ]
                });
            }
        }

        return resultado;
    }
}