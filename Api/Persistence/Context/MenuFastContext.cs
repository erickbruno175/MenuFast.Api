using MenuFast.Api.Api.Domain.Entities.Models.Cardapio;
using MenuFast.Api.Api.Domain.Entities.Models.Cliente;
using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesLoja;
using MenuFast.Api.Api.Domain.Entities.Models.Cozinha;
using MenuFast.Api.Api.Domain.Entities.Models.Loja;
using MenuFast.Api.Api.Domain.Entities.Models.Financeiro;
using MenuFast.Api.Api.Domain.Entities.Models.Funcionario;
using MenuFast.Api.Api.Domain.Entities.Models.Mesa;
using MenuFast.Api.Api.Domain.Entities.Models.Pedido;
using MenuFast.Api.Api.Domain.Entities.Models.Seguranca;
using Microsoft.EntityFrameworkCore;
using MenuFast.Api.Api.Domain.Entities.Models;

namespace MenuFast.Api.Api.Persistence.Context;

public class MenuFastContext : DbContext {
    public MenuFastContext(DbContextOptions<MenuFastContext> options)
        : base(options) {
    }

    public DbSet<Cardapio> Cardapios { get; set; }
    public DbSet<CategoriaProduto> CategoriasProdutos { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<OpcaoProduto> OpcoesProdutos { get; set; }
    public DbSet<Complemento> Complementos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<ConfiguracaoLoja> ConfiguracoesLoja{ get; set; }
    public DbSet<FormaPagamento> FormasPagamento { get; set; }
    public DbSet<HorarioFuncionamento> HorariosFuncionamento { get; set; }
    public DbSet<OrdemProducao> OrdensProducao { get; set; }
    public DbSet<Loja> Lojas { get; set; }
    public DbSet<ContaBancaria> ContasBancarias { get; set; }
    public DbSet<ChavePix> ChavesPix { get; set; }
    public DbSet<Caixa> Caixas { get; set; }
    public DbSet<ContaPagar> ContasPagar { get; set; }
    public DbSet<ContaReceber> ContasReceber { get; set; }
    public DbSet<MovimentoCaixa> MovimentosCaixa { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Mesa> Mesas { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItensPedido { get; set; }
    public DbSet<PagamentoPedido> PagamentosPedido { get; set; }
    public DbSet<HistoricoPedido> HistoricosPedido { get; set; }
    public DbSet<Entrega> Entregas { get; set; }
    public DbSet<HistoricoPedido> HistoricoPedidos { get; set; }
    public DbSet<ItemPedido> ItemsPedido { get; set; }
    public DbSet<Perfil> Perfis { get; set; }
    public DbSet<Permissao> Permissoes { get; set; }
    public DbSet<PerfilPermissao> PerfilPermissoes { get; set; }
    public DbSet<HistoricoAcesso> HistoricoAcessos { get; set; }
    public DbSet<ConfiguracaoSeguranca> ConfiguracoesSeguranca { get; set; }
    public DbSet<TemplateEmail> TemplatesEmail { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MenuFastContext).Assembly);
    }
}