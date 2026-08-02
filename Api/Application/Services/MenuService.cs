using MenuFast.Api.Api.Application.Responses.Menu;
using MenuFast.Api.Api.Persistence;
using MenuFast.Api.Api.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MenuFast.Api.Api.Services;

public class MenuService {
    private readonly MenuFastContext _context;

    public MenuService(MenuFastContext context) {
        _context = context;
    }

    public async Task<List<MenuItemResponse>> ObterMenuAsync(int funcionarioId,CancellationToken cancellationToken = default) {
        var perfilId = await _context.Funcionarios.Where(f => f.Id == funcionarioId).Select(f => f.PerfilId).FirstOrDefaultAsync(cancellationToken) ;

        if(perfilId == 0)
        {
            return new List<MenuItemResponse>();
        }

        var permissoes = await _context.PerfilPermissoes.Where(pp => pp.PerfilId == perfilId).Select(pp => pp.Permissao.Codigo).ToListAsync(cancellationToken);
        var menu = CriarMenu();

        return FiltrarMenu(menu, permissoes);
    }

    private static List<MenuItemResponse> CriarMenu() {
        return
        [
            // =====================================================
            // DASHBOARD
            // =====================================================

            new()
            {
                Nome = "Dashboard",
                Icone = "dashboard",
                Rota = "/dashboard",
                Permissao = "DASHBOARD_VISUALIZAR"
            },

            // =====================================================
            // PEDIDOS
            // =====================================================

            new()
            {
                Nome = "Pedidos",
                Icone = "receipt_long",
                Permissao = "PEDIDO_VISUALIZAR",

                Filhos =
                [
                    new()
                    {
                        Nome = "Pedidos",
                        Rota = "/pedidos",
                        Permissao = "PEDIDO_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Mesas",
                        Rota = "/mesas",
                        Permissao = "MESA_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Histórico",
                        Rota = "/pedidos/historico",
                        Permissao = "PEDIDO_VISUALIZAR_HISTORICO"
                    }
                ]
            },

            // =====================================================
            // PRODUTOS
            // =====================================================

            new()
            {
                Nome = "Produtos",
                Icone = "inventory_2",
                Permissao = "PRODUTO_VISUALIZAR",

                Filhos =
                [
                    new()
                    {
                        Nome = "Produtos",
                        Rota = "/produtos",
                        Permissao = "PRODUTO_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Categorias",
                        Rota = "/categorias",
                        Permissao = "CATEGORIA_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Complementos",
                        Rota = "/complementos",
                        Permissao = "COMPLEMENTO_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Cardápio",
                        Rota = "/cardapios",
                        Permissao = "CARDAPIO_VISUALIZAR"
                    }
                ]
            },

            // =====================================================
            // ESTOQUE
            // =====================================================

            new()
            {
                Nome = "Estoque",
                Icone = "inventory",
                Rota = "/estoque",
                Permissao = "ESTOQUE_VISUALIZAR"
            },

            // =====================================================
            // CLIENTES
            // =====================================================

            new()
            {
                Nome = "Clientes",
                Icone = "people",
                Permissao = "CLIENTE_VISUALIZAR",

                Filhos =
                [
                    new()
                    {
                        Nome = "Clientes",
                        Rota = "/clientes",
                        Permissao = "CLIENTE_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Pedidos",
                        Rota = "/clientes/pedidos",
                        Permissao = "CLIENTE_VISUALIZAR_PEDIDOS"
                    },

                    new()
                    {
                        Nome = "Histórico",
                        Rota = "/clientes/historico",
                        Permissao = "CLIENTE_VISUALIZAR_HISTORICO"
                    }
                ]
            },

            // =====================================================
            // DELIVERY
            // =====================================================

            new()
            {
                Nome = "Delivery",
                Icone = "delivery_dining",
                Permissao = "DELIVERY_VISUALIZAR",

                Filhos =
                [
                    new()
                    {
                        Nome = "Pedidos",
                        Rota = "/delivery",
                        Permissao = "DELIVERY_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Entregas",
                        Rota = "/entregas",
                        Permissao = "ENTREGA_VISUALIZAR"
                    }
                ]
            },

            // =====================================================
            // RETIRADA
            // =====================================================

            new()
            {
                Nome = "Retirada",
                Icone = "shopping_bag",
                Permissao = "RETIRADA_VISUALIZAR",
                Rota = "/retirada"
            },

            // =====================================================
            // CAIXA
            // =====================================================

            new()
            {
                Nome = "Caixa",
                Icone = "point_of_sale",
                Permissao = "CAIXA_VISUALIZAR",

                Filhos =
                [
                    new()
                    {
                        Nome = "Caixa",
                        Rota = "/caixa",
                        Permissao = "CAIXA_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Abrir caixa",
                        Rota = "/caixa/abrir",
                        Permissao = "CAIXA_ABRIR"
                    },

                    new()
                    {
                        Nome = "Histórico",
                        Rota = "/caixa/historico",
                        Permissao = "CAIXA_VISUALIZAR_HISTORICO"
                    }
                ]
            },

            // =====================================================
            // FINANCEIRO
            // =====================================================

            new()
            {
                Nome = "Financeiro",
                Icone = "payments",
                Permissao = "FINANCEIRO_VISUALIZAR",

                Filhos =
                [
                    new()
                    {
                        Nome = "Financeiro",
                        Rota = "/financeiro",
                        Permissao = "FINANCEIRO_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Contas a receber",
                        Rota = "/financeiro/contas-receber",
                        Permissao = "FINANCEIRO_VISUALIZAR_CONTAS_RECEBER"
                    },

                    new()
                    {
                        Nome = "Contas a pagar",
                        Rota = "/financeiro/contas-pagar",
                        Permissao = "FINANCEIRO_VISUALIZAR_CONTAS_PAGAR"
                    }
                ]
            },

            // =====================================================
            // FUNCIONÁRIOS
            // =====================================================

            new()
            {
                Nome = "Funcionários",
                Icone = "badge",
                Permissao = "FUNCIONARIO_VISUALIZAR",

                Filhos =
                [
                    new()
                    {
                        Nome = "Funcionários",
                        Rota = "/funcionarios",
                        Permissao = "FUNCIONARIO_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Funções",
                        Rota = "/funcoes",
                        Permissao = "FUNCAO_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Perfis",
                        Rota = "/perfis",
                        Permissao = "PERFIL_VISUALIZAR"
                    }
                ]
            },

            // =====================================================
            // RELATÓRIOS
            // =====================================================

            new()
            {
                Nome = "Relatórios",
                Icone = "assessment",
                Permissao = "RELATORIO_VISUALIZAR",

                Filhos =
                [
                    new()
                    {
                        Nome = "Vendas",
                        Rota = "/relatorios/vendas",
                        Permissao = "RELATORIO_VENDAS"
                    },

                    new()
                    {
                        Nome = "Pedidos",
                        Rota = "/relatorios/pedidos",
                        Permissao = "RELATORIO_PEDIDOS"
                    },

                    new()
                    {
                        Nome = "Caixa",
                        Rota = "/relatorios/caixa",
                        Permissao = "RELATORIO_CAIXA"
                    },

                    new()
                    {
                        Nome = "Estoque",
                        Rota = "/relatorios/estoque",
                        Permissao = "RELATORIO_ESTOQUE"
                    },

                    new()
                    {
                        Nome = "Produtos",
                        Rota = "/relatorios/produtos",
                        Permissao = "RELATORIO_PRODUTOS"
                    },

                    new()
                    {
                        Nome = "Clientes",
                        Rota = "/relatorios/clientes",
                        Permissao = "RELATORIO_CLIENTES"
                    },

                    new()
                    {
                        Nome = "Funcionários",
                        Rota = "/relatorios/funcionarios",
                        Permissao = "RELATORIO_FUNCIONARIOS"
                    },

                    new()
                    {
                        Nome = "Entregas",
                        Rota = "/relatorios/entregas",
                        Permissao = "RELATORIO_ENTREGAS"
                    },

                    new()
                    {
                        Nome = "Financeiro",
                        Rota = "/relatorios/financeiro",
                        Permissao = "RELATORIO_FINANCEIRO"
                    }
                ]
            },

            // =====================================================
            // LOJA
            // =====================================================

            new()
            {
                Nome = "Loja",
                Icone = "store",
                Permissao = "LOJA_VISUALIZAR",

                Filhos =
                [
                    new()
                    {
                        Nome = "Dados da loja",
                        Rota = "/loja",
                        Permissao = "LOJA_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Endereço",
                        Rota = "/loja/endereco",
                        Permissao = "LOJA_ALTERAR_ENDERECO"
                    },

                    new()
                    {
                        Nome = "Contatos",
                        Rota = "/loja/contatos",
                        Permissao = "LOJA_ALTERAR_CONTATOS"
                    }
                ]
            },

            // =====================================================
            // CONFIGURAÇÕES
            // =====================================================

            new()
            {
                Nome = "Configurações",
                Icone = "settings",
                Permissao = "CONFIGURACAO_VISUALIZAR",

                Filhos =
                [
                    new()
                    {
                        Nome = "Geral",
                        Rota = "/configuracoes",
                        Permissao = "CONFIGURACAO_VISUALIZAR"
                    },

                    new()
                    {
                        Nome = "Mesas",
                        Rota = "/configuracoes/mesas",
                        Permissao = "CONFIGURACAO_MESA"
                    },

                    new()
                    {
                        Nome = "Delivery",
                        Rota = "/configuracoes/delivery",
                        Permissao = "CONFIGURACAO_DELIVERY"
                    },

                    new()
                    {
                        Nome = "Retirada",
                        Rota = "/configuracoes/retirada",
                        Permissao = "CONFIGURACAO_RETIRADA"
                    },

                    new()
                    {
                        Nome = "Estoque",
                        Rota = "/configuracoes/estoque",
                        Permissao = "CONFIGURACAO_ESTOQUE"
                    },

                    new()
                    {
                        Nome = "Taxa de serviço",
                        Rota = "/configuracoes/taxa-servico",
                        Permissao = "CONFIGURACAO_TAXA_SERVICO"
                    },

                    new()
                    {
                        Nome = "Cozinha",
                        Rota = "/configuracoes/cozinha",
                        Permissao = "CONFIGURACAO_COZINHA"
                    },

                    new()
                    {
                        Nome = "Bar",
                        Rota = "/configuracoes/bar",
                        Permissao = "CONFIGURACAO_BAR"
                    },

                    new()
                    {
                        Nome = "Horário de funcionamento",
                        Rota = "/configuracoes/horario",
                        Permissao = "CONFIGURACAO_HORARIO_FUNCIONAMENTO"
                    }
                ]
            }
        ];
    }

    private static List<MenuItemResponse> FiltrarMenu(List<MenuItemResponse> menu,List<string> permissoes) {var resultado = new List<MenuItemResponse>();

        foreach(var item in menu)
        {
            var filhos = item.Filhos;

            if(filhos.Count > 0)
            {
                filhos = FiltrarMenu(filhos, permissoes);
            }

            var possuiPermissao =string.IsNullOrWhiteSpace(item.Permissao) ||permissoes.Contains(item.Permissao);

            var possuiFilhos = filhos.Count > 0;

            if(possuiPermissao && (item.Rota != null || possuiFilhos))
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
            else if(possuiFilhos)
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