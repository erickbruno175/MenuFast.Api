using MenuFast.Api.Api.Application.Services.CaixaServices;
using MenuFast.Api.Api.Application.Services.ContextApplication;
using MenuFast.Api.Api.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuFast.Api.Api.Controllers {
    [ApiController]
    [Route("api/caixa")]
    [Authorize]
    public class CaixaController : ControllerBase {
        private readonly CaixaService _caixaService;
        private readonly ApplicationContextService _contextApplication;

        public CaixaController(
            CaixaService caixaService,
            ApplicationContextService contextApplicationService) {

            _caixaService = caixaService;
            _contextApplication = contextApplicationService;
        }

        [HttpPost]
        [Route("abrir")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AbrirCaixa( string nome = "Caixa") {
            var lojaId = _contextApplication.LojaId()!.Value;
            var funcionarioId = _contextApplication.FuncionarioId()!.Value;
            var caixa = await _caixaService.AbrirCaixaAsync(lojaId, funcionarioId, nome);
            return Ok(new { mensagem = "Caixa aberto com sucesso.", caixa });
        }

        [HttpGet]
        [Route("aberto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BuscarCaixaAberto() {
            var lojaId = _contextApplication.LojaId()!.Value;
            var caixa = await _caixaService.BuscarCaixaAbertoAsync(lojaId);
            if(caixa == null) return NotFound(new { mensagem = "Não existe caixa aberto." });
            return Ok(caixa);
        }

        [HttpPost]
        [Route("movimento")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegistrarMovimento(int funcionarioId, TipoMovimentoCaixa tipo, decimal valor, string? descricao = null) {
            var lojaId = _contextApplication.LojaId()!.Value;
            var movimento = await _caixaService.RegistrarMovimentoAsync(lojaId, funcionarioId, tipo, valor, descricao);
            return Ok(new { mensagem = "Movimento registrado com sucesso.", movimento });
        }

        [HttpPost]
        [Route("sangria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Sangria(decimal valor, string? descricao = null) {
            var lojaId = _contextApplication.LojaId()!.Value;
            var funcionarioId = _contextApplication.FuncionarioId()!.Value;
            var movimento = await _caixaService.RegistrarSangriaAsync(lojaId, funcionarioId, valor, descricao);

            return Ok(new { mensagem = "Sangria registrada com sucesso.", movimento });
        }

        [HttpPost]
        [Route("suprimento")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Suprimento(decimal valor, string? descricao = null) {
            var lojaId = _contextApplication.LojaId()!.Value;
            var funcionarioId = _contextApplication.FuncionarioId()!.Value;
            var movimento = await _caixaService.RegistrarSuprimentoAsync(lojaId, funcionarioId, valor, descricao);
            return Ok(new { mensagem = "Suprimento registrado com sucesso.", movimento });
        }

        [HttpGet]
        [Route("movimentos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> BuscarMovimentos() {
            var lojaId = _contextApplication.LojaId()!.Value;
            var movimentos = await _caixaService.BuscarMovimentosAsync(lojaId);

            return Ok(movimentos);
        }

        [HttpGet]
        [Route("valor-atual")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CalcularValorAtual() {
            var lojaId = _contextApplication.LojaId()!.Value;
            var valor = await _caixaService.CalcularValorAtualAsync(lojaId);

            return Ok(new { valor });
        }

        [HttpPost]
        [Route("fechar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> FecharCaixa(decimal valorFechamento) {
            var lojaId = _contextApplication.LojaId()!.Value;
            var funcionarioId = _contextApplication.FuncionarioId()!.Value;
            var caixa = await _caixaService.FecharCaixaAsync(lojaId, funcionarioId, valorFechamento);

            return Ok(new { mensagem = "Caixa fechado com sucesso.", caixa });
        }
    }
}