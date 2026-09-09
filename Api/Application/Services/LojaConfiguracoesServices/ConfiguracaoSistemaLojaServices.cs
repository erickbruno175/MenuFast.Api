using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Application.Services.Services.OpenRouteService;
using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesLoja;
using MenuFast.Api.Api.Domain.Entities.Models.Loja;
using MenuFast.Api.Api.Domain.Enum;
using MenuFast.Api.Api.Persistence.Context;
using MenuFast.Api.Api.Util.Helpers;
using MenuFast.Api.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace MenuFast.Api.Api.Application.Services.LojaConfiguracoes;

public class ConfiguracaoSistemaLojaServices {
    private readonly MenuFastContext _menuFastContext;
    private readonly IDistributedCache _cache;
    private readonly OpenRouteServices _openRouteServices;

    public ConfiguracaoSistemaLojaServices(MenuFastContext menuFastContext, IDistributedCache redis, OpenRouteServices openRouteServices) {
        _menuFastContext = menuFastContext;
        _cache = redis;
        _openRouteServices = openRouteServices;
    }

    public async Task<DadosLojaResponse> CadastrarDadosLoja(DadosEmpresaRequest requestDadosEmpresa) {
        if(!DocumentoHelper.ValidarCnpj(requestDadosEmpresa.Cnpj))
            throw new BusinessLogicException("CNPJ inválido.");

        var coordenadas = await _openRouteServices.BuscarCoordenadasAsync(
            requestDadosEmpresa.Cep,
            requestDadosEmpresa.Logradouro,
            requestDadosEmpresa.Numero,
            requestDadosEmpresa.Bairro,
            requestDadosEmpresa.Cidade,
            requestDadosEmpresa.Estado);

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
            Cep = requestDadosEmpresa.Cep,
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
            Logo = requestDadosEmpresa.Logo,
            Latitude = coordenadas?.Latitude,
            Longitude = coordenadas?.Longitude,
            ConfiguracaoFinalizada = false
        };

        await _menuFastContext.Lojas.AddAsync(loja);
        await _menuFastContext.SaveChangesAsync();

        return await MapearDados(loja);

    }

    public async Task<DadosLojaResponse> AtualizarDadosLoja(int idLoja, DadosEmpresaRequest requestDadosEmpresa) {
        var loja = await _menuFastContext.Lojas.FindAsync(idLoja);

        if(loja == null)
            throw new BusinessLogicException("Loja não encontrada.");

        if(!DocumentoHelper.ValidarCnpj(requestDadosEmpresa.Cnpj))
            throw new BusinessLogicException("CNPJ inválido.");

        var coordenadas = await _openRouteServices.BuscarCoordenadasAsync(
            requestDadosEmpresa.Cep,
            requestDadosEmpresa.Logradouro,
            requestDadosEmpresa.Numero,
            requestDadosEmpresa.Bairro,
            requestDadosEmpresa.Cidade,
            requestDadosEmpresa.Estado);

        loja.Slug = SlugHelper.GerarSlug(requestDadosEmpresa.NomeFantasia);
        loja.RazaoSocial = requestDadosEmpresa.RazaoSocial;
        loja.Cnpj = DocumentoHelper.RemoverCaracteresEspeciais(requestDadosEmpresa.Cnpj);
        loja.NomeFantasia = requestDadosEmpresa.NomeFantasia;
        loja.InscricaoEstadual = requestDadosEmpresa.InscricaoEstadual;
        loja.Telefone = DocumentoHelper.RemoverMascaraTelefone(requestDadosEmpresa.Telefone);
        loja.Email = requestDadosEmpresa.Email;
        loja.Cep = requestDadosEmpresa.Cep;
        loja.Logradouro = requestDadosEmpresa.Logradouro;
        loja.Numero = requestDadosEmpresa.Numero;
        loja.Bairro = requestDadosEmpresa.Bairro;
        loja.Cidade = requestDadosEmpresa.Cidade;
        loja.Estado = requestDadosEmpresa.Estado;
        loja.Uf = requestDadosEmpresa.Uf;
        loja.Complemento = requestDadosEmpresa.Complemento;
        loja.Sigla = requestDadosEmpresa.Sigla;
        loja.WhatsApp = requestDadosEmpresa.WhatsApp;
        loja.Site = requestDadosEmpresa.Site;
        loja.Logo = requestDadosEmpresa.Logo;
        loja.Latitude = coordenadas?.Latitude;
        loja.Longitude = coordenadas?.Longitude;
        loja.DataAlteracao = DateTime.Now;

        await _menuFastContext.SaveChangesAsync();
        await _cache.RemoveAsync($"configuracoes-loja:{idLoja}");

        return await MapearDados(loja);
    }


    public async Task<IEnumerable<HorarioFuncionamento>> CadastrarHorarioFuncionamento(int idLoja, List<CadastrarHorarioFuncionamentoRequest> horariosRequest) {
        var lojaExiste = await _menuFastContext.Lojas.AnyAsync(x => x.Id == idLoja);

        if(!lojaExiste)
            throw new BusinessLogicException("Loja não encontrada.");

        if(horariosRequest == null || !horariosRequest.Any())
            throw new BusinessLogicException("Informe os horários de funcionamento.");

        var horariosExistentes = await _menuFastContext.HorariosFuncionamento
            .Where(x => x.LojaId == idLoja)
            .ToListAsync();

        if(horariosExistentes.Any())
            _menuFastContext.HorariosFuncionamento.RemoveRange(horariosExistentes);

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
        await _cache.RemoveAsync($"configuracoes-loja:{idLoja}");

        return horarios;
    }

    public async Task<IEnumerable<HorarioFuncionamento>> AtualizarHorarioFuncionamento(List<CadastrarHorarioFuncionamentoRequest> horariosRequest, int idHorario) {
        var horario = await _menuFastContext.HorariosFuncionamento.FirstOrDefaultAsync(x => x.Id == idHorario);

        if(horario == null)
            throw new BusinessLogicException("Horário de funcionamento não encontrado.");

        var request = horariosRequest?.FirstOrDefault();

        if(request == null)
            throw new BusinessLogicException("Horário de funcionamento não informado.");

        horario.DiaSemana = request.DiaSemana;
        horario.Fechado = request.Fechado;
        horario.HoraAbertura = request.HoraAbertura;
        horario.HoraFechamento = request.HoraFechamento;

        await _menuFastContext.SaveChangesAsync();
        await _cache.RemoveAsync($"configuracoes-loja:{horario.LojaId}");

        return await _menuFastContext.HorariosFuncionamento
            .Where(x => x.LojaId == horario.LojaId)
            .ToListAsync();
    }

    public async Task<ConfiguracaoLoja> CadastrarConfiguracaoLoja(int idLoja, CadastrarConfiguracaoLojaRequest request) {
        var loja = await _menuFastContext.Lojas.FirstOrDefaultAsync(x => x.Id == idLoja);

        if(loja == null)
            throw new BusinessLogicException("Loja não encontrada.");

        var possuiHorario = await _menuFastContext.HorariosFuncionamento.AnyAsync(x => x.LojaId == idLoja);

        if(!possuiHorario)
            throw new BusinessLogicException("Cadastre o horário de funcionamento antes de finalizar a configuração da loja.");

        var configuracaoExistente = await _menuFastContext.ConfiguracoesLoja.FirstOrDefaultAsync(x => x.LojaId == idLoja);

        if(configuracaoExistente != null)
            throw new BusinessLogicException("A configuração da loja já foi cadastrada.");

        var configuracao = new ConfiguracaoLoja
        {
            CobraTaxaServico = request.CobraTaxaServico,
            PercentualTaxaServico = request.PercentualTaxaServico,
            CobraTaxaEntrega = request.CobraTaxaEntrega,
            TipoTaxaEntrega = request.TipoTaxaEntrega,
            TaxaEntrega = request.TaxaEntrega,
            TaxaBaseEntrega = request.TaxaBaseEntrega,
            ValorPorKm = request.ValorPorKm,
            DistanciaMaximaEntregaKm = request.DistanciaMaximaEntregaKm,
            PermiteVendaSemEstoque = request.PermiteVendaSemEstoque,
            TrabalhaComDelivery = request.TrabalhaComDelivery,
            TrabalhaComMesa = request.TrabalhaComMesa,
            TrabalhaComRetirada = request.TrabalhaComRetirada,
            AbilitarImpressoraTermica = request.AbilitarImpressoraTermica,
            AbilitarKDS = request.AbilitarKDS,
            LojaId = idLoja
        };

        await _menuFastContext.ConfiguracoesLoja.AddAsync(configuracao);

        loja.ConfiguracaoFinalizada = true;

        var administradores = await _menuFastContext.Funcionarios
            .Where(x => x.LojaId == idLoja &&
                        x.PerfilId == (int)PerfilUsuario.Administrador &&
                        x.Ativo &&
                        x.PrimeiroAcesso)
            .ToListAsync();

        foreach(var administrador in administradores)
            administrador.PrimeiroAcesso = false;

        await _menuFastContext.SaveChangesAsync();
        await _cache.RemoveAsync($"configuracoes-loja:{idLoja}");

        return configuracao;
    }

    public async Task<ConfiguracaoLoja> AtualizarConfiguracaoLoja(int idLoja, CadastrarConfiguracaoLojaRequest request) {
        var configuracao = await _menuFastContext.ConfiguracoesLoja.FirstOrDefaultAsync(x => x.LojaId == idLoja);

        if(configuracao == null)
            throw new BusinessLogicException("Configurações da loja não encontradas.");

        configuracao.TrabalhaComRetirada = request.TrabalhaComRetirada;
        configuracao.TrabalhaComMesa = request.TrabalhaComMesa;
        configuracao.TrabalhaComDelivery = request.TrabalhaComDelivery;
        configuracao.PermiteVendaSemEstoque = request.PermiteVendaSemEstoque;
        configuracao.CobraTaxaServico = request.CobraTaxaServico;
        configuracao.PercentualTaxaServico = request.PercentualTaxaServico;
        configuracao.CobraTaxaEntrega = request.CobraTaxaEntrega;
        configuracao.TipoTaxaEntrega = request.TipoTaxaEntrega;
        configuracao.TaxaEntrega = request.TaxaEntrega;
        configuracao.TaxaBaseEntrega = request.TaxaBaseEntrega;
        configuracao.ValorPorKm = request.ValorPorKm;
        configuracao.DistanciaMaximaEntregaKm = request.DistanciaMaximaEntregaKm;
        configuracao.AbilitarKDS = request.AbilitarKDS;
        configuracao.AbilitarImpressoraTermica = request.AbilitarImpressoraTermica;

        await _menuFastContext.SaveChangesAsync();
        await _cache.RemoveAsync($"configuracoes-loja:{idLoja}");

        return configuracao;
    }

    public async Task<ConfiguracoesLojaResponse> ConsultarConfiguracoesLoja(int lojaId) {
        var cacheKey = $"configuracoes-loja:{lojaId}";
        var cache = await _cache.GetStringAsync(cacheKey);

        if(!string.IsNullOrEmpty(cache))
            return JsonSerializer.Deserialize<ConfiguracoesLojaResponse>(cache)!;

        var loja = await _menuFastContext.Lojas
            .Include(l => l.Configuracao)
            .Include(l => l.Horarios)
            .FirstOrDefaultAsync(x => x.Id == lojaId);

        if(loja == null)
            throw new BusinessLogicException("Loja não encontrada.");

        if(loja.Configuracao == null)
            throw new BusinessLogicException("Configurações da loja não encontradas.");
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
            PercentualTaxaServico = loja.Configuracao.PercentualTaxaServico ?? 0,
            CobraTaxaEntrega = loja.Configuracao.CobraTaxaEntrega,
            TipoTaxaEntrega = loja.Configuracao.TipoTaxaEntrega,
            TaxaEntrega = loja.Configuracao.TaxaEntrega,
            TaxaBaseEntrega = loja.Configuracao.TaxaBaseEntrega,
            ValorPorKm = loja.Configuracao.ValorPorKm,
            DistanciaMaximaEntregaKm = loja.Configuracao.DistanciaMaximaEntregaKm,
            AbilitarImpressoraTermica = loja.Configuracao.AbilitarImpressoraTermica,
            AbilitarKDS = loja.Configuracao.AbilitarKDS,
            ValorAberturaCaixa = loja.Configuracao.ValorAberturaCaixa,
            HorarioFuncionamentos = loja.Horarios.Select(x => new HorarioFuncionamentoResponse
            {
                Id = x.Id,
                LojaId = x.LojaId,
                DiaSemana = x.DiaSemana,
                HoraAbertura = x.HoraAbertura,
                HoraFechamento = x.HoraFechamento,
                Fechado = x.Fechado
            }).ToList()
        };

        await _cache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(response),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });

        return response;
    }

    public async Task<bool> LembrarFinalizarCadastroConfiguracoesLoja(int idFuncionario) {
        var funcionario = await _menuFastContext.Funcionarios.FirstOrDefaultAsync(
            f => f.Id == idFuncionario &&
                 f.PerfilId == (int)PerfilUsuario.Administrador &&
                 f.Ativo);

        if(funcionario == null)
            return false;

        return funcionario.PrimeiroAcesso;
    }

    public async Task<IEnumerable<FormaPagamentoResponse>> ConsultarFormasPagamento() {
        var pagamentos = await _menuFastContext.FormasPagamento
            .AsNoTracking()
            .Where(x => x.Ativo)
            .Select(x => new FormaPagamentoResponse
            {
                Id = x.Id,
                Descricao = x.Descricao
            })
            .ToListAsync();

        return pagamentos;
    }

    private async Task<DadosLojaResponse> MapearDados(Loja loja) {

        return new DadosLojaResponse
        {
            Id = loja.Id,
            Ativo = loja.Ativo,
            RazaoSocial = loja.RazaoSocial,
            NomeFantasia = loja.NomeFantasia,
            Cnpj = loja.Cnpj,
            InscricaoEstadual = loja.InscricaoEstadual,
            Telefone = loja.Telefone,
            Email = loja.Email,
            Cep = loja.Cep,
            Logradouro = loja.Logradouro,
            Numero = loja.Numero,
            Bairro = loja.Bairro,
            Cidade = loja.Cidade,
            Estado = loja.Estado,
            Uf = loja.Uf,
            Complemento = loja.Complemento,
            Sigla = loja.Sigla,
            WhatsApp = loja.WhatsApp,
            Site = loja.Site,
            Logo = loja.Logo,
            Latitude = (double)loja.Latitude,
            Longitude = (double)loja.Longitude,
            ConfiguracaoFinalizada = loja.ConfiguracaoFinalizada
        };

    }
}
