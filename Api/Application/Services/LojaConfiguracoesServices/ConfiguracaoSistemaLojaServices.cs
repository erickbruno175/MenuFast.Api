using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.InkML;
using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Application.Services.Redis;
using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesLoja;
using MenuFast.Api.Api.Domain.Entities.Models.Loja;
using MenuFast.Api.Api.Domain.Enum;
using MenuFast.Api.Api.Persistence.Context;
using MenuFast.Api.Api.Util.Helpers;
using MenuFast.Api.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace MenuFast.Api.Api.Application.Services.LojaConfiguracoes {
    public class ConfiguracaoSistemaLojaServices {

        private readonly MenuFastContext _menuFastContext;
        private readonly IDistributedCache _cache;
        public ConfiguracaoSistemaLojaServices(MenuFastContext menuFastContext, IDistributedCache redis) {
            _menuFastContext = menuFastContext;
            _cache = redis;
        }


        public async Task<Loja> CadastrarDadosLoja(DadosEmpresaRequest requestDadosEmpresa) {


            if(!DocumentoHelper.ValidarCnpj(requestDadosEmpresa.Cnpj))
            {
                throw new BusinessLogicException("CNPJ inválido.");

            }
            var loja = new Loja
            {
                Ativo = true,
                DataCadastro = DateTime.Now,
                Slug = SlugHelper.GerarSlug(requestDadosEmpresa.NomeFantasia),
                RazaoSocial = requestDadosEmpresa.RazaoSocial,
                Cnpj = DocumentoHelper.RemoverCaracteresEspeciais(requestDadosEmpresa.Cnpj),
                NomeFantasia = requestDadosEmpresa.NomeFantasia,
                InscricaoEstadual = requestDadosEmpresa.InscricaoEstadual,
                Telefone = DocumentoHelper.RemoverMascaraTelefone(requestDadosEmpresa.Telefone),
                Email = requestDadosEmpresa.Email,
                Logradouro = requestDadosEmpresa.Logradouro,
                Numero = requestDadosEmpresa.Numero,
                Bairro = requestDadosEmpresa.Bairro,
                Cidade = requestDadosEmpresa.Cidade,
                Estado = requestDadosEmpresa.Estado,
                Uf = requestDadosEmpresa.Uf,
                Complemento = requestDadosEmpresa.Complemento,
                Sigla = requestDadosEmpresa.Sigla,
                WhatsApp = requestDadosEmpresa.WhatsApp,
                Site = requestDadosEmpresa.Site,
                Logo = requestDadosEmpresa.Logo
            };

            await _menuFastContext.Lojas.AddAsync(loja);
            await _menuFastContext.SaveChangesAsync();

            return loja;
        }

        public async Task<Loja> AtualizarDadosLoja(int idLoja, DadosEmpresaRequest requestDadosEmpresa) {
            var lojaParaEdicao = await _menuFastContext.Lojas.FindAsync(idLoja);

            if(lojaParaEdicao == null)
                return null;

            if(!DocumentoHelper.ValidarCnpj(requestDadosEmpresa.Cnpj))
            {
                throw new BusinessLogicException("CNPJ inválido.");

            }

            lojaParaEdicao.Slug = SlugHelper.GerarSlug(requestDadosEmpresa.NomeFantasia);
            lojaParaEdicao.RazaoSocial = requestDadosEmpresa.RazaoSocial;
            lojaParaEdicao.NomeFantasia = requestDadosEmpresa.NomeFantasia;
            lojaParaEdicao.InscricaoEstadual = requestDadosEmpresa.InscricaoEstadual;
            lojaParaEdicao.Telefone = DocumentoHelper.RemoverMascaraTelefone(requestDadosEmpresa.Telefone);
            lojaParaEdicao.Email = requestDadosEmpresa.Email;
            lojaParaEdicao.Logradouro = requestDadosEmpresa.Logradouro;
            lojaParaEdicao.Numero = requestDadosEmpresa.Numero;
            lojaParaEdicao.Bairro = requestDadosEmpresa.Bairro;
            lojaParaEdicao.Cidade = requestDadosEmpresa.Cidade;
            lojaParaEdicao.Estado = requestDadosEmpresa.Estado;
            lojaParaEdicao.Uf = requestDadosEmpresa.Uf;
            lojaParaEdicao.Complemento = requestDadosEmpresa.Complemento;
            lojaParaEdicao.Sigla = requestDadosEmpresa.Sigla;
            lojaParaEdicao.WhatsApp = requestDadosEmpresa.WhatsApp;
            lojaParaEdicao.Site = requestDadosEmpresa.Site;
            lojaParaEdicao.Logo = requestDadosEmpresa.Logo;
            lojaParaEdicao.DataAlteracao = DateTime.Now;

            await _menuFastContext.SaveChangesAsync();
            return lojaParaEdicao;
        }
        public async Task<IEnumerable<HorarioFuncionamento>> CadastrarHorarioFuncionemnto(int idLoja, List<CadastrarHorarioFuncionamentoRequest> horariosRequest) {

            var horarios = horariosRequest.Select(x => new HorarioFuncionamento
            {

                DiaSemana = x.DiaSemana,
                Fechado = x.Fechado,
                HoraAbertura = x.HoraAbertura,
                HoraFechamento = x.HoraFechamento,
                LojaId = idLoja

            }).ToList();
            await _menuFastContext.HorariosFuncionamento.AddRangeAsync(horarios);
            await _menuFastContext.SaveChangesAsync();


            var configuracaoLoja = await _menuFastContext.ConfiguracoesLoja
           .FirstOrDefaultAsync(x => x.LojaId == idLoja);

            if(configuracaoLoja != null)
            {
                var loja = await _menuFastContext.Lojas.FirstOrDefaultAsync(x => x.Id == idLoja);

                if(loja != null)
                {
                    loja.ConfiguracaoFinalizada = true;
                }
            }

            await _menuFastContext.SaveChangesAsync();

            return horarios;
        }
        public async Task<IEnumerable<HorarioFuncionamento>> AtualizarHorarioFuncionamento(List<CadastrarHorarioFuncionamentoRequest> horariosRequest, int idHorario) {
            foreach(var request in horariosRequest)
            {
                var horario = await _menuFastContext.HorariosFuncionamento.FirstOrDefaultAsync(x => x.Id == idHorario);

                if(horario == null)
                    continue;

                horario.DiaSemana = request.DiaSemana;
                horario.Fechado = request.Fechado;
                horario.HoraAbertura = request.HoraAbertura;
                horario.HoraFechamento = request.HoraFechamento;
            }

            await _menuFastContext.SaveChangesAsync();

            return await _menuFastContext.HorariosFuncionamento.Where(x => horariosRequest.Select(r => idHorario).Contains(x.Id)).ToListAsync();
        }

        public async Task<ConfiguracaoLoja> CadastrarConfiguracaoLoja(int idLoja, CadastrarConfiguracaoLojaRequest request) {
            var confLoja = new ConfiguracaoLoja
            {
                CobraTaxaServico = request.CobraTaxaServico,
                EnviarPedidoAutomaticamenteCozinha = request.EnviarPedidoAutomaticamenteCozinha,
                ExigirGarcomNaMesa = request.ExigirGarcomNaMesa,
                PercentualTaxaServico = request.PercentualTaxaServico,
                PermiteVendaSemEstoque = request.PermiteVendaSemEstoque,
                TrabalhaComDelivery = request.TrabalhaComDelivery,
                TrabalhaComMesa = request.TrabalhaComMesa,
                TrabalhaComRetirada = request.TrabalhaComRetirada,
                EnviarPedidoAutomaticamenteBar = request.EnviarPedidoAutomaticamenteBar,
                AbilitarImpressoraTermica = request.AbilitarImpressoraTermica,
                AbilitarKDS = request.AbilitarKDS,
                LojaId = idLoja,
            };
            await _menuFastContext.AddAsync(confLoja);
            await _menuFastContext.SaveChangesAsync();
            return confLoja;
        }

        public async Task<ConfiguracaoLoja> AtualizarConfiguracaoLoja(int idConfig, CadastrarConfiguracaoLojaRequest request) {

            var configuracaoLojaEditar = await _menuFastContext.ConfiguracoesLoja.FirstOrDefaultAsync(f => f.LojaId == idConfig);

            if(configuracaoLojaEditar == null) return null;

            configuracaoLojaEditar.TrabalhaComRetirada = request.TrabalhaComRetirada;
            configuracaoLojaEditar.TrabalhaComMesa = request.TrabalhaComMesa;
            configuracaoLojaEditar.ExigirGarcomNaMesa = request.ExigirGarcomNaMesa;
            configuracaoLojaEditar.CobraTaxaServico = request.CobraTaxaServico;
            configuracaoLojaEditar.TrabalhaComDelivery = request.TrabalhaComDelivery;
            configuracaoLojaEditar.EnviarPedidoAutomaticamenteCozinha = request.EnviarPedidoAutomaticamenteCozinha;
            configuracaoLojaEditar.EnviarPedidoAutomaticamenteBar = request.EnviarPedidoAutomaticamenteBar;
            configuracaoLojaEditar.AbilitarKDS = request.AbilitarKDS;
            configuracaoLojaEditar.AbilitarImpressoraTermica = request.AbilitarImpressoraTermica;
            await _menuFastContext.SaveChangesAsync();
            return configuracaoLojaEditar;
        }

        public async Task<ConfiguracoesLojaResponse> ConsultarConfiguracoesLoja(int lojaId) {

            var cacheKey = $"configuracoes-loja:{lojaId}";
            await _cache.RemoveAsync(cacheKey);

            var cache = await _cache.GetStringAsync(cacheKey);
            if(!string.IsNullOrEmpty(cache))
            {
                return JsonSerializer.Deserialize<ConfiguracoesLojaResponse>(cache)!;
            }

            var loja = await _menuFastContext.Lojas
                .Include(l => l.Configuracao)
                .Include(h => h.Horarios)
                .FirstOrDefaultAsync(x => x.Id == lojaId);

            if(loja == null) throw new BusinessLogicException("Loja não encontrada.");

            if(loja.Configuracao == null) throw new BusinessLogicException("Configurações da loja não encontradas.");

            var response = new ConfiguracoesLojaResponse
            {
                Id = loja.Configuracao.Id,

                Ativo = loja.Ativo,
                RazaoSocial = loja.RazaoSocial,
                Email = loja.Email,

                TrabalhaComMesa = loja.Configuracao.TrabalhaComMesa,
                TrabalhaComDelivery = loja.Configuracao.TrabalhaComDelivery,
                TrabalhaComRetirada = loja.Configuracao.TrabalhaComRetirada,
                PermiteVendaSemEstoque = loja.Configuracao.PermiteVendaSemEstoque,
                CobraTaxaServico = loja.Configuracao.CobraTaxaServico,
                PercentualTaxaServico = loja.Configuracao.PercentualTaxaServico?? 0,
                ExigirGarcomNaMesa = loja.Configuracao.ExigirGarcomNaMesa,
                EnviarPedidoAutomaticamenteCozinha = loja.Configuracao.EnviarPedidoAutomaticamenteCozinha,
                EnviarPedidoAutomaticamenteBar = loja.Configuracao.EnviarPedidoAutomaticamenteBar,
                AbilitarImpressoraTermica = loja.Configuracao.AbilitarImpressoraTermica,
                AbilitarKDS = loja.Configuracao.AbilitarKDS,
                horarioFuncionamentos = loja.Horarios.Select(x => new HorarioFuncionamento
                {
                    DiaSemana = x.DiaSemana,
                    Fechado = x.Fechado,
                    HoraAbertura = x.HoraAbertura,
                    HoraFechamento = x.HoraFechamento,
                }).ToList(),
            };


            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(response),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });

            return response;
        }
        public async Task<bool> LembrarFinalizarCadastroConfiguracoesLoja(int idFuncionario) {
            var funcionario = await _menuFastContext.Funcionarios
                .Include(f => f.Loja)
                .FirstOrDefaultAsync(f =>
                    f.Id == idFuncionario &&
                    f.PerfilId == (int)PerfilUsuario.Administrador &&
                    f.Ativo);


            if(funcionario.Loja.ConfiguracaoFinalizada) return false;
            var possuiConfiguracao = await _menuFastContext.ConfiguracoesLoja.AnyAsync(x => x.LojaId == funcionario.Loja.Id);
            var possuiHorario = await _menuFastContext.HorariosFuncionamento.AnyAsync(x => x.LojaId == funcionario.Loja.Id);
            return !possuiConfiguracao || !possuiHorario;
        }
    

    }
}
