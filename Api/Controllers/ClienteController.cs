using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.Services.ClienteServices;
using MenuFast.Api.Api.Application.Services.ContextUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuFast.Api.Api.Controllers {
    [ApiController]
    [Route("api/cliente")]
    [Authorize]
    public class ClienteController : ControllerBase {
        private readonly ClienteService _clienteService;
        private readonly UsuarioContextService _usuarioContext;

        public ClienteController(ClienteService clienteService , UsuarioContextService usuarioContextService) {
            _clienteService = clienteService;
            _usuarioContext = usuarioContextService;
        }

        [HttpPost]
        [Route("cadastrar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CadastrarCliente([FromBody] ClienteRequest request) {
            var cliente = await _clienteService.CadastrarAsync(request,_usuarioContext.LojaId().Value);
            return Ok(new{mensagem = "Cliente cadastrado com sucesso.",cliente});
        }
        [HttpPut]
        [Route("atualizar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtualizarCliente(int id,[FromBody] ClienteRequest request) {
            var cliente = await _clienteService.AtualizarAsync(id,request, _usuarioContext.LojaId().Value);
            if(cliente == null)return NotFound(new{mensagem = "Cliente não encontrado."});
            return Ok(new{mensagem = "Cliente atualizado com sucesso.",cliente});
        }

        [HttpGet]
        [Route("listar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ConsultarClientes() {
            var clientes = await _clienteService.ListarAsync(_usuarioContext.LojaId().Value);
            return Ok(clientes);
        }
        [HttpGet]
        [Route("buscar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BuscarCliente(int id) {
            var cliente = await _clienteService.BuscarPorIdAsync(id, _usuarioContext.LojaId().Value);
            if(cliente == null)return NotFound(new{mensagem = "Cliente não encontrado."});
            return Ok(cliente);
        }

        [HttpGet]
        [Route("pesquisar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PesquisarClientes([FromQuery] string pesquisa) {
            var clientes = await _clienteService.PesquisarAsync(pesquisa, _usuarioContext.LojaId().Value);
            return Ok(clientes);
        }


        [HttpPatch]
        [Route("alterar-status/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AlterarStatusCliente(int id) {
            var alterado = await _clienteService.AlterarStatusAsync(id, _usuarioContext.LojaId().Value);
            if(!alterado)return NotFound(new{mensagem = "Cliente não encontrado."});

            return Ok(new{mensagem = "Status do cliente alterado com sucesso."});
        }

        [HttpDelete]
        [Route("excluir/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ExcluirCliente(int id) {
            var excluido = await _clienteService.ExcluirAsync(id, _usuarioContext.LojaId().Value);
            if(!excluido)return NotFound(new{mensagem = "Cliente não encontrado."});
            return Ok(new{mensagem = "Cliente excluído com sucesso."});
        }
    }
}