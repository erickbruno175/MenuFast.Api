using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Application.Services.ContextApplication;
using MenuFast.Api.Api.Application.Services.LojaConfiguracoes;
using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesLoja;
using MenuFast.Api.Api.Domain.Entities.Models.Loja;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuFast.Api.Api.Controllers {
    [ApiController]
    [Route("api/configuracoes")]
    public class ConfiguracaoSistemaLojaController : ControllerBase {
        private readonly ConfiguracaoSistemaLojaServices _configuracaoSistemaLoja;
        private readonly ApplicationContextService _applicationContextService;

        public ConfiguracaoSistemaLojaController(ConfiguracaoSistemaLojaServices configuracaoSistemaLoja, ApplicationContextService applicationContextService) {
            _configuracaoSistemaLoja = configuracaoSistemaLoja;
            _applicationContextService = applicationContextService;
        }

        [HttpPost]
        [Route("dados-loja")]
        [ProducesResponseType(typeof(Loja), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CadastrarDadosLoja([FromBody] DadosEmpresaRequest request) {
            var loja = await _configuracaoSistemaLoja.CadastrarDadosLoja(request);
            return Ok(loja);
        }

        [HttpPut]
        [Route("dados-loja")]
        [Authorize]
        [ProducesResponseType(typeof(Loja), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AtualizarDadosLoja([FromBody] DadosEmpresaRequest request) {
            var lojaId = _applicationContextService.LojaId();

            if(!lojaId.HasValue)
                return Unauthorized("Funcionario não identificado.");

            var loja = await _configuracaoSistemaLoja.AtualizarDadosLoja(lojaId.Value, request);

            if(loja == null)
                return NotFound("Loja não encontrada.");

            return Ok(loja);
        }
        [HttpPut]
        [Route("horarios")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<HorarioFuncionamento>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SalvarHorarioFuncionamento([FromBody] List<CadastrarHorarioFuncionamentoRequest> request) {
            var lojaId = _applicationContextService.LojaId();

            if(!lojaId.HasValue)
                return Unauthorized("Funcionario não identificado.");

            var horarios = await _configuracaoSistemaLoja.SalvarHorarioFuncionamento(lojaId.Value, request);

            return Ok(horarios);
        }

        [HttpPost]
        [Route("configuracoes-loja")]
        [Authorize]
        [ProducesResponseType(typeof(ConfiguracaoLoja), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CadastrarConfiguracaoLoja([FromBody] CadastrarConfiguracaoLojaRequest request) {
            var lojaId = _applicationContextService.LojaId();

            if(!lojaId.HasValue)
                return Unauthorized("Funcionario não identificado.");

            var configuracao = await _configuracaoSistemaLoja.CadastrarConfiguracaoLoja(lojaId.Value, request);

            return Ok(configuracao);
        }

        [HttpPut]
        [Route("{idConfig}/configuracoes-loja")]
        [Authorize]
        [ProducesResponseType(typeof(ConfiguracaoLoja), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AtualizarConfiguracaoLoja(int idConfig, [FromBody] CadastrarConfiguracaoLojaRequest request) {
            var configuracao = await _configuracaoSistemaLoja.AtualizarConfiguracaoLoja(idConfig, request);

            if(configuracao == null)
                return NotFound("Configuração da loja não encontrada.");

            return Ok(configuracao);
        }

        [HttpGet]
        [Route("lembrar-finalizacao-configuracao")]
        [Authorize]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LembrarFinalizarCadastro() {
            var funcionarioId = _applicationContextService.FuncionarioId();

            if(!funcionarioId.HasValue)
                return Unauthorized("Funcionario não identificado");

            var lembrar = await _configuracaoSistemaLoja.LembrarFinalizarCadastroConfiguracoesLoja(funcionarioId!.Value);

            return Ok(lembrar);
        }

        [HttpGet]
        [Route("consultar-configuracoes-loja")]
        [Authorize]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ConfiguracoesLojaResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConsultarConfiguracoesLoja() {
            if(!_applicationContextService.LojaId().HasValue)
                return Unauthorized("Funcionario não identificado");

            var configuracoesLoja = await _configuracaoSistemaLoja.ConsultarConfiguracoesLoja(_applicationContextService.LojaId()!.Value);

            return Ok(configuracoesLoja);
        }

        [HttpGet]
        [Route("consultar-formas-pagamento")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<Application.DTOs.Response.FormaPagamentoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ConsultarFormasPagamento() {
            var formasPagamento = await _configuracaoSistemaLoja.ConsultarFormasPagamento();

            return Ok(formasPagamento);
        }

        [HttpGet]
        [Route("consultar-dados-loja")]
        [Authorize]
        [ProducesResponseType(typeof(DadosLojaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ConsultarDadosLoja() {
            var funcionarioId = _applicationContextService.FuncionarioId();
            if(!funcionarioId.HasValue) return Unauthorized("Funcionário não identificado.");
            var dadosLoja = await _configuracaoSistemaLoja.ConsultarDadosLoja(funcionarioId.Value);
            return Ok(dadosLoja);
        }
    }
}