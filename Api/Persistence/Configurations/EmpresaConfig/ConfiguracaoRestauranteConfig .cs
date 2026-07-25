using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesEmpresa;
using MenuFast.Api.Api.Domain.Entities.Models.Empresa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.ConfiguracoesEmpresa;

public class ConfiguracaoRestauranteConfig : IEntityTypeConfiguration<ConfiguracaoRestaurante> {
    public void Configure(EntityTypeBuilder<ConfiguracaoRestaurante> builder) {
        builder.ToTable("ConfiguracaoRestaurante");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(1001, 1);

        builder.Property(x => x.Id).HasComment("Identificador único da configuração do restaurante.");
        builder.Property(x => x.EmpresaId).HasComment("Empresa vinculada à configuração do restaurante.");
        builder.Property(x => x.TrabalhaComMesa).HasComment("Indica se o restaurante trabalha com controle de mesas.");
        builder.Property(x => x.TrabalhaComComanda).HasComment("Indica se o restaurante trabalha com comandas.");
        builder.Property(x => x.TrabalhaComDelivery).HasComment("Indica se o restaurante trabalha com pedidos delivery.");
        builder.Property(x => x.TrabalhaComRetirada).HasComment("Indica se o restaurante trabalha com retirada no balcão.");
        builder.Property(x => x.ControlaEstoque).HasComment("Indica se o restaurante utiliza controle de estoque.");
        builder.Property(x => x.PermiteVendaSemEstoque).HasComment("Indica se permite realizar venda de produtos sem estoque.");
        builder.Property(x => x.CobraTaxaServico).HasComment("Indica se cobra taxa de serviço.");
        builder.Property(x => x.PercentualTaxaServico).HasPrecision(5, 2).HasComment("Percentual aplicado para cobrança da taxa de serviço.");
        builder.Property(x => x.ExigirGarcomNaMesa).HasComment("Indica se é obrigatório informar garçom responsável pela mesa.");
        builder.Property(x => x.ImprimirPedidoAutomaticamente).HasComment("Indica se o pedido deve ser impresso automaticamente.");
        builder.Property(x => x.EnviarPedidoAutomaticamenteCozinha).HasComment("Indica se o pedido deve ser enviado automaticamente para a cozinha.");
        builder.Property(x => x.ExigirCaixaAberto).HasComment("Indica se exige caixa aberto para realizar vendas.");
        builder.Property(x => x.ImprimirComprovanteFechamento).HasComment("Indica se imprime comprovante no fechamento do caixa.");
        builder.Property(x => x.IdentificarClienteObrigatorio).HasComment("Indica se a identificação do cliente é obrigatória.");
        builder.Property(x => x.Ativo).HasComment("Indica se a configuração está ativa.");
        builder.HasOne<Empresa>().WithOne().HasForeignKey<ConfiguracaoRestaurante>(x => x.EmpresaId).OnDelete(DeleteBehavior.Cascade);
    }
}