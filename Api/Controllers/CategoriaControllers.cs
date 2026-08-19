using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Application.Services.CategoriaServices;
using MenuFast.Api.Api.Domain.Entities.Models.Cardapio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MenuFast.Api.Api.Controllers {

    [ApiController]
    [Route("api/categoria")]
    public class CategoriaControllers : ControllerBase {

        private readonly CategoriaService _categoriaService;
        public CategoriaControllers(CategoriaService categoriaService) { _categoriaService = categoriaService; }

        [HttpPost]
        [Route("cadastrar")]
        [Authorize]
        [ProducesResponseType(typeof(CategoriaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CadastrarCategoria(CategoriaRequest request) {

            var categoria = await _categoriaService.CadastrarCategoria(request);
            return Created("Categoria criada com sucesso", categoria);
        }
        [HttpPut]
        [Route("atualizar")]
        [Authorize]
        [ProducesResponseType(typeof(CategoriaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AtualizarCategoria(CategoriaRequest request) {
            var categoria = await _categoriaService.AtualizarCategoria(request);

            return Created("Categoria atualozada com sucesso", categoria);
        }
        [HttpGet]
        [Route("listar")]
        [Authorize]
        [ProducesResponseType(typeof(CategoriaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> ListarCategorias(int idLoja) {
            return Ok(await _categoriaService.ListarCategorias(idLoja));
        }

        [HttpDelete("deletar/{id}")]
        [Authorize]
        public async Task<ActionResult> RemoverCategoria(int id) {
            await _categoriaService.RemoverCategoriaProduto(id);
            return Ok(new { message = $"Categoria '{id}' removida com sucesso." });
        }
    }
}
