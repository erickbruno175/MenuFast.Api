using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Application.Services.CategoriaServices;
using MenuFast.Api.Api.Application.Services.ContextUser;
using MenuFast.Api.Api.Domain.Entities.Models.Cardapio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MenuFast.Api.Api.Controllers {

    [ApiController]
    [Route("api/categoria")]
    public class CategoriaController : ControllerBase {

        private readonly CategoriaService _categoriaService;
        private readonly UsuarioContextService _usuarioContextService;
        public CategoriaController(CategoriaService categoriaService, UsuarioContextService usuarioContextService) {
            _categoriaService = categoriaService;
            _usuarioContextService = usuarioContextService;
        }

        [HttpPost]
        [Route("cadastrar")]
        [Authorize]
        [ProducesResponseType(typeof(CategoriaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CadastrarCategoria([FromBody] CategoriaRequest request) {

            var categoria = await _categoriaService.CadastrarCategoria(request);
            return Created("Categoria criada com sucesso", categoria);
        }
        [HttpPut]
        [Route("atualizar")]
        [Authorize]
        [ProducesResponseType(typeof(CategoriaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AtualizarCategoria([FromBody] CategoriaRequest request) {
            var categoria = await _categoriaService.AtualizarCategoria(request);

            return Created("Categoria atualozada com sucesso", categoria);
        }
        [HttpGet]
        [Route("listar")]
        [Authorize]
        [ProducesResponseType(typeof(CategoriaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> ListarCategorias() {
            return Ok(await _categoriaService.ListarCategorias(_usuarioContextService.LojaId()!.Value));
        }

        [HttpDelete("deletar/{id}")]
        [Authorize]
        public async Task<ActionResult> RemoverCategoria(int id) {
            await _categoriaService.RemoverCategoriaProduto(id);
            return Ok(new { message = $"Categoria '{id}' removida com sucesso." });
        }
    }
}
