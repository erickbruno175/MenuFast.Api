using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesLoja;
using MenuFast.Api.Api.Domain.Entities.Models.Financeiro;
using MenuFast.Api.Api.Domain.Enum;
using MenuFast.Api.Api.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MenuFast.Api.Api.Application.Services.CaixaServices;

public class CaixaService {
    private readonly MenuFastContext _contexto;

    public CaixaService(MenuFastContext contexto) {
        _contexto = contexto;
    }

  
    public async Task<Caixa> AbrirCaixaAsync(int lojaId,int funcionarioId,string nome = "Caixa") {
        var caixaAberto = await _contexto.Caixas.FirstOrDefaultAsync(x =>x.LojaId == lojaId &&x.Aberto);

        if(caixaAberto != null)
            throw new Exception("Já existe um caixa aberto para esta loja.");

        var configuracao = await _contexto.ConfiguracoesLoja
            .FirstOrDefaultAsync(x => x.LojaId == lojaId);

        if(configuracao == null)
            throw new Exception("Configuração da loja não encontrada.");

        var caixa = new Caixa
        {
            LojaId = lojaId,
            Nome = nome,
            Aberto = true,
            ValorAbertura = configuracao.ValorAberturaCaixa!.Value,
            ValorFechamento = 0,
            DataAbertura = DateTime.Now,
            FuncioanrioId = funcionarioId
        };

        _contexto.Caixas.Add(caixa);

        await _contexto.SaveChangesAsync();

        return caixa;
    }


    public async Task<Caixa?> BuscarCaixaAbertoAsync(int lojaId) {
        return await _contexto.Caixas.Include(x => x.Movimentos).FirstOrDefaultAsync(x =>x.LojaId == lojaId &&x.Aberto);
    }


    public async Task<MovimentoCaixa> RegistrarMovimentoAsync(int lojaId,int funcionarioId,TipoMovimentoCaixa tipo,decimal valor,
        string? descricao = null) {
        var caixa = await _contexto.Caixas.FirstOrDefaultAsync(x =>x.LojaId == lojaId &&x.Aberto);

        if(caixa == null)throw new Exception("Não existe caixa aberto para esta loja.");

        if(valor <= 0)throw new Exception("O valor do movimento deve ser maior que zero.");

        var movimento = new MovimentoCaixa
        {
            LojaId = lojaId,
            CaixaId = caixa.Id,
            FuncionarioId = funcionarioId,
            Tipo = tipo,
            Valor = valor,
            Descricao = descricao,
            Data = DateTime.Now,
            Status = StatusContaFinanceira.Ativo
        };

        _contexto.MovimentosCaixa.Add(movimento);

        await _contexto.SaveChangesAsync();
        return movimento;
    }


    public async Task<MovimentoCaixa> RegistrarSangriaAsync(int lojaId,int funcionarioId,decimal valor,string? descricao = null) {
        return await RegistrarMovimentoAsync(lojaId,funcionarioId,TipoMovimentoCaixa.Sangria,valor,descricao);
    }



    public async Task<MovimentoCaixa> RegistrarSuprimentoAsync(int lojaId,int funcionarioId,decimal valor,string? descricao = null) {
        return await RegistrarMovimentoAsync(lojaId,funcionarioId,TipoMovimentoCaixa.Suprimento,valor,descricao);
    }


    public async Task<List<MovimentoCaixa>> BuscarMovimentosAsync(int lojaId) {
        var caixa = await _contexto.Caixas.FirstOrDefaultAsync(x =>x.LojaId == lojaId &&x.Aberto);

        if(caixa == null)throw new Exception("Não existe caixa aberto para esta loja.");

        return await _contexto.MovimentosCaixa
            .Where(x =>
                x.CaixaId == caixa.Id &&
                x.Status == StatusContaFinanceira.Ativo)
            .OrderByDescending(x => x.Data)
            .ToListAsync();
    }


    public async Task<decimal> CalcularValorAtualAsync(int lojaId) {
        var caixa = await _contexto.Caixas
            .FirstOrDefaultAsync(x =>
                x.LojaId == lojaId &&
                x.Aberto);

        if(caixa == null)throw new Exception("Não existe caixa aberto para esta loja.");

        var movimentos = await _contexto.MovimentosCaixa
            .Where(x =>
                x.CaixaId == caixa.Id &&
                x.Status == StatusContaFinanceira.Ativo)
            .ToListAsync();

        decimal valor = caixa.ValorAbertura;

        foreach(var movimento in movimentos)
        {
            switch(movimento.Tipo)
            {
            case TipoMovimentoCaixa.Venda:
            case TipoMovimentoCaixa.Suprimento:
            case TipoMovimentoCaixa.Entrada:
                valor += movimento.Valor;
                break;

            case TipoMovimentoCaixa.Sangria:
            case TipoMovimentoCaixa.Saida:
            case TipoMovimentoCaixa.Estorno:
                valor -= movimento.Valor;
                break;
            }
        }

        return valor;
    }
  
    public async Task<Caixa> FecharCaixaAsync(int lojaId,int funcionarioId,decimal valorFechamento) {
        var caixa = await _contexto.Caixas.FirstOrDefaultAsync(x =>x.LojaId == lojaId && x.Aberto);

        if(caixa == null) throw new Exception("Não existe caixa aberto para esta loja.");

        if(valorFechamento < 0)throw new Exception("O valor de fechamento não pode ser negativo.");

        var valorCalculado = await CalcularValorAtualAsync(lojaId);

        caixa.ValorFechamento = valorFechamento;
        caixa.DataFechamento = DateTime.Now;
        caixa.Aberto = false;

        await _contexto.SaveChangesAsync();

        return caixa;
    }
}