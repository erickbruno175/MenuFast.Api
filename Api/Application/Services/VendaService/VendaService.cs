using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Domain.Entities.Models.Financeiro;
using MenuFast.Api.Api.Domain.Entities.Models.Pedido;
using MenuFast.Api.Api.Domain.Enum;
using MenuFast.Api.Api.Persistence.Context;
using MenuFast.Api.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace MenuFast.Api.Api.Application.Services.VendaService;

public class VendaService {
    private readonly MenuFastContext _context;
    private readonly EstoqueServices.EstoqueServices _estoqueServices;

    public VendaService(MenuFastContext context, EstoqueServices.EstoqueServices estoqueServices) {
        _context = context;
        _estoqueServices = estoqueServices;
    }

    public async Task<VendaResponse> FinalizarVendaAsync(int lojaId, ConfirmarPagamentoRequest request) {
        if(request == null)
            throw new BusinessLogicException("Os dados da venda são obrigatórios.");

        if(request.Pagamentos == null || !request.Pagamentos.Any())
            throw new BusinessLogicException("Informe pelo menos uma forma de pagamento.");

        if(!request.PedidoId.HasValue && !request.MesaId.HasValue)
            throw new BusinessLogicException("Informe o pedido ou a mesa para finalizar a venda.");

        List<Pedido> pedidos;

        if(request.MesaId.HasValue)
        {
            pedidos = await BuscarPedidosMesaAsync(lojaId, request.MesaId.Value);
        }
        else
        {
            var pedido = await BuscarPedidoAsync(lojaId, request.PedidoId.Value);
            pedidos = [ pedido ];
        }

        ValidarPedidos(pedidos);

        var valorTotal = pedidos.Sum(x => x.Total);

        ValidarPagamentos(request.Pagamentos, valorTotal);

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var venda = new Venda
            {
                LojaId = lojaId,
                ValorBruto = pedidos.Sum(x => x.Subtotal),
                Desconto = pedidos.Sum(x => x.Desconto),
                Acrescimo = pedidos.Sum(x => x.TaxaServico + x.TaxaEntrega),
                ValorTotal = valorTotal,
                DataVenda = DateTime.Now,
                StatusPagamento = StatusPagamento.Confirmado
            };

            foreach(var pedido in pedidos)
            {
                venda.Pedidos.Add(pedido);
            }

            _context.Vendas.Add(venda);
            var totalPago = request.Pagamentos.Sum(x => x.Valor);
            var troco = totalPago > valorTotal ? totalPago - valorTotal : 0;
            foreach(var pagamentoRequest in request.Pagamentos)
            {
                var pagamento = new PagamentoVenda
                {
                    Venda = venda,
                    FormaPagamentoId = pagamentoRequest.FormaPagamentoId,
                    Valor = pagamentoRequest.Valor,
                    Troco = troco
                };

                _context.PagamentosVenda.Add(pagamento);
            }

            foreach(var pedido in pedidos)
            {
                await _estoqueServices.BaixaEstoqueProduto(pedido);

                if(pedido.FuncionarioId.HasValue)
                {
                    var funcionario = await _context.Funcionarios
                         .Include(l=> l.Loja)
                         .ThenInclude(c=> c.Configuracao)
                        .FirstOrDefaultAsync(f=> f.Id == pedido.FuncionarioId.Value && f.LojaId == lojaId && f.PerfilId == (int) PerfilUsuario.Garcom);

                    if(funcionario != null && funcionario.PercentualComissao >0) {
                        var valorComissao = pedido.Total * funcionario.PercentualComissao / funcionario.Loja?.Configuracao?.PercentualTaxaServico;
                        var comissao = new ComissaoVenda
                        {
                            FuncionarioId = funcionario.Id,
                            PedidoId = pedido.Id,
                            ValorVenda = pedido.Total,
                            PercentualComissao = funcionario.PercentualComissao,
                            ValorComissao = valorComissao!.Value,
                            DataVenda = DateTime.Now,
                            StatusComissao = StatusComissao.Pendente
                        };

                        _context.ComissoesVenda.Add(comissao);
                    }
                }
                pedido.Status = StatusPedido.Finalizado;
            }

            await LiberarMesaAsync(pedidos);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return MapearResponse(venda);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<VendaResponse> BuscarPorIdAsync(int vendaId, int lojaId) {
        var venda = await BuscarVendaAsync(vendaId, lojaId);

        return MapearResponse(venda);
    }

    public async Task<List<VendaResponse>> ListarAsync(int lojaId) {
        var vendas = await _context.Vendas
            .AsNoTracking()
            .Include(x => x.Pagamentos)
            .Include(x => x.Pedidos)
            .Where(x => x.LojaId == lojaId)
            .OrderByDescending(x => x.DataVenda)
            .ToListAsync();

        return vendas.Select(MapearResponse).ToList();
    }

    public async Task<List<VendaResponse>> ListarPorPeriodoAsync(int lojaId, DateTime inicio, DateTime fim) {
        var vendas = await _context.Vendas
            .AsNoTracking()
            .Include(x => x.Pagamentos)
            .Include(x => x.Pedidos)
            .Where(x => x.LojaId == lojaId &&
                        x.DataVenda >= inicio &&
                        x.DataVenda <= fim)
            .OrderByDescending(x => x.DataVenda)
            .ToListAsync();

        return vendas.Select(MapearResponse).ToList();
    }

    public async Task<List<VendaResponse>> ListarPorPedidoAsync(int lojaId, int pedidoId) {
        var vendas = await _context.Vendas
            .AsNoTracking()
            .Include(x => x.Pagamentos)
            .Include(x => x.Pedidos)
            .Where(x => x.LojaId == lojaId && x.Pedidos.Any(p => p.Id == pedidoId))
            .OrderByDescending(x => x.DataVenda)
            .ToListAsync();

        return vendas.Select(MapearResponse).ToList();
    }

    public async Task<List<VendaResponse>> ListarPorMesaAsync(int lojaId, int mesaId) {
        var vendas = await _context.Vendas
            .AsNoTracking()
            .Include(x => x.Pagamentos)
            .Include(x => x.Pedidos)
            .Where(x => x.LojaId == lojaId && x.Pedidos.Any(p => p.MesaId == mesaId))
            .OrderByDescending(x => x.DataVenda)
            .ToListAsync();

        return vendas.Select(MapearResponse).ToList();
    }

    public async Task<VendaResponse> CancelarVendaAsync(int vendaId, int lojaId) {
        var venda = await BuscarVendaAsync(vendaId, lojaId);

        if(venda.StatusPagamento == StatusPagamento.Cancelado)
            throw new BusinessLogicException("A venda já está cancelada.");

        if(venda.StatusPagamento == StatusPagamento.Estornado)
            throw new BusinessLogicException("A venda já está estornada.");

        if(venda.StatusPagamento != StatusPagamento.Confirmado)
            throw new BusinessLogicException("Somente vendas confirmadas podem ser canceladas.");

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            foreach(var pedido in venda.Pedidos)
            {
                await _estoqueServices.DevolverEstoqueProduto(pedido);
                pedido.Status = StatusPedido.Cancelado;
            }

            venda.StatusPagamento = StatusPagamento.Cancelado;

            await LiberarMesaAsync(venda.Pedidos.ToList());

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return MapearResponse(venda);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<VendaResponse> EstornarVendaAsync(int vendaId, int lojaId) {
        var venda = await BuscarVendaAsync(vendaId, lojaId);

        if(venda.StatusPagamento == StatusPagamento.Cancelado)
            throw new BusinessLogicException("A venda está cancelada.");

        if(venda.StatusPagamento == StatusPagamento.Estornado)
            throw new BusinessLogicException("A venda já está estornada.");

        if(venda.StatusPagamento != StatusPagamento.Confirmado)
            throw new BusinessLogicException("Somente vendas confirmadas podem ser estornadas.");

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            foreach(var pedido in venda.Pedidos)
            {
                await _estoqueServices.DevolverEstoqueProduto(pedido);
                pedido.Status = StatusPedido.Cancelado;
            }

            venda.StatusPagamento = StatusPagamento.Estornado;

            await LiberarMesaAsync(venda.Pedidos.ToList());

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return MapearResponse(venda);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<decimal> CalcularTotalAsync(int lojaId, ConfirmarPagamentoRequest request) {
        if(request == null)
            throw new BusinessLogicException("Os dados da venda são obrigatórios.");

        if(!request.PedidoId.HasValue && !request.MesaId.HasValue)
            throw new BusinessLogicException("Informe o pedido ou a mesa.");

        if(request.MesaId.HasValue)
        {
            var pedidos = await BuscarPedidosMesaAsync(lojaId, request.MesaId.Value);

            ValidarPedidos(pedidos);

            return pedidos.Sum(x => x.Total);
        }

        var pedido = await BuscarPedidoAsync(lojaId, request.PedidoId.Value);

        ValidarPedidos([ pedido ]);

        return pedido.Total;
    }

    private async Task<Pedido> BuscarPedidoAsync(int lojaId, int pedidoId) {
        var pedido = await _context.Pedidos
            .Include(x => x.Itens)
            .ThenInclude(x => x.Produto)
            .FirstOrDefaultAsync(x => x.Id == pedidoId && x.LojaId == lojaId);

        if(pedido == null)
            throw new BusinessLogicException("Pedido não encontrado.");

        return pedido;
    }

    private async Task<List<Pedido>> BuscarPedidosMesaAsync(int lojaId, int mesaId) {
        var pedidos = await _context.Pedidos
            .Include(x => x.Itens)
            .ThenInclude(x => x.Produto)
            .Where(x => x.LojaId == lojaId &&
                        x.MesaId == mesaId &&
                        x.Status == StatusPedido.AguardandoPagamento)
            .OrderBy(x => x.Id)
            .ToListAsync();

        if(!pedidos.Any())
            throw new BusinessLogicException("Não existem pedidos aguardando pagamento para esta mesa.");

        return pedidos;
    }

    private void ValidarPedidos(List<Pedido> pedidos) {
        if(pedidos == null || !pedidos.Any())
            throw new BusinessLogicException("Nenhum pedido encontrado para finalizar a venda.");

        foreach(var pedido in pedidos)
        {
            if(!pedido.Itens.Any())
                throw new BusinessLogicException($"O pedido {pedido.Id} não possui itens.");

            if(pedido.Status != StatusPedido.AguardandoPagamento)
                throw new BusinessLogicException($"O pedido {pedido.Id} não está aguardando pagamento.");

            if(pedido.Total <= 0)
                throw new BusinessLogicException($"O pedido {pedido.Id} possui valor inválido.");
        }
    } 

    private void ValidarPagamentos(List<PagamentoRequest> pagamentos, decimal valorTotal) {
        foreach(var pagamento in pagamentos)
        {
            if(pagamento.FormaPagamentoId <= 0)
                throw new BusinessLogicException("Forma de pagamento inválida.");

            if(pagamento.Valor <= 0)
                throw new BusinessLogicException("O valor do pagamento deve ser maior que zero.");
        }

        var totalPago = pagamentos.Sum(x => x.Valor);

        if(totalPago < valorTotal)
            throw new BusinessLogicException($"Valor pago insuficiente. Total da venda: {valorTotal:C}. Total informado: {totalPago:C}.");
    }

    private async Task LiberarMesaAsync(List<Pedido> pedidos) {
        var mesaIds = pedidos
            .Where(x => x.MesaId.HasValue)
            .Select(x => x.MesaId!.Value)
            .Distinct()
            .ToList();

        foreach(var mesaId in mesaIds)
        {
            var possuiPedidoAtivo = await _context.Pedidos.AnyAsync(x =>
                x.MesaId == mesaId &&
                x.LojaId == pedidos.First().LojaId &&
                x.Status != StatusPedido.Finalizado &&
                x.Status != StatusPedido.Cancelado);

            if(possuiPedidoAtivo)
                continue;

            var mesa = await _context.Mesas
                .FirstOrDefaultAsync(x => x.Id == mesaId && x.LojaId == pedidos.First().LojaId);

            if(mesa != null)
                mesa.StatusMesa = StatusMesa.Livre;
        }
    }

    private async Task<Venda> BuscarVendaAsync(int vendaId, int lojaId) {
        var venda = await _context.Vendas
            .Include(x => x.Pedidos)
            .ThenInclude(x => x.Itens)
            .ThenInclude(x => x.Produto)
            .Include(x => x.Pagamentos)
            .FirstOrDefaultAsync(x => x.Id == vendaId && x.LojaId == lojaId);

        if(venda == null)
            throw new BusinessLogicException("Venda não encontrada.");

        return venda;
    }

    private VendaResponse MapearResponse(Venda venda) {
        return new VendaResponse
        {
            Id = venda.Id,
            LojaId = venda.LojaId,
            ValorBruto = venda.ValorBruto,
            Desconto = venda.Desconto,
            Acrescimo = venda.Acrescimo,
            ValorTotal = venda.ValorTotal,
            DataVenda = venda.DataVenda,
            FuncionarioId = venda.FuncionarioId,
            StatusPagamento = venda.StatusPagamento,
            Pedidos = venda.Pedidos.Select(x => x.Id).ToList(),
            Pagamentos = venda.Pagamentos.Select(x => new PagamentoVendaResponse
            {
                Id = x.Id,
                FormaPagamentoId = x.FormaPagamentoId,
                Valor = x.Valor,
                Troco = x.Troco
            }).ToList()
        };
    }
}
