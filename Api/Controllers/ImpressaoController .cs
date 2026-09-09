using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Application.Services.ContextApplication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuFast.Api.Api.Controllers {
    [ApiController]
    [Route("api/impressao")]
    public class ImpressaoController : ControllerBase {
        private readonly ConfiguracaoImpressaoService _configuracaoImpressao;
        private readonly ApplicationContextService _applicationContextService;

        public ImpressaoController(ConfiguracaoImpressaoService configuracaoImpressao, ApplicationContextService applicationContextService) {
            _configuracaoImpressao = configuracaoImpressao;
            _applicationContextService = applicationContextService;
        }

        [HttpGet]
        [Route("configuracoes")]
        [Authorize]
        [ProducesResponseType(typeof(ConfiguracaoImpressaoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ConsultarConfiguracoes() {
            if(!_applicationContextService.LojaId().HasValue)
                return Unauthorized("Funcionario não identificado");

            var configuracao = await _configuracaoImpressao.ObterAsync(_applicationContextService.LojaId()!.Value);

            return Ok(configuracao);
        }

        [HttpPut]
        [Route("configuracoes")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AtualizarConfiguracoes([FromBody] AtualizarConfiguracaoImpressaoRequest request) {
            if(!_applicationContextService.LojaId().HasValue)
                return Unauthorized("Funcionario não identificado");

            await _configuracaoImpressao.AtualizarAsync(_applicationContextService.LojaId()!.Value, request);

            return NoContent();
        }
    }
}