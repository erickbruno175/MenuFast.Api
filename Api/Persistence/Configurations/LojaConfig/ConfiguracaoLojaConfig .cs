using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesLoja;
using MenuFast.Api.Api.Domain.Entities.Models.Loja;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.ConfiguracoesLoja;

public class ConfiguracaoRestauranteConfig : IEntityTypeConfiguration<ConfiguracaoLoja> {
    public void Configure(EntityTypeBuilder<ConfiguracaoLoja> builder) {
        builder.ToTable("ConfiguracaoLoja");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(1001, 1);

        builder.Property(x => x.Id).HasComment("Identificador único da configuração do restaurante.");
        builder.Property(x => x.LojaId).HasComment("Loja vinculada à configuração do restaurante.");
        builder.Property(x => x.TrabalhaComMesa).HasComment("Indica se o restaurante trabalha com controle de mesas.");
        builder.Property(x => x.TrabalhaComDelivery).HasComment("Indica se o restaurante trabalha com pedidos delivery.");
        builder.Property(x => x.TrabalhaComRetirada).HasComment("Indica se o restaurante trabalha com retirada no balcão.");
        builder.Property(x => x.PermiteVendaSemEstoque).HasComment("Indica se permite realizar venda de produtos sem estoque.");
        builder.Property(x => x.CobraTaxaServico).HasComment("Indica se cobra taxa de serviço.");
        builder.Property(x => x.PercentualTaxaServico).HasPrecision(5, 2).HasComment("Percentual aplicado para cobrança da taxa de serviço.");
        builder.Property(x => x.ExigirGarcomNaMesa).HasComment("Indica se é obrigatório informar garçom responsável pela mesa.");
        builder.Property(x => x.EnviarPedidoAutomaticamenteCozinha).HasComment("Indica se o pedido deve ser enviado automaticamente para a cozinha.");
        builder.Property(x => x.EnviarPedidoAutomaticamenteBar).HasComment("Indica se o pedido deve ser enviado automaticamente para o bar.");
        builder.Property(x => x.Ativo).HasComment("Indica se a configuração está ativa.");
        builder.Property(x => x.AbilitarKDS).HasComment("Indica se a configuração está ativa do KDS.");
        builder.Property(x => x.AbilitarImpressoraTermica).HasComment("Indica se a configuração está ativa da impressora termica.");

        builder.HasOne(x => x.Loja).WithOne(x => x.Configuracao).HasForeignKey<ConfiguracaoLoja>(x => x.LojaId).OnDelete(DeleteBehavior.Cascade);
    }
}