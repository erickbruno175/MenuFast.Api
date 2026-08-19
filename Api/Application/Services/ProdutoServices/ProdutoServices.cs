using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Application.Services.ContextUser;
using MenuFast.Api.Api.Domain.Entities.Models.Cardapio;
using MenuFast.Api.Api.Persistence.Context;
using MenuFast.Api.Api.Util.Helpers;
using MenuFast.Api.Middlewares;
using Microsoft.EntityFrameworkCore;
using System.CodeDom;
using System.Collections;
using System.Globalization;

namespace MenuFast.Api.Api.Application.Services.ProdutoServices {
    public class ProdutoServices {

        private readonly MenuFastContext _menuFastContext;

        public ProdutoServices(MenuFastContext menuFastContext) { this._menuFastContext = menuFastContext; }


        public async Task<Produto> CadastrarNovoProduto(ProdutoRequest request) {

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
                Ativo = request.Ativo,

            };
            _menuFastContext.AddAsync(produto);
            return produto;
        }

        public async Task<Produto> AtualizarProduto(int idProdudto, ProdutoRequest produtoRequest) {

            var produtoParaAtualizar = await _menuFastContext.Produtos.FirstOrDefaultAsync(x => x.Id == idProdudto);
            if(produtoParaAtualizar == null) throw new BusinessLogicException("Produto não encontrato");

            produtoParaAtualizar.ProdutoEsgotado = produtoRequest.Ativo;
            produtoParaAtualizar.FotoProduto = produtoRequest.FotoProduto;
            produtoParaAtualizar.CategoriaProdutoId = produtoRequest.CategoriaProdutoId;
            produtoParaAtualizar.ControlaEstoque = produtoRequest.ControlaEstoque;
            produtoRequest.Preco = produtoRequest.Preco;
            produtoParaAtualizar.Codigo = produtoParaAtualizar.Codigo;
            produtoParaAtualizar.Descricao = produtoRequest.Descricao;
            _menuFastContext.AddAsync(produtoParaAtualizar);
            return produtoParaAtualizar;

        }

        public async Task<List<DetalheProdutos>> ListaProdutos(int idLojas) {
            var consultaProdutos = _menuFastContext.Produtos.AsNoTracking().ToList();
            var produtos = consultaProdutos.Where(p => p.LojaId == idLojas && p.ProdutoEsgotado).OrderBy(p => p.Nome)
                  .Select(p => new DetalheProdutos
                  {
                      FotoProduto = p.FotoProduto,
                      Id = p.Id,
                      Nome = p.Nome,
                      Preco = MoedaHelper.FormatarReal(p.Preco),
                      Codigo = p.Codigo,
                      Descricao = p.Descricao,
                      Ativo = p.Ativo,
                  })
                 .ToList();
            return produtos;
        }
        public async Task<List<DetalheProdutos>> BuscarProdutos(int idLojas, FiltroProdutoRequest? filtro) {
            var produtos = _menuFastContext.Produtos.AsNoTracking().
                Include(c => c.CategoriaProduto).
                Where(p => p.LojaId == idLojas && p.ProdutoEsgotado);

            if(filtro.TipoFiltro.PorNome == "NOME" && !string.IsNullOrWhiteSpace(filtro.Nome))
            {
                produtos = produtos.Where(p => EF.Functions.Like(p.Nome, $"%{filtro.Nome}%"));
            }
            else if(filtro.TipoFiltro.PorCategoriaId == "CATEGORIA" && filtro.CategoriaId != null)
            {
                produtos = produtos.Where(p => p.CategoriaProdutoId == filtro.CategoriaId);
            }
            else if(filtro.TipoFiltro.TipoCodigo == "CODIGO" && !string.IsNullOrEmpty(filtro.Codigo))
            {
                produtos = produtos.Where(p => p.Codigo == filtro.Codigo);
            }
            produtos = (IQueryable<Produto>)produtos.ToListAsync();

            return await produtos
            .Select(p => new DetalheProdutos
            {
                Codigo = p.Codigo,
                FotoProduto = p.FotoProduto,
                Nome = p.Nome,
                Preco = MoedaHelper.FormatarReal(p.Preco),
                Descricao = p.Descricao,
                Ativo = p.Ativo,

            })
            .ToListAsync();
        }
        public async Task RemoverProdutoCardapio(int idProduto) {
            var produto = _menuFastContext.Produtos.FirstOrDefault(p => p.Id == idProduto); _menuFastContext.Produtos.Remove(produto);
        }
        public async Task EsgotarProduto(IEnumerable<int> idsProduto , bool esgotado ) {
            var produtos = await _menuFastContext.Produtos.Where(p => idsProduto.Contains(p.Id)).ToListAsync();
            foreach(var produto in produtos)
            {
                produto.ProdutoEsgotado = esgotado;
            }
            await _menuFastContext.SaveChangesAsync();
        }


    }
}
