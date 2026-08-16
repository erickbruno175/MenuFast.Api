using MenuFast.Api.Api.Domain.Entities.Models.Cardapio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProdutoConfig : IEntityTypeConfiguration<Produto> {
    public void Configure(EntityTypeBuilder<Produto> builder) {
        builder.ToTable("Produto");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(1001, 1);
        builder.Property(x => x.Id).HasComment("Identificador único do produto.");
        builder.Property(x => x.CategoriaProdutoId).HasComment("Categoria à qual o produto pertence.");
        builder.Property(x => x.Nome).IsRequired().HasMaxLength(100).HasComment("Nome do produto.");
        builder.Property(x => x.Preco).HasPrecision(18, 2).HasComment("Preço de venda do produto.");
        builder.Property(x => x.ControlaEstoque).HasComment("Indica se o produto controla estoque.");
        builder.Property(x => x.Ativo).HasComment("Indica se o produto está ativo.");
        builder.Property(x => x.Descricao).HasComment("Indica os igredientes ");
        builder.Property(x => x.FotoProduto).HasMaxLength(500).HasComment("Caminho ou URL da foto do produto.");
        builder.HasOne(x => x.CategoriaProduto).WithMany(x => x.Produtos).HasForeignKey(x => x.CategoriaProdutoId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Loja).WithMany().HasForeignKey(x => x.LojaId).OnDelete(DeleteBehavior.NoAction);
        builder.HasMany(x => x.Complementos).WithMany(x => x.Produtos);
    }
}