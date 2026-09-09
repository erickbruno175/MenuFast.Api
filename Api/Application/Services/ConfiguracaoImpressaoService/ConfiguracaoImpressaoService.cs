using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracaoImpressao;
using MenuFast.Api.Api.Persistence.Context;
using Microsoft.EntityFrameworkCore;

public class ConfiguracaoImpressaoService {
    private readonly MenuFastContext _context;

    public ConfiguracaoImpressaoService(MenuFastContext context) {
        _context = context;
    }

    public async Task<ConfiguracaoImpressaoResponse> ObterAsync(int lojaId) {
        var configuracao = await _context.ConfiguracoesImpressao
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.LojaId == lojaId);

        if(configuracao == null)
        {
            configuracao = new ConfiguracaoImpressao
            {
                LojaId = lojaId,
                LarguraPapel = 80,
                MostrarNomeLoja = true,
                MostrarCnpj = true,
                MostrarEndereco = true,
                MostrarTelefone = true
            };

            _context.ConfiguracoesImpressao.Add(configuracao);

            await _context.SaveChangesAsync();
        }

        return new ConfiguracaoImpressaoResponse
        {
            Id = configuracao.Id,
            ImprimirPedidoAoEnviar = configuracao.ImprimirPedidoAoEnviar,
            ImprimirPedidoCozinha = configuracao.ImprimirPedidoCozinha,
            ImprimirPedidoBar = configuracao.ImprimirPedidoBar,
            ImpressoraPadrao = configuracao.ImpressoraPadrao,
            LarguraPapel = configuracao.LarguraPapel,
            MostrarNomeLoja = configuracao.MostrarNomeLoja,
            MostrarCnpj = configuracao.MostrarCnpj,
            MostrarEndereco = configuracao.MostrarEndereco,
            MostrarTelefone = configuracao.MostrarTelefone,
            MensagemRodape = configuracao.MensagemRodape
        };
    }

    public async Task AtualizarAsync(int lojaId,AtualizarConfiguracaoImpressaoRequest request) {
        var configuracao = await _context.ConfiguracoesImpressao.FirstOrDefaultAsync(x => x.LojaId == lojaId);

        if(configuracao == null)
        {
            configuracao = new ConfiguracaoImpressao
            {
                LojaId = lojaId
            };

            _context.ConfiguracoesImpressao.Add(configuracao);
        }

        configuracao.ImprimirPedidoAoEnviar =request.ImprimirPedidoAoEnviar;
        configuracao.ImprimirPedidoCozinha =request.ImprimirPedidoCozinha;
        configuracao.ImprimirPedidoBar =request.ImprimirPedidoBar;
        configuracao.ImpressoraPadrao =request.ImpressoraPadrao;

        configuracao.LarguraPapel =request.LarguraPapel;
        configuracao.MostrarNomeLoja =request.MostrarNomeLoja;
        configuracao.MostrarCnpj =request.MostrarCnpj;
        configuracao.MostrarEndereco =request.MostrarEndereco;
        configuracao.MostrarTelefone =request.MostrarTelefone;
        configuracao.MensagemRodape =request.MensagemRodape;
        await _context.SaveChangesAsync();
    }
}