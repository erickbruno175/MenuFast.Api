using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.Services.ContextUser;
using MenuFast.Api.Api.Application.Services.LojaConfiguracoes;
using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesLoja;
using MenuFast.Api.Api.Domain.Entities.Models.Loja;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace MenuFast.Api.Api.Controllers {
    [ApiController]
    [Route("api/configuracoes")]
    public class ConfiguracaoSistemaLojaController : ControllerBase {
        private readonly ConfiguracaoSistemaLoja _configuracaoSistemaLoja;
        private readonly UsuarioContextService _usuarioContextService;
        public ConfiguracaoSistemaLojaController(
            ConfiguracaoSistemaLoja configuracaoSistemaLoja , UsuarioContextService usuarioContextService) {
            _configuracaoSistemaLoja = configuracaoSistemaLoja;
            _usuarioContextService = usuarioContextService;
        }

        [HttpPost]
        [Route("dados-loja")]
        [Authorize]
        [ProducesResponseType(typeof(Loja), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CadastrarDadosLoja([FromBody] DadosEmpresaRequest request) 
            {var loja = await _configuracaoSistemaLoja.CadastrarDadosLoja(request);
            return Ok(loja);
        }

        [HttpPut]
        [Route("dados-loja/{idLoja}")]
        [Authorize]
        [ProducesResponseType(typeof(Loja), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AtualizarDadosLoja(int idLoja,[FromBody] DadosEmpresaRequest request) 
        {
            var loja = await _configuracaoSistemaLoja.AtualizarDadosLoja(idLoja, request);
            if(loja == null)return NotFound("Loja não encontrada.");
            return Ok(loja);
        }

        [HttpPost]
        [Route("{idLoja}/horarios")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<HorarioFuncionamento>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CadastrarHorarioFuncionamento(int idLoja,[FromBody] List<CadastrarHorarioFuncionamentoRequest> request) {
            var horarios = await _configuracaoSistemaLoja.CadastrarHorarioFuncionemnto(idLoja, request);
            return Ok(horarios);
        }

        [HttpPut]
        [Route("{idHorario}/horarios")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<HorarioFuncionamento>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AtualizarHorarioFuncionamento(int idHorario,[FromBody] List<CadastrarHorarioFuncionamentoRequest> request) {
            var horarios = await _configuracaoSistemaLoja.AtualizarHorarioFuncionemnto(idHorario, request);
            return Ok(horarios);
        }

        [HttpPost]
        [Route("{idLoja}/configuracoes-loja")]
        [Authorize]
        [ProducesResponseType(typeof(ConfiguracaoLoja), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CadastrarConfiguracaoLoja(int idLoja,[FromBody] CadastrarConfiguracaoLojaRequest request) {
            var configuracao = await _configuracaoSistemaLoja.CadastrarConfiguracaoLoja(idLoja, request);
            return Ok(configuracao);
        }

        [HttpPut]
        [Route("{idConfig}/configuracoes-loja")]
        [Authorize]
        [ProducesResponseType(typeof(ConfiguracaoLoja), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AtualizarConfiguracaoLoja(int idConfig,[FromBody] CadastrarConfiguracaoLojaRequest request) {
            var configuracao = await _configuracaoSistemaLoja.AtualizarConfiguracaoLoja(idConfig, request);

            if(configuracao == null)return NotFound("Configuração da loja não encontrada.");
            return Ok(configuracao);
        }

        [HttpGet]
        [Route("lembrar-finalizar")]
        [Authorize]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LembrarFinalizarCadastro() {
            var lembrar = await _configuracaoSistemaLoja.LembrarFinalizarCadastroConfiguracoesLoja(_usuarioContextService.FuncionarioId().Value);
            return Ok(lembrar);
        }
    }
}