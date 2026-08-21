using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Domain.Entities.Models.Cardapio;
using MenuFast.Api.Api.Persistence.Context;
using MenuFast.Api.Api.Util.Helpers;
using MenuFast.Api.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace MenuFast.Api.Api.Application.Services.ProdutoServices {
    public class ProdutoServices {
        private readonly MenuFastContext _menuFastContext;

        public ProdutoServices(MenuFastContext menuFastContext) {
            _menuFastContext = menuFastContext;
        }

    

        public async Task<DetalheProdutosResponse> CadastrarNovoProduto(
            ProdutoRequest request) {
            var produto = new Produto
            {
                ProdutoEsgotado = request.Ativo,
                CategoriaProdutoId = request.CategoriaProdutoId,
                LojaId = request.LojaId,
                FotoProduto = request.FotoProduto,
                Preco = request.Preco,
                Nome = request.Nome,
                DataCadastro = DateTime.UtcNow,
                ControlaEstoque = request.ControlaEstoque,
                Codigo = UtilHelper.GerarCodigoProduto(_menuFastContext),
                Descricao = request.Descricao,
                Ativo = request.Ativo
            };

            await _menuFastContext.Produtos.AddAsync(produto);

            await _menuFastContext.SaveChangesAsync();

            return ConverterParaDetalhe(produto);
        }

    

        public async Task<DetalheProdutosResponse> AtualizarProduto(int idProduto,ProdutoRequest request) {
            var produto = await _menuFastContext.Produtos.FirstOrDefaultAsync(x => x.Id == idProduto);

            if(produto == null)throw new BusinessLogicException("Produto não encontrado");

            produto.Nome = request.Nome;
            produto.FotoProduto = request.FotoProduto;
            produto.Preco = request.Preco;
            produto.CategoriaProdutoId = request.CategoriaProdutoId;
            produto.ControlaEstoque = request.ControlaEstoque;
            produto.Descricao = request.Descricao;
            produto.Ativo = request.Ativo;
            produto.ProdutoEsgotado = request.Ativo;

            await _menuFastContext.SaveChangesAsync();

            return ConverterParaDetalhe(produto);
        }

        public async Task<DetalheProdutosResponse> DetalharProduto(int idProduto) {
            var produto = await _menuFastContext.Produtos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == idProduto);

            if(produto == null)throw new BusinessLogicException("Produto não encontrado");
            return ConverterParaDetalhe(produto);
        }

        public async Task<List<DetalheProdutosResponse>> ListaProdutos(int idLoja) {
            return await _menuFastContext.Produtos.AsNoTracking()
                .Where(p =>
                    p.LojaId == idLoja &&
                    p.ProdutoEsgotado)
                .OrderBy(p => p.Nome)
                .Select(p => new DetalheProdutosResponse
                {
                    Id = p.Id,
                    FotoProduto = p.FotoProduto,
                    Nome = p.Nome,
                    Preco = MoedaHelper.FormatarReal(p.Preco),
                    Codigo = p.Codigo,
                    Descricao = p.Descricao,
                    Ativo = p.Ativo
                })
                .ToListAsync();
        }
       
        public async Task<List<DetalheProdutosResponse>> BuscarProdutos(int idLoja,FiltroProdutoRequest? filtro , string tipoFiltro) {
            var produtos = _menuFastContext.Produtos.AsNoTracking().Where(p =>p.LojaId == idLoja);

            if(filtro != null)
            {
                if(tipoFiltro == "NOME" &&!string.IsNullOrWhiteSpace(filtro.Nome))
                {
                    produtos = produtos.Where(p =>EF.Functions.Like(p.Nome,$"%{filtro.Nome}%"));
                }
                else if(tipoFiltro == "CATEGORIA" && filtro.CategoriaId != null)
                {
                    produtos = produtos.Where(p => p.CategoriaProdutoId == filtro.CategoriaId);
                }
                else if(tipoFiltro == "CODIGO" && !string.IsNullOrWhiteSpace(filtro.Codigo))
                {
                    produtos = produtos.Where(p =>p.Codigo == filtro.Codigo);
                }else
                {
                    produtos = produtos.Where(p => p.Ativo);
                }
            }

            return await produtos
                .OrderBy(p => p.Nome)
                .Select(p => new DetalheProdutosResponse
                {
                    Id = p.Id,
                    Codigo = p.Codigo,
                    FotoProduto = p.FotoProduto,
                    Nome = p.Nome,
                    Preco = MoedaHelper.FormatarReal(p.Preco),
                    Descricao = p.Descricao,
                    Ativo = p.Ativo
                })
                .ToListAsync();
        }
       
        public async Task RemoverProdutoCardapio(int idProduto) {
            var produto = await _menuFastContext.Produtos.FirstOrDefaultAsync(p => p.Id == idProduto);

            if(produto == null)throw new BusinessLogicException("Produto não encontrado");
            _menuFastContext.Produtos.Remove(produto);
            await _menuFastContext.SaveChangesAsync();
        }
       
        public async Task EsgotarProduto(IEnumerable<int> idsProduto,bool esgotado) {
            var produtos = await _menuFastContext.Produtos.Where(p => idsProduto.Contains(p.Id)).ToListAsync();

            foreach(var produto in produtos)
            {
                produto.ProdutoEsgotado = esgotado;
            }

            await _menuFastContext.SaveChangesAsync();
        }

        private static DetalheProdutosResponse ConverterParaDetalhe(Produto produto) {
            return new DetalheProdutosResponse
            {
                Id = produto.Id,
                Nome = produto.Nome,
                FotoProduto = produto.FotoProduto,
                Preco = MoedaHelper.FormatarReal(produto.Preco),
                Codigo = produto.Codigo,
                Descricao = produto.Descricao,
                Ativo = produto.Ativo
            };
        }
    }
}