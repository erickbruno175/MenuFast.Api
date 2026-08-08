using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Domain.Entities.Models.Funcionario;
using MenuFast.Api.Api.Domain.Entities.Models.Seguranca;
using MenuFast.Api.Api.Persistence.Context;
using MenuFast.Api.Api.Util.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace MenuFast.Api.Api.Application.Services.Funcionario {
    public class FuncioanarioService {
        private readonly MenuFastContext _menuFastContext;

        public FuncioanarioService(MenuFastContext menuFastContext) {
            _menuFastContext = menuFastContext;
        }


        public async Task<List<Perfil>> GetPerfisAsync() {
            return await _menuFastContext.Perfis.ToListAsync();
        }
        public async Task<List<Permissao>> GetPermissoesAsync() {
            return await _menuFastContext.Permissoes.ToListAsync();

        }
        public async Task<List<PerfilPermissao>> GetPerfilPermissoesAsync() {
            return await _menuFastContext.PerfilPermissoes.ToListAsync();
        }
   
        public async Task CadastrarFuncionario(CadastrarFuncionarioRequest request) {
            var perfil = await _menuFastContext.Perfis.FirstOrDefaultAsync(p => p.Id == request.PerfilId);

           
            var funcionario = new Domain.Entities.Models.Funcionario.Funcionario
            {
                Nome = request.Nome,
                Email = request.Email,
                SenhaHash = request.SenhaHash,
                DataCadastro = DateTime.Now,
                Ativo = request.Ativo,
                DataAdmissao = DateTime.Now,
                DataBloqueio = null,
                DataUltimoLogin = null,
                FuncaoId = request.FuncaoId,
                PrimeiroAcesso = true,
                LojaId = request.LojaId,
                Salario = request.Salario ?? null,
                Login = request.Login,
                Cpf = DocumentoHelper.RemoverCaracteresEspeciais(request.Cpf),
                DataExpiracaoSenha = _menuFastContext.ConfiguracoesSeguranca.FirstOrDefault()?.TempoExpiracaoSessaoDias != null ? DateTime.Now.AddDays(_menuFastContext.ConfiguracoesSeguranca.FirstOrDefault().TempoExpiracaoSessaoDias) : (DateTime?)null,
            };
            _menuFastContext.Funcionarios.Add(funcionario);
            await _menuFastContext.SaveChangesAsync();
        }

        public async Task EditarFuncionario(int id, CadastrarFuncionarioRequest request) {
            var funcionario = await _menuFastContext.Funcionarios
                .FirstOrDefaultAsync(f => f.Id == id);

            if(funcionario == null)
            {
                throw new Exception("Funcionário não encontrado.");
            }
          
            funcionario.Nome = request.Nome;
            funcionario.Email = request.Email;
            funcionario.FuncaoId = request.FuncaoId;
            funcionario.Ativo = request.Ativo;
            funcionario.Bloqueado = request.Bloqueado;
            funcionario.LojaId = request.LojaId;
            funcionario.Salario = request.Salario;
            funcionario.Login = request.Login;
            funcionario.Telefone = DocumentoHelper.RemoverCaracteresEspeciais(request.Telefone);
            funcionario.Cpf = DocumentoHelper.RemoverCaracteresEspeciais(request.Cpf);
            funcionario.DataExpiracaoSenha = _menuFastContext.ConfiguracoesSeguranca.FirstOrDefault()?.TempoExpiracaoSessaoDias != null ? DateTime.Now.AddDays(_menuFastContext.ConfiguracoesSeguranca.FirstOrDefault().TempoExpiracaoSessaoDias) : (DateTime?)null;

            await _menuFastContext.SaveChangesAsync();
        }
        public async Task<List<Domain.Entities.Models.Funcionario.Funcionario>> GetFuncionariosAsync() {
            return await _menuFastContext.Funcionarios
                 .Include(p => p.Perfil)
                 .ToListAsync();
        }
    }
}
