    using global::MenuFast.Api.Api.Application.DTOs.Request;
    using global::MenuFast.Api.Api.Application.DTOs.Response;
    using global::MenuFast.Api.Api.Application.Services.ContextApplication;
    using global::MenuFast.Api.Api.Application.Services.SegurancaServices;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    namespace MenuFast.Api.Api.Controllers {

        [ApiController]
        [Route("api/seguranca/configuracoes")]
        public class ConfiguracoesSegurancaController : ControllerBase {

            private readonly ConfiguracoesSegurancaServices _service;
            private readonly ApplicationContextService _applicationContextService;

            public ConfiguracoesSegurancaController(
                ConfiguracoesSegurancaServices service,
                ApplicationContextService applicationContextService) {

                _service = service;
                _applicationContextService = applicationContextService;
            }

            [HttpGet]
            [Authorize]
            [ProducesResponseType(typeof(ConfiguracaoSegurancaResponse), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> Consultar() {

                var lojaId = _applicationContextService.LojaId();

                if(!lojaId.HasValue)
                    return Unauthorized();

                var resultado = await _service.ConsultarAsync(lojaId.Value);

                return Ok(resultado);
            }

            [HttpPut]
            [Authorize]
            [ProducesResponseType(typeof(ConfiguracaoSegurancaResponse), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> Atualizar([FromBody] ConfiguracaoSegurancaRequest request) {

                var lojaId = _applicationContextService.LojaId();

                if(!lojaId.HasValue)
                    return Unauthorized();

                if(request.MaxTentativasLogin <= 0)
                    return BadRequest(new { Mensagem = "O número máximo de tentativas de login deve ser maior que zero." });

                if(request.TempoBloqueioMinutos <= 0)
                    return BadRequest(new { Mensagem = "O tempo de bloqueio deve ser maior que zero." });

                if(request.TempoExpiracaoSessaoDias <= 0)
                    return BadRequest(new { Mensagem = "O tempo de expiração da sessão deve ser maior que zero." });

                var resultado = await _service.AtualizarAsync(
                    lojaId.Value,
                    request.MaxTentativasLogin,
                    request.TempoBloqueioMinutos,
                    request.TempoExpiracaoSessaoDias);

                return Ok(resultado);
            }
        }
    }
