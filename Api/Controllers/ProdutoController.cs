using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Application.Services.ContextApplication;
using MenuFast.Api.Api.Application.Services.ProdutoServices;
using MenuFast.Api.Api.Domain.Entities.Models.Cardapio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuFast.Api.Api.Controllers {
    [ApiController]
    [Route("api/produto")]
    public class ProdutoController : ControllerBase {
        private readonly ProdutoServices _produtoService;
        private readonly ApplicationContextService _applicationContextService;

        public ProdutoController(ProdutoServices produtoService, ApplicationContextService applicationContextService) {
            _produtoService = produtoService;
            _applicationContextService = applicationContextService;
        }

        [HttpPost]
        [Route("cadastrar")]
        [Authorize]
        [ProducesResponseType(typeof(DetalheProdutosResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CadastrarProduto([FromBody] ProdutoRequest request) {
            var produto = await _produtoService.CadastrarProduto(request);

            return Created("Produto criado com sucesso", produto);
        }

        [HttpPut]
        [Route("atualizar")]
        [Authorize]
        [ProducesResponseType(typeof(DetalheProdutosResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AtualizarProduto(int idProduto, [FromBody] ProdutoRequest request) {
            var produto = await _produtoService.AtualizarProduto(idProduto, request);

            return Created("Produto atualizado com sucesso", produto);
        }

        [HttpGet]
        [Route("listar")]
        [Authorize]
        [ProducesResponseType(typeof(List<DetalheProdutosResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> ListarProdutos(int idLoja) {
            return Ok(await _produtoService.ListaProdutosEmEstoque(idLoja));
        }

        [HttpPost]
        [Route("buscar")]
        [Authorize]
        [ProducesResponseType(typeof(List<DetalheProdutosResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> BuscarProdutos(int idLoja, [FromBody] FiltroProdutoRequest? filtro, string tipoFiltro) {
            return Ok(await _produtoService.BuscarProdutos(idLoja, filtro, tipoFiltro));
        }

        [HttpDelete("deletar/{id}")]
        [Authorize]
        public async Task<ActionResult> RemoverProduto(int id) {
            await _produtoService.RemoverProduto(id);

            return Ok(new
            {
                message = $"Produto '{id}' removido com sucesso."
            });
        }
        [HttpPost("cadastrar-em-massa")]
        public async Task<IActionResult> CadastrarProdutosEmMassa([FromBody] List<ProdutoRequest> requests) {
            var produtos = await _produtoService.CadastrarProdutosEmMassa(requests);
            return Ok(produtos);
        }

    }
}