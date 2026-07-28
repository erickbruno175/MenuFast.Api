using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Domain.Entities.Models.Empresa;
using MenuFast.Api.Api.Persistence.Context;
using MenuFast.Api.Api.Util.Helpers;

namespace MenuFast.Api.Api.Application.Services.EmpresaConfiguracoes {
    public class EmpresaConfiguracoesService {
        private readonly MenuFastContext _menuFastContext;

        public EmpresaConfiguracoesService(MenuFastContext menuFastContext) {
            _menuFastContext = menuFastContext;
        }


        public async Task<Loja> CadastrarDadosEmpresa(RequestDadosEmpresa requestDadosEmpresa) {
            var empresa = new Loja
            {
                Ativo = true,
                DataCadastro = DateTime.Now,
                Slug = SlugHelper.GerarSlug(requestDadosEmpresa.NomeFantasia),
                RazaoSocial = requestDadosEmpresa.RazaoSocial,
                NomeFantasia = requestDadosEmpresa.NomeFantasia,
                Cnpj = DocumentoHelper.RemoverCaracteresEspeciais(requestDadosEmpresa.Cnpj),
                InscricaoEstadual = requestDadosEmpresa.InscricaoEstadual,
                Telefone = DocumentoHelper.RemoverMascaraTelefone(requestDadosEmpresa.Telefone),
                Email = requestDadosEmpresa.Email,
                Cep = requestDadosEmpresa.Cep,
                Logradouro = requestDadosEmpresa.Logradouro,
                Numero = requestDadosEmpresa.Numero,
                Bairro = requestDadosEmpresa.Bairro,
                Cidade = requestDadosEmpresa.Cidade,
                Estado = requestDadosEmpresa.Estado,
                Uf = requestDadosEmpresa.Uf,
                Complemento = requestDadosEmpresa.Complemento,
                Sigla = requestDadosEmpresa.Sigla,
                Facebook = requestDadosEmpresa.Facebook,
                Instagram = requestDadosEmpresa.Instagram,
                WhatsApp = requestDadosEmpresa.WhatsApp,
                TikTok = requestDadosEmpresa.TikTok,
                YouTube = requestDadosEmpresa.YouTube,
                LinkedIn = requestDadosEmpresa.LinkedIn,
                Site = requestDadosEmpresa.Site,
                Logo = requestDadosEmpresa.Logo
            };

            await _menuFastContext.Lojas.AddAsync(empresa);
            await _menuFastContext.SaveChangesAsync();

            return empresa;
        }

        public async Task<Loja?> AtualizarDadosEmpresa(int idLoja, RequestDadosEmpresa requestDadosEmpresa) {
            var lojaParaEdicao = await _menuFastContext.Lojas.FindAsync(idLoja);

            if(lojaParaEdicao == null)
                return null;

            lojaParaEdicao.Slug = SlugHelper.GerarSlug(requestDadosEmpresa.NomeFantasia);
            lojaParaEdicao.RazaoSocial = requestDadosEmpresa.RazaoSocial;
            lojaParaEdicao.NomeFantasia = requestDadosEmpresa.NomeFantasia;
            lojaParaEdicao.Cnpj = DocumentoHelper.RemoverCaracteresEspeciais(requestDadosEmpresa.Cnpj);
            lojaParaEdicao.InscricaoEstadual = requestDadosEmpresa.InscricaoEstadual;
            lojaParaEdicao.Telefone = DocumentoHelper.RemoverMascaraTelefone(requestDadosEmpresa.Telefone);
            lojaParaEdicao.Email = requestDadosEmpresa.Email;
            lojaParaEdicao.Cep = requestDadosEmpresa.Cep;
            lojaParaEdicao.Logradouro = requestDadosEmpresa.Logradouro;
            lojaParaEdicao.Numero = requestDadosEmpresa.Numero;
            lojaParaEdicao.Bairro = requestDadosEmpresa.Bairro;
            lojaParaEdicao.Cidade = requestDadosEmpresa.Cidade;
            lojaParaEdicao.Estado = requestDadosEmpresa.Estado;
            lojaParaEdicao.Uf = requestDadosEmpresa.Uf;
            lojaParaEdicao.Complemento = requestDadosEmpresa.Complemento;
            lojaParaEdicao.Sigla = requestDadosEmpresa.Sigla;
            lojaParaEdicao.Facebook = requestDadosEmpresa.Facebook;
            lojaParaEdicao.Instagram = requestDadosEmpresa.Instagram;
            lojaParaEdicao.WhatsApp = requestDadosEmpresa.WhatsApp;
            lojaParaEdicao.TikTok = requestDadosEmpresa.TikTok;
            lojaParaEdicao.YouTube = requestDadosEmpresa.YouTube;
            lojaParaEdicao.LinkedIn = requestDadosEmpresa.LinkedIn;
            lojaParaEdicao.Site = requestDadosEmpresa.Site;
            lojaParaEdicao.Logo = requestDadosEmpresa.Logo;
            lojaParaEdicao.DataAlteracao = DateTime.Now;

            await _menuFastContext.SaveChangesAsync();

            return lojaParaEdicao;
        }
    }
}
