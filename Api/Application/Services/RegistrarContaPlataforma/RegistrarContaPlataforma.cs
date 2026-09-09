using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Persistence.Context;
using MenuFast.Api.Api.Util.Helpers;
using MenuFast.Api.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace MenuFast.Api.Api.Application.Services.RegistrarContaPlataforma {
    public class RegistrarContaPlataforma {
        private readonly MenuFastContext _menuFastContext;

        public RegistrarContaPlataforma(MenuFastContext menuFastContext) {
            _menuFastContext = menuFastContext;
        }

        public async Task RegistrarAsync(RegistrarContaPlataformaRequest request, int lojaId) {

            if(DocumentoHelper.ValidarCpf(request.Cpf) == false)
                throw new BusinessLogicException("CPF inválido.");

            if(_menuFastContext.Funcionarios.Any(f => f.Cpf == DocumentoHelper.RemoverCaracteresEspeciais(request.Cpf)))
                throw new BusinessLogicException("CPF já cadastrado.");

            var funcionario = new Domain.Entities.Models.Funcionario.Funcionario
            {
                Nome = request.Nome,
                Cpf = DocumentoHelper.RemoverCaracteresEspeciais(request.Cpf),
                Email = request.Email,
                Telefone = DocumentoHelper.RemoverCaracteresEspeciais(request.Telefone),
                SenhaHash = SegurancaHelper.CriarHash(request.SenhaHash),
                LojaId = lojaId,
                PerfilId = 1,
                Ativo = true,
                PrimeiroAcesso = true,
                DataAdmissao = DateTime.Now,
                DataCadastro = DateTime.Now
            };

            _menuFastContext.Funcionarios.Add(funcionario);

            await _menuFastContext.SaveChangesAsync();
        }

        public async Task CancelarAsync(int lojaId) {
            var funcionario = await _menuFastContext.Funcionarios
                .FirstOrDefaultAsync(f => f.LojaId == lojaId && f.PerfilId == 1);

            if(funcionario == null)
                throw new Exception("Conta da plataforma não encontrada.");

            _menuFastContext.Funcionarios.Remove(funcionario);

            await _menuFastContext.SaveChangesAsync();
        }
    }
}