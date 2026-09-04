using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Application.Services.Services.OpenRouteService;
using MenuFast.Api.Api.Domain.Entities.Models.Cliente;
using MenuFast.Api.Api.Persistence.Context;
using MenuFast.Api.Api.Util.Helpers;
using MenuFast.Api.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace MenuFast.Api.Api.Application.Services.ClienteServices {
    public class ClienteService {

        private readonly MenuFastContext _context;
        private readonly OpenRouteServices _openRouteServices;

        public ClienteService(
            MenuFastContext context,
            OpenRouteServices openRouteServices) {

            _context = context;
            _openRouteServices = openRouteServices;
        }
        public async Task<ClienteResponse> CadastrarAsync(
            ClienteRequest request,
            int lojaId) {

            if(!DocumentoHelper.ValidarCpf(request.CPF))
            {
                throw new BusinessLogicException("CPF inválido.");
            }

            var coordenadas = await _openRouteServices.BuscarCoordenadasAsync(
                request.CEP,
                request.Logradouro,
                request.Numero,
                request.Bairro,
                request.Cidade,
                request.Estado);

            var cliente = new Cliente
            {
                LojaId = lojaId,
                Nome = request.Nome,
                CPF = DocumentoHelper.RemoverCaracteresEspeciais(request.CPF),
                DataNascimento = request.DataNascimento,
                Telefone = DocumentoHelper.RemoverMascaraTelefone(request.Telefone),
                WhatsApp = DocumentoHelper.RemoverMascaraTelefone(request.WhatsApp),
                Email = request.Email,
                CEP = request.CEP,
                Logradouro = request.Logradouro,
                Numero = request.Numero,
                Complemento = request.Complemento,
                Bairro = request.Bairro,
                Cidade = request.Cidade,
                Estado = request.Estado,
                PontoReferencia = request.PontoReferencia,
                Observacao = request.Observacao,
                Latitude = coordenadas?.Latitude ?? 0m,         
                Longitude = coordenadas?.Longitude ?? 0m,
                Ativo = true,
                DataCadastro = DateTime.UtcNow
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return MapearResponse(cliente);
        }
        public async Task<List<ClienteResponse>> ListarAsync(int lojaId) {

            var clientes = await _context.Clientes
                .Where(x => x.LojaId == lojaId)
                .OrderBy(x => x.Nome)
                .ToListAsync();

            return clientes
                .Select(MapearResponse)
                .ToList();
        }
        public async Task<ClienteResponse?> BuscarPorIdAsync(int id,int lojaId) {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(x =>x.Id == id &&x.LojaId == lojaId);
            if(cliente == null)return null;
            return MapearResponse(cliente);
        }
        public async Task<ClienteResponse?> AtualizarAsync(int id,ClienteRequest request,int lojaId) {

            if(!DocumentoHelper.ValidarCpf(request.CPF))
            {
                throw new BusinessLogicException("CPF inválido.");
            }

            var cliente = await _context.Clientes.FirstOrDefaultAsync(x =>x.Id == id && x.LojaId == lojaId);

            if(cliente == null)return null;

            var coordenadas = await _openRouteServices.BuscarCoordenadasAsync(
                request.CEP,
                request.Logradouro,
                request.Numero,
                request.Bairro,
                request.Cidade,
                request.Estado);

            cliente.Nome = request.Nome;
            cliente.CPF = DocumentoHelper.RemoverCaracteresEspeciais(request.CPF);
            cliente.DataNascimento = request.DataNascimento;
            cliente.Telefone = DocumentoHelper.RemoverMascaraTelefone(request.Telefone);
            cliente.WhatsApp = DocumentoHelper.RemoverMascaraTelefone(request.WhatsApp);
            cliente.Email = request.Email;
            cliente.CEP = request.CEP;
            cliente.Logradouro = request.Logradouro;
            cliente.Numero = request.Numero;
            cliente.Complemento = request.Complemento;
            cliente.Bairro = request.Bairro;
            cliente.Cidade = request.Cidade;
            cliente.Estado = request.Estado;
            cliente.PontoReferencia = request.PontoReferencia;
            cliente.Observacao = request.Observacao;

            cliente.Latitude = coordenadas?.Latitude ?? 0m;
            cliente.Longitude = coordenadas?.Longitude ?? 0m;
            await _context.SaveChangesAsync();

            return MapearResponse(cliente);
        }

        public async Task<bool> AlterarStatusAsync(
            int id,
            int lojaId) {

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.LojaId == lojaId);

            if(cliente == null)
                return false;

            cliente.Ativo = !cliente.Ativo;

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<List<ClienteResponse>> PesquisarAsync(string pesquisa,int lojaId) {

            var clientes = await _context.Clientes.Where(x =>x.LojaId == lojaId &&
                    (
                        x.Nome.Contains(pesquisa) ||
                        x.CPF.Contains(pesquisa) ||
                        x.Telefone.Contains(pesquisa) ||
                        x.WhatsApp.Contains(pesquisa)
                    ))
                .OrderBy(x => x.Nome)
                .ToListAsync();

            return clientes
                .Select(MapearResponse)
                .ToList();
        }
        public async Task<bool> ExcluirAsync(int id,int lojaId) {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(x =>x.Id == id &&x.LojaId == lojaId);

            if(cliente == null)
                return false;

            _context.Clientes.Remove(cliente);

            await _context.SaveChangesAsync();

            return true;
        }

        private static ClienteResponse MapearResponse(Cliente cliente) {

            return new ClienteResponse
            {
                Id = cliente.Id,
                LojaId = cliente.LojaId,
                Nome = cliente.Nome,
                CPF = cliente.CPF,
                DataNascimento = cliente.DataNascimento,
                Telefone = cliente.Telefone,
                WhatsApp = cliente.WhatsApp,
                Email = cliente.Email,
                CEP = cliente.CEP,
                Logradouro = cliente.Logradouro,
                Numero = cliente.Numero,
                Complemento = cliente.Complemento,
                Bairro = cliente.Bairro,
                Cidade = cliente.Cidade,
                Estado = cliente.Estado,
                PontoReferencia = cliente.PontoReferencia,
                Observacao = cliente.Observacao,
                Ativo = cliente.Ativo,
                DataCadastro = cliente.DataCadastro,
                Latitude = cliente.Latitude.Value,
                Longitude = cliente.Longitude.Value
            };
        }
    }
}