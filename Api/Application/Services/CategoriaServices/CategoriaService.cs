using DocumentFormat.OpenXml.Office2010.Excel;
using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Domain.Entities.Models.Cardapio;
using MenuFast.Api.Api.Domain.Entities.Models.Loja;
using MenuFast.Api.Api.Persistence.Context;
using MenuFast.Api.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace MenuFast.Api.Api.Application.Services.CategoriaServices {
    public class CategoriaService {

        private readonly MenuFastContext _menuFastContext;

        public CategoriaService(MenuFastContext menuFastContext) { _menuFastContext = menuFastContext; }

        public async Task<CategoriaResponse> CadastrarCategoria(CategoriaRequest request) {
            if(await _menuFastContext.CategoriasProdutos.AnyAsync(c => c.Nome.Trim().ToUpper() == request.Nome && c.LojaId == request.LojaId))
{
                throw new BusinessLogicException($"Categoria {request.Nome} já existe");
            }
            var categoria = new CategoriaProduto{Nome = request.Nome,LojaId = request.LojaId};
            await _menuFastContext.CategoriasProdutos.AddAsync(categoria);
            await _menuFastContext.SaveChangesAsync();
            return new CategoriaResponse{Id = categoria.Id,Nome = categoria.Nome,};
        }
        public async Task<CategoriaResponse> AtualizarCategoria(CategoriaRequest request) {
            var categoria = await _menuFastContext.CategoriasProdutos.FirstOrDefaultAsync(c => c.Id == request.CategoriaId);

            if(categoria == null) { throw new BusinessLogicException("Categoria não encontrada"); }
            var categoriaExiste = await _menuFastContext.CategoriasProdutos.AnyAsync(c => c.Id != request.CategoriaId && c.Nome == request.Nome && c.LojaId == request.LojaId);

            if(categoriaExiste){throw new BusinessLogicException($"Categoria {request.Nome} já existe");}
            categoria.Nome = request.Nome;
            categoria.LojaId = request.LojaId;
            await _menuFastContext.SaveChangesAsync();

            return new CategoriaResponse{Id = categoria.Id,Nome = request.Nome,};
        }
        public async Task<List<CategoriaResponse>> ListarCategorias(int idLoja) {
            var categorias = await _menuFastContext.CategoriasProdutos
                .Where(cp => cp.LojaId == idLoja)
                .Select(cp => new CategoriaResponse{Id = cp.Id,Nome = cp.Nome,}).ToListAsync();

                return categorias;
        }

        public async Task RemoverCategoriaProduto(int id) {
            var categoriaProduto = await _menuFastContext.CategoriasProdutos.FirstOrDefaultAsync(x => x.Id == id);
            if(categoriaProduto == null)throw new Exception($"CategoriaProduto com ID {id} não encontrada.");
            _menuFastContext.CategoriasProdutos.Remove(categoriaProduto);

            var resultado = await _menuFastContext.SaveChangesAsync();
            if(resultado == 0)throw new Exception("Nenhum registro foi removido.");
        }
    }
}
