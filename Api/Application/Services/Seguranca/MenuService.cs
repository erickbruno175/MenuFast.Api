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
            .Where(f => f.Id == funcionarioId)
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
            // =========================================================
            // DASHBOARD
            // =========================================================

            new()
            {
                Nome = "Dashboard",
                Icone = "dashboard",
                Rota = "/dashboard",
                Permissao = "DASHBOARD_VISUALIZAR"
            },

            // =========================================================
            // VENDAS
            // =========================================================

            new()
            {
                Nome = "Vendas",
                Icone = "point_of_sale",
                Filhos =
                [
                    new()
                    {
                        Nome = "Pedidos",
                        Icone = "receipt_long",
                        Rota = "/pedidos",
                        Permissao = "PEDIDO_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Mesas",
                        Icone = "table_restaurant",
                        Rota = "/mesas",
                        Permissao = "MESA_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Delivery",
                        Icone = "delivery_dining",
                        Rota = "/delivery",
                        Permissao = "DELIVERY_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Retirada",
                        Icone = "shopping_bag",
                        Rota = "/retirada",
                        Permissao = "RETIRADA_VISUALIZAR"
                    }
                ]
            },

            // =========================================================
            // CATÁLOGO
            // =========================================================

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
                        Nome = "Complementos",
                        Icone = "add_circle",
                        Rota = "/complementos",
                        Permissao = "COMPLEMENTO_VISUALIZAR"
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

            // =========================================================
            // ESTOQUE
            // =========================================================

            new()
            {
                Nome = "Estoque",
                Icone = "warehouse",
                Rota = "/estoque",
                Permissao = "ESTOQUE_VISUALIZAR"
            },

            // =========================================================
            // CLIENTES
            // =========================================================

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

            // =========================================================
            // DELIVERY
            // =========================================================

            new()
            {
                Nome = "Delivery",
                Icone = "delivery_dining",
                Filhos =
                [
                    new()
                    {
                        Nome = "Pedidos",
                        Icone = "receipt_long",
                        Rota = "/delivery",
                        Permissao = "DELIVERY_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Entregas",
                        Icone = "local_shipping",
                        Rota = "/entregas",
                        Permissao = "ENTREGA_VISUALIZAR"
                    }
                ]
            },

            // =========================================================
            // RETIRADA
            // =========================================================

            new()
            {
                Nome = "Retirada",
                Icone = "shopping_bag",
                Rota = "/retirada",
                Permissao = "RETIRADA_VISUALIZAR"
            },

            // =========================================================
            // CAIXA
            // =========================================================

            new()
            {
                Nome = "Caixa",
                Icone = "point_of_sale",
                Filhos =
                [
                    new()
                    {
                        Nome = "Caixa",
                        Icone = "point_of_sale",
                        Rota = "/caixa",
                        Permissao = "CAIXA_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Abrir caixa",
                        Icone = "lock_open",
                        Rota = "/caixa/abrir",
                        Permissao = "CAIXA_ABRIR"
                    },

                    new()
                    {
                        Nome = "Sangria",
                        Icone = "remove_circle",
                        Rota = "/caixa/sangria",
                        Permissao = "CAIXA_SANGRIA"
                    },

                    new()
                    {
                        Nome = "Suprimento",
                        Icone = "add_circle",
                        Rota = "/caixa/suprimento",
                        Permissao = "CAIXA_SUPRIMENTO"
                    },

                    new()
                    {
                        Nome = "Movimentos",
                        Icone = "swap_vert",
                        Rota = "/caixa/movimentos",
                        Permissao = "CAIXA_MOVIMENTO"
                    },

                    new()
                    {
                        Nome = "Conferência",
                        Icone = "fact_check",
                        Rota = "/caixa/conferencia",
                        Permissao = "CAIXA_CONFERIR"
                    },

                    new()
                    {
                        Nome = "Histórico",
                        Icone = "history",
                        Rota = "/caixa/historico",
                        Permissao = "CAIXA_VISUALIZAR_HISTORICO"
                    }
                ]
            },

            // =========================================================
            // FINANCEIRO
            // =========================================================

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
                        Nome = "Contas a receber",
                        Icone = "trending_up",
                        Rota = "/financeiro/contas-receber",
                        Permissao = "FINANCEIRO_VISUALIZAR_CONTAS_RECEBER"
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

            // =========================================================
            // FORMAS DE PAGAMENTO
            // =========================================================

            new()
            {
                Nome = "Formas de pagamento",
                Icone = "credit_card",
                Rota = "/formas-pagamento",
                Permissao = "PAGAMENTO_VISUALIZAR"
            },

            // =========================================================
            // FUNCIONÁRIOS
            // =========================================================

            new()
            {
                Nome = "Funcionários",
                Icone = "badge",
                Rota = "/funcionarios",
                Permissao = "FUNCIONARIO_VISUALIZAR"
            },

            // =========================================================
            // SEGURANÇA
            // =========================================================

            new()
            {
                Nome = "Segurança",
                Icone = "security",
                Filhos =
                [
                    new()
                    {
                        Nome = "Funções",
                        Icone = "work",
                        Rota = "/funcoes",
                        Permissao = "FUNCAO_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Perfis",
                        Icone = "manage_accounts",
                        Rota = "/perfis",
                        Permissao = "PERFIL_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Permissões",
                        Icone = "lock",
                        Rota = "/permissoes",
                        Permissao = "PERMISSAO_VISUALIZAR"
                    }
                ]
            },

            // =========================================================
            // RELATÓRIOS
            // =========================================================

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

            // =========================================================
            // LOJA
            // =========================================================

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
                    },

                    new()
                    {
                        Nome = "Endereço",
                        Icone = "location_on",
                        Rota = "/loja/endereco",
                        Permissao = "LOJA_ALTERAR_ENDERECO"
                    },

                    new()
                    {
                        Nome = "Contatos",
                        Icone = "contacts",
                        Rota = "/loja/contatos",
                        Permissao = "LOJA_ALTERAR_CONTATOS"
                    }
                ]
            },

            // =========================================================
            // CONFIGURAÇÕES
            // =========================================================

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
                        Nome = "Mesas",
                        Icone = "table_restaurant",
                        Rota = "/configuracoes/mesas",
                        Permissao = "CONFIGURACAO_MESA"
                    },

                    new()
                    {
                        Nome = "Delivery",
                        Icone = "delivery_dining",
                        Rota = "/configuracoes/delivery",
                        Permissao = "CONFIGURACAO_DELIVERY"
                    },

                    new()
                    {
                        Nome = "Retirada",
                        Icone = "shopping_bag",
                        Rota = "/configuracoes/retirada",
                        Permissao = "CONFIGURACAO_RETIRADA"
                    },

                    new()
                    {
                        Nome = "Estoque",
                        Icone = "inventory",
                        Rota = "/configuracoes/estoque",
                        Permissao = "CONFIGURACAO_ESTOQUE"
                    },

                    new()
                    {
                        Nome = "Taxa de serviço",
                        Icone = "percent",
                        Rota = "/configuracoes/taxa-servico",
                        Permissao = "CONFIGURACAO_TAXA_SERVICO"
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
                        Nome = "Cozinha",
                        Icone = "restaurant",
                        Rota = "/configuracoes/cozinha",
                        Permissao = "CONFIGURACAO_COZINHA"
                    },

                    new()
                    {
                        Nome = "Bar",
                        Icone = "local_bar",
                        Rota = "/configuracoes/bar",
                        Permissao = "CONFIGURACAO_BAR"
                    },

                    new()
                    {
                        Nome = "Horário de funcionamento",
                        Icone = "schedule",
                        Rota = "/configuracoes/horario",
                        Permissao = "CONFIGURACAO_HORARIO_FUNCIONAMENTO"
                    }
                ]
            },

            // =========================================================
            // HISTÓRICOS
            // =========================================================

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
            {
                filhos = FiltrarMenu(filhos, permissoes);
            }

            var possuiPermissao =
                string.IsNullOrWhiteSpace(item.Permissao) ||
                permissoes.Contains(item.Permissao);

            var possuiFilhos = filhos.Count > 0;

            // Item folha:
            // precisa possuir a permissão.
            if(!possuiFilhos && !string.IsNullOrWhiteSpace(item.Rota))
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

                continue;
            }

            // Menu pai:
            // não precisa ter permissão própria.
            // Basta possuir pelo menos um filho permitido.
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
            }
        }

        return resultado;
    }
}