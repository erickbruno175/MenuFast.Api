using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracaoImpressao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.ConfiguracaoImpressaoConfig;

public class ConfiguracaoImpressaoConfig : IEntityTypeConfiguration<ConfiguracaoImpressao> {
    public void Configure(EntityTypeBuilder<ConfiguracaoImpressao> builder) {
        builder.ToTable("ConfiguracaoImpressao");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(1001, 1);
        builder.Property(x => x.Id).HasComment("Identificador único da configuração de impressão.");
        builder.Property(x => x.LojaId).IsRequired().HasComment("Identificador da loja.");
        builder.Property(x => x.ImprimirPedidoAoEnviar).IsRequired().HasComment("Define se o pedido deve ser impresso automaticamente ao ser enviado.");
        builder.Property(x => x.ImprimirPedidoCozinha).IsRequired().HasComment("Define se o pedido deve ser impresso na cozinha.");
        builder.Property(x => x.ImprimirPedidoBar).IsRequired().HasComment("Define se o pedido deve ser impresso no bar.");
        builder.Property(x => x.ImpressoraPadrao).HasMaxLength(200).HasComment("Nome da impressora padrão.");
        builder.Property(x => x.LarguraPapel).IsRequired().HasComment("Largura do papel da impressora em milímetros.");
        builder.Property(x => x.MostrarNomeLoja).IsRequired().HasComment("Define se o nome da loja será exibido no comprovante.");
        builder.Property(x => x.MostrarCnpj).IsRequired().HasComment("Define se o CNPJ da loja será exibido no comprovante.");
        builder.Property(x => x.MostrarEndereco).IsRequired().HasComment("Define se o endereço da loja será exibido no comprovante.");
        builder.Property(x => x.MostrarTelefone).IsRequired().HasComment("Define se o telefone da loja será exibido no comprovante.");
        builder.Property(x => x.MensagemRodape).HasMaxLength(500).HasComment("Mensagem exibida no rodapé do comprovante.");
        builder.HasOne(x => x.Loja).WithMany().HasForeignKey(x => x.LojaId).OnDelete(DeleteBehavior.NoAction);
    }
}