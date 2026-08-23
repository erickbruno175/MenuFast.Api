using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.Services.Funcionario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuFast.Api.Api.Controllers {
    [ApiController]
    [Route("api/funcionario")]
    [Authorize]
    public class FuncionarioController : ControllerBase {
        private readonly FuncioanarioService _funcionarioService;

        public FuncionarioController(FuncioanarioService funcionarioService) {
            _funcionarioService = funcionarioService;
        }

        [HttpPost]
        [Route("cadastrar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CadastrarFuncionario([FromBody] CadastrarFuncionarioRequest request) {
            await _funcionarioService.CadastrarFuncionario(request);
            return Ok(new{mensagem = "Funcionário cadastrado com sucesso."});
        }

        [HttpPut]
        [Route("atualizar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditarFuncionario(int id,[FromBody] CadastrarFuncionarioRequest request) {
            await _funcionarioService.EditarFuncionario(id, request);
            return Ok(new{mensagem = "Funcionário atualizado com sucesso."});
        }

        [HttpGet]
        [Route("listar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ConsultarFuncionarios([FromQuery] int idLoja,[FromQuery] string? nome = null,[FromQuery] int? perfilId = null) {
            var funcionarios = await _funcionarioService.ConsultarFuncionarios(idLoja, nome, perfilId);
            return Ok(funcionarios);
        }
    }

}