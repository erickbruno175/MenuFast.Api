
using AutoMapper;
using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Application.Services.EstoqueServices;
using MenuFast.Api.Api.Application.Services.KdsServices;
using MenuFast.Api.Api.Application.Services.Services.OpenRouteService;
using MenuFast.Api.Api.Domain.Entities.Models.Cardapio;
using MenuFast.Api.Api.Domain.Entities.Models.Cliente;
using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesLoja;
using MenuFast.Api.Api.Domain.Entities.Models.Mesa;
using MenuFast.Api.Api.Domain.Entities.Models.Pedido;
using MenuFast.Api.Api.Domain.Enum;
using MenuFast.Api.Api.Persistence.Context;
using MenuFast.Api.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace MenuFast.Api.Api.Application.Services.PedidoServices;

public class PedidoService {
    private readonly MenuFastContext _context;
    private readonly KdsService _kdsService;
    private readonly IMapper _mapper;
    private readonly OpenRouteServices _openRouteServices;
    private readonly EstoqueServices.EstoqueServices _estoqueServices;

    public PedidoService(MenuFastContext context, KdsService kdsService, IMapper mapper, OpenRouteServices openRouteServices, EstoqueServices.EstoqueServices estoqueServices) {
        _context = context;
        _kdsService = kdsService;
        _mapper = mapper;
        _openRouteServices = openRouteServices;
        _estoqueServices = estoqueServices;
    }

    public async Task<PedidoResponse> CriarPedidoAsync(CriarPedidoRequest request, int lojaId, int funcionarioId) {
        if(request.Itens == null || request.Itens.Count == 0)
            throw new BusinessLogicException("O pedido deve possuir pelo menos um item.");

        var configuracaoLoja = await _context.ConfiguracoesLoja.FirstOrDefaultAsync(x => x.LojaId == lojaId);

        decimal taxaEntrega = 0;
        Mesa? mesa = null;

        if(request.TipoPedido == TipoPedido.Mesa)
        {
            if(!request.MesaId.HasValue)
                throw new BusinessLogicException("A mesa é obrigatória para pedido de mesa.");

            mesa = await _context.Mesas.FirstOrDefaultAsync(x => x.Id == request.MesaId.Value && x.LojaId == lojaId);

            if(mesa == null)
                throw new BusinessLogicException("Mesa não encontrada.");

            if(mesa.StatusMesa == StatusMesa.Bloqueada)
                throw new BusinessLogicException("A mesa está bloqueada.");
        }

        if(request.TipoPedido == TipoPedido.Delivery)
        {
            if(configuracaoLoja == null || !configuracaoLoja.TrabalhaComDelivery)
                throw new BusinessLogicException("Esta loja não trabalha com delivery.");

            if(!request.ClienteId.HasValue)
                throw new BusinessLogicException("O cliente é obrigatório para delivery.");

            var cliente = await _context.Clientes.FirstOrDefaultAsync(x => x.Id == request.ClienteId.Value && x.LojaId == lojaId);

            if(cliente == null)
                throw new BusinessLogicException("Cliente não encontrado.");

            var resultadoTaxa = await CalcularTaxaEntregaAsync(lojaId, cliente, configuracaoLoja);
            taxaEntrega = resultadoTaxa.Taxa;
        }

        var produtoIds = request.Itens.Select(x => x.ProdutoId).Distinct().ToList();

        var produtos = await _context.Produtos
            .Where(x => produtoIds.Contains(x.Id) && x.LojaId == lojaId && x.Ativo)
            .ToListAsync();

        if(produtos.Count != produtoIds.Count)
            throw new BusinessLogicException("Um ou mais produtos não foram encontrados ou estão inativos.");

        var pedido = new Pedido
        {
            LojaId = lojaId,
            MesaId = request.MesaId,
            ClienteId = request.ClienteId,
            FuncionarioId = funcionarioId,
            TipoPedido = request.TipoPedido,
            Status = StatusPedido.Aberto,
            DataPedidoHora = DateTime.Now,
            Observacao = request.Observacao,
            Subtotal = 0,
            Desconto = 0,
            TaxaServico = 0,
            TaxaEntrega = taxaEntrega,
            Total = 0
        };

        foreach(var itemRequest in request.Itens)
            AdicionarItemAoPedido(pedido, itemRequest, produtos);

        RecalcularPedido(pedido);

        if(mesa != null)
            mesa.StatusMesa = StatusMesa.Ocupada;

        await _context.Pedidos.AddAsync(pedido);
        await _context.SaveChangesAsync();

        return await BuscarPorIdAsync(pedido.Id, lojaId);
    }

    public async Task<PedidoResponse> AdicionarItensAsync(int pedidoId, AdicionarItensPedidoRequest request, int lojaId) {
        if(request.Itens == null || request.Itens.Count == 0)
            throw new BusinessLogicException("Informe pelo menos um item.");

        var pedido = await BuscarPedidoAsync(pedidoId, lojaId);

        ValidarPedidoAberto(pedido);

        var produtoIds = request.Itens.Select(x => x.ProdutoId).Distinct().ToList();

        var produtos = await _context.Produtos
            .Where(x => produtoIds.Contains(x.Id) && x.LojaId == lojaId && x.Ativo)
            .ToListAsync();

        if(produtos.Count != produtoIds.Count)
            throw new BusinessLogicException("Um ou mais produtos não foram encontrados ou estão inativos.");

        foreach(var itemRequest in request.Itens)
            AdicionarItemAoPedido(pedido, itemRequest, produtos);

        RecalcularPedido(pedido);

        await _context.SaveChangesAsync();

        return await BuscarPorIdAsync(pedidoId, lojaId);
    }

    public async Task<PedidoResponse> AlterarQuantidadeItemAsync(int pedidoId, int itemId, AlterarQuantidadeItemPedidoRequest request, int lojaId) {
        if(request.Quantidade <= 0)
            throw new BusinessLogicException("A quantidade deve ser maior que zero.");

        var pedido = await BuscarPedidoAsync(pedidoId, lojaId);

        ValidarPedidoAberto(pedido);

        var item = pedido.Itens.FirstOrDefault(x => x.Id == itemId);

        if(item == null)
            throw new BusinessLogicException("Item não encontrado.");

        item.Quantidade = request.Quantidade;
        item.Total = (item.ValorUnitario * item.Quantidade) - item.Desconto;

        if(item.Total < 0)
            item.Total = 0;

        RecalcularPedido(pedido);

        await _context.SaveChangesAsync();

        return await BuscarPorIdAsync(pedidoId, lojaId);
    }

    public async Task<PedidoResponse> RemoverItemAsync(int pedidoId, int itemId, int lojaId) {
        var pedido = await BuscarPedidoAsync(pedidoId, lojaId);

        ValidarPedidoAberto(pedido);

        var item = pedido.Itens.FirstOrDefault(x => x.Id == itemId);

        if(item == null)
            throw new BusinessLogicException("Item não encontrado.");

        _context.ItensPedido.Remove(item);
        pedido.Itens.Remove(item);

        RecalcularPedido(pedido);

        await _context.SaveChangesAsync();

        return await BuscarPorIdAsync(pedidoId, lojaId);
    }

    public async Task<PedidoProducaoResponse> EnviarPedidoAsync(int pedidoId, int lojaId) {
        var pedido = await BuscarPedidoAsync(pedidoId, lojaId);

        ValidarPedidoAberto(pedido);

        if(!pedido.Itens.Any())
            throw new BusinessLogicException("Não é possível enviar um pedido sem itens.");

        RecalcularPedido(pedido);
        pedido.Status = StatusPedido.Enviado;

        await _context.SaveChangesAsync();

        var pedidoProducao = MontarPedidoProducao(pedido);

        await _kdsService.EnviarPedidoAsync(pedidoProducao);

        return pedidoProducao;
    }

    public async Task<PedidoProducaoResponse> IniciarProducaoAsync(int pedidoId, int lojaId) {
        var pedido = await BuscarPedidoAsync(pedidoId, lojaId);

        if(pedido.Status != StatusPedido.Enviado)
            throw new BusinessLogicException("O pedido precisa estar enviado para iniciar a produção.");

        pedido.Status = StatusPedido.EmProducao;

        await _context.SaveChangesAsync();

        var pedidoProducao = _mapper.Map<PedidoProducaoResponse>(pedido);

        await _kdsService.AtualizarStatusAsync(pedido.LojaId, pedido.Id, pedido.Status);

        return pedidoProducao;
    }

    public async Task<PedidoProducaoResponse> FinalizarProducaoAsync(int pedidoId, int lojaId) {
        var pedido = await BuscarPedidoAsync(pedidoId, lojaId);

        if(pedido.Status != StatusPedido.EmProducao)
            throw new BusinessLogicException("O pedido precisa estar em produção para ser finalizado.");

        pedido.Status = StatusPedido.Pronto;

        await _context.SaveChangesAsync();

        var pedidoProducao = _mapper.Map<PedidoProducaoResponse>(pedido);

        await _kdsService.AtualizarStatusAsync(pedido.LojaId, pedido.Id, pedido.Status);

        return pedidoProducao;
    }

    public async Task<PedidoResponse> CancelarAsync(int pedidoId, int lojaId) {
        var pedido = await BuscarPedidoAsync(pedidoId, lojaId);

        if(pedido.Status == StatusPedido.Cancelado)
            throw new BusinessLogicException("O pedido já está cancelado.");

        if(pedido.Status == StatusPedido.Finalizado)
            throw new BusinessLogicException("Não é possível cancelar um pedido finalizado.");

        pedido.Status = StatusPedido.Cancelado;

        await VerificarLiberacaoMesaAsync(pedido);
        await _context.SaveChangesAsync();

        return await BuscarPorIdAsync(pedidoId, lojaId);
    }

    public async Task<List<PedidoResponse>> IniciarFechamentoAsync(int lojaId, int? pedidoId = null, int? mesaId = null) {
        List<Pedido> pedidos;

        if(pedidoId.HasValue)
        {
            var pedido = await BuscarPedidoAsync(pedidoId.Value, lojaId);

            pedidos = [ pedido ];
        }
        else if(mesaId.HasValue)
        {
            var mesaExiste = await _context.Mesas.AnyAsync(x => x.Id == mesaId && x.LojaId == lojaId);

            if(!mesaExiste)
                throw new BusinessLogicException("Mesa não encontrada.");

            pedidos = await _context.Pedidos
                .Include(x => x.Itens)
                .ThenInclude(x => x.Produto)
                .Where(x => x.MesaId == mesaId && x.LojaId == lojaId &&
                            x.Status != StatusPedido.Finalizado &&
                            x.Status != StatusPedido.Cancelado)
                .OrderBy(x => x.DataPedidoHora)
                .ToListAsync();

            if(!pedidos.Any())
                throw new BusinessLogicException("Não existem pedidos ativos para esta mesa.");
        }
        else
        {
            throw new BusinessLogicException("Informe o pedido ou a mesa para iniciar o fechamento.");
        }

        foreach(var pedido in pedidos)
        {
            if(!pedido.Itens.Any())
                throw new BusinessLogicException($"O pedido {pedido.Id} não possui itens.");

            if(pedido.Status != StatusPedido.Pronto && pedido.Status != StatusPedido.AguardandoPagamento)
                throw new BusinessLogicException($"O pedido {pedido.Id} ainda não está pronto para fechamento.");

            pedido.Status = StatusPedido.AguardandoPagamento;
        }

        await _context.SaveChangesAsync();

        return pedidos.Select(MapearResponse).ToList();
    }

    public async Task<PedidoResponse> BuscarPorIdAsync(int pedidoId, int lojaId) {
        var pedido = await _context.Pedidos
            .Include(x => x.Itens)
            .ThenInclude(p => p.Produto)
            .FirstOrDefaultAsync(x => x.Id == pedidoId && x.LojaId == lojaId);

        if(pedido == null)
            throw new BusinessLogicException("Pedido não encontrado.");

        return MapearResponse(pedido);
    }

    public async Task<List<PedidoResponse>> ListarPorMesaAsync(int mesaId, int lojaId) {
        var mesaExiste = await _context.Mesas.AnyAsync(x => x.Id == mesaId && x.LojaId == lojaId);

        if(!mesaExiste)
            throw new BusinessLogicException("Mesa não encontrada.");

        var pedidos = await _context.Pedidos
            .Include(x => x.Itens)
            .ThenInclude(p => p.Produto)
            .Where(x => x.MesaId == mesaId && x.LojaId == lojaId)
            .OrderBy(x => x.DataPedidoHora)
            .ToListAsync();

        return pedidos.Select(MapearResponse).ToList();
    }

    public async Task<List<PedidoResponse>> ListarAsync(int lojaId) {
        var pedidos = await _context.Pedidos
            .Include(x => x.Itens)
            .ThenInclude(p => p.Produto)
            .Where(x => x.LojaId == lojaId)
            .OrderByDescending(x => x.DataPedidoHora)
            .ToListAsync();

        return pedidos.Select(MapearResponse).ToList();
    }

    public async Task<List<PedidoResponse>> ListarPorStatusAsync(int lojaId, StatusPedido status) {
        var pedidos = await _context.Pedidos
            .Include(x => x.Itens)
            .ThenInclude(p => p.Produto)
            .Where(x => x.LojaId == lojaId && x.Status == status)
            .OrderBy(x => x.DataPedidoHora)
            .ToListAsync();

        return pedidos.Select(MapearResponse).ToList();
    }

    public async Task<List<PedidoResponse>> ListarPedidosAbertosMesaAsync(int mesaId, int lojaId) {
        var pedidos = await _context.Pedidos
            .Include(x => x.Itens)
            .ThenInclude(p => p.Produto)
            .Where(x => x.MesaId == mesaId && x.LojaId == lojaId && x.Status == StatusPedido.Aberto)
            .OrderBy(x => x.DataPedidoHora)
            .ToListAsync();

        return pedidos.Select(MapearResponse).ToList();
    }

    public async Task<List<PedidoResponse>> ListarPedidosAtivosMesaAsync(int mesaId, int lojaId) {
        var pedidos = await _context.Pedidos
            .Include(x => x.Itens)
            .ThenInclude(p => p.Produto)
            .Where(x => x.MesaId == mesaId && x.LojaId == lojaId && x.Status != StatusPedido.Finalizado && x.Status != StatusPedido.Cancelado)
            .OrderBy(x => x.DataPedidoHora)
            .ToListAsync();

        return pedidos.Select(MapearResponse).ToList();
    }

    public async Task<decimal> CalcularTotalMesaAsync(int mesaId, int lojaId) {
        var mesaExiste = await _context.Mesas.AnyAsync(x => x.Id == mesaId && x.LojaId == lojaId);

        if(!mesaExiste)
            throw new BusinessLogicException("Mesa não encontrada.");

        return await _context.Pedidos
            .Where(x => x.MesaId == mesaId && x.LojaId == lojaId && x.Status != StatusPedido.Cancelado)
            .SumAsync(x => x.Total);
    }

    public async Task<decimal> CalcularTotalMesaAtivaAsync(int mesaId, int lojaId) {
        var mesaExiste = await _context.Mesas.AnyAsync(x => x.Id == mesaId && x.LojaId == lojaId);

        if(!mesaExiste)
            throw new BusinessLogicException("Mesa não encontrada.");

        return await _context.Pedidos
            .Where(x => x.MesaId == mesaId && x.LojaId == lojaId && x.Status != StatusPedido.Finalizado && x.Status != StatusPedido.Cancelado)
            .SumAsync(x => x.Total);
    }

    public async Task<decimal> CalcularTotalClienteAsync(int clienteId, int lojaId) {
        var clienteExiste = await _context.Clientes.AnyAsync(x => x.Id == clienteId && x.LojaId == lojaId);

        if(!clienteExiste)
            throw new BusinessLogicException("Cliente não encontrado.");

        return await _context.Pedidos
            .Where(x => x.ClienteId == clienteId && x.LojaId == lojaId && x.Status != StatusPedido.Cancelado)
            .SumAsync(x => x.Total);
    }

    public async Task<decimal> CalcularTotalClienteAtivoAsync(int clienteId, int lojaId) {
        var clienteExiste = await _context.Clientes.AnyAsync(x => x.Id == clienteId && x.LojaId == lojaId);

        if(!clienteExiste)
            throw new BusinessLogicException("Cliente não encontrado.");

        return await _context.Pedidos
            .Where(x => x.ClienteId == clienteId && x.LojaId == lojaId && x.Status != StatusPedido.Finalizado && x.Status != StatusPedido.Cancelado)
            .SumAsync(x => x.Total);
    }

    private async Task<Pedido> BuscarPedidoAsync(int pedidoId, int lojaId) {
        var pedido = await _context.Pedidos
            .Include(x => x.Itens)
            .ThenInclude(x => x.Produto)
            .FirstOrDefaultAsync(x => x.Id == pedidoId && x.LojaId == lojaId);

        if(pedido == null)
            throw new BusinessLogicException("Pedido não encontrado.");

        return pedido;
    }

    private static void ValidarPedidoAberto(Pedido pedido) {
        if(pedido.Status != StatusPedido.Aberto)
            throw new BusinessLogicException("Essa operação só pode ser realizada em um pedido aberto.");
    }

    private static void AdicionarItemAoPedido(Pedido pedido, ItemPedidoRequest itemRequest, List<Produto> produtos) {
        if(itemRequest.Quantidade <= 0)
            throw new BusinessLogicException("A quantidade do produto deve ser maior que zero.");

        var produto = produtos.FirstOrDefault(x => x.Id == itemRequest.ProdutoId);

        if(produto == null)
            throw new BusinessLogicException("Produto não encontrado.");

        var valorUnitario = produto.Preco;
        var subtotalItem = valorUnitario * itemRequest.Quantidade;
        var desconto = itemRequest.Desconto;

        if(desconto < 0)
            desconto = 0;

        if(desconto > subtotalItem)
            desconto = subtotalItem;

        var totalItem = subtotalItem - desconto;

        pedido.Itens.Add(new ItemPedido
        {
            ProdutoId = produto.Id,
            Quantidade = itemRequest.Quantidade,
            ValorUnitario = valorUnitario,
            Desconto = desconto,
            Total = totalItem,
            Observacao = itemRequest.Observacao
        });
    }

    private static void RecalcularPedido(Pedido pedido) {
        pedido.Subtotal = pedido.Itens.Sum(x => x.ValorUnitario * x.Quantidade);
        pedido.Desconto = pedido.Itens.Sum(x => x.Desconto);
        pedido.Total = pedido.Subtotal - pedido.Desconto + pedido.TaxaServico + pedido.TaxaEntrega;

        if(pedido.Total < 0)
            pedido.Total = 0;
    }

    private async Task<(decimal Taxa, decimal DistanciaKm)> CalcularTaxaEntregaAsync(int lojaId, Cliente cliente, ConfiguracaoLoja configuracao) {
        if(!configuracao.CobraTaxaEntrega)
            return (0, 0);

        if(configuracao.TipoTaxaEntrega == TipoTaxaEntrega.Fixa)
        {
            var taxaFixa = configuracao.TaxaEntrega ?? 0;
            return (Math.Round(taxaFixa, 2), 0);
        }

        if(configuracao.TipoTaxaEntrega == TipoTaxaEntrega.PorDistancia)
        {
            var loja = await _context.Lojas.FirstOrDefaultAsync(x => x.Id == lojaId);

            if(loja == null)
                throw new BusinessLogicException("Loja não encontrada.");

            if(!loja.Latitude.HasValue || !loja.Longitude.HasValue)
                throw new BusinessLogicException("A localização da loja não está cadastrada.");

            if(!cliente.Latitude.HasValue || !cliente.Longitude.HasValue)
                throw new BusinessLogicException("A localização do cliente não está cadastrada.");

            var distanciaKm = await _openRouteServices.CalcularDistanciaAsync(
                loja.Latitude.Value,
                loja.Longitude.Value,
                cliente.Latitude.Value,
                cliente.Longitude.Value);

            if(configuracao.DistanciaMaximaEntregaKm.HasValue && distanciaKm > configuracao.DistanciaMaximaEntregaKm.Value)
                throw new BusinessLogicException($"O endereço está fora da área de entrega. Distância máxima: {configuracao.DistanciaMaximaEntregaKm.Value:N2} km.");

            var taxaBase = configuracao.TaxaBaseEntrega ?? 0;
            var valorPorKm = configuracao.ValorPorKm ?? 0;
            var taxa = taxaBase + distanciaKm * valorPorKm;

            return (Math.Round((decimal)taxa, 2), Math.Round((decimal)distanciaKm, 2));
        }

        throw new BusinessLogicException("Tipo de taxa de entrega inválido.");
    }

    private async Task VerificarLiberacaoMesaAsync(Pedido pedido) {
        if(!pedido.MesaId.HasValue)
            return;

        var existeOutroPedidoAtivo = await _context.Pedidos.AnyAsync(x =>
            x.Id != pedido.Id &&
            x.MesaId == pedido.MesaId &&
            x.LojaId == pedido.LojaId &&
            x.Status != StatusPedido.Finalizado &&
            x.Status != StatusPedido.Cancelado);

        if(existeOutroPedidoAtivo)
            return;

        var mesa = await _context.Mesas.FirstOrDefaultAsync(x => x.Id == pedido.MesaId.Value && x.LojaId == pedido.LojaId);

        if(mesa != null)
            mesa.StatusMesa = StatusMesa.Livre;
    }

    private static PedidoProducaoResponse MontarPedidoProducao(Pedido pedido) {
        return new PedidoProducaoResponse
        {
            PedidoId = pedido.Id,
            LojaId = pedido.LojaId,
            MesaId = pedido.MesaId,
            TipoPedido = pedido.TipoPedido,
            DataPedidoHora = pedido.DataPedidoHora,
            Observacao = pedido.Observacao,
            Itens = pedido.Itens
                .Where(p => p.Produto != null && p.Produto.EnviaParaProducao)
                .Select(x => new ItemPedidoProducaoResponse
                {
                    ItemPedidoId = x.Id,
                    ProdutoId = x.ProdutoId,
                    Quantidade = x.Quantidade,
                    Observacao = x.Observacao,
                    NomeProduto = x.Produto.Nome
                })
                .ToList()
        };
    }

    public async Task TransferirPedidosMesaAsync(int mesaOrigemId, int mesaDestinoId, int lojaId) {
        if(mesaOrigemId == mesaDestinoId)
            throw new BusinessLogicException("A mesa de origem e destino não podem ser a mesma.");
        var mesaOrigem = await _context.Mesas.FirstOrDefaultAsync(x => x.Id == mesaOrigemId && x.LojaId == lojaId);
        var mesaDestino = await _context.Mesas.FirstOrDefaultAsync(x => x.Id == mesaDestinoId && x.LojaId == lojaId);
        if(mesaOrigem == null)
            throw new BusinessLogicException("Mesa de origem não encontrada.");
        if(mesaDestino == null)
            throw new BusinessLogicException("Mesa de destino não encontrada.");
        var pedidosAtivos = await _context.Pedidos
            .Where(x => x.MesaId == mesaOrigemId && x.LojaId == lojaId &&
                        x.Status != StatusPedido.Finalizado && x.Status != StatusPedido.Cancelado)
            .ToListAsync();
        if(!pedidosAtivos.Any())
            throw new BusinessLogicException("Não existem pedidos ativos na mesa de origem.");
        foreach(var pedido in pedidosAtivos)
            pedido.MesaId = mesaDestinoId;
        mesaDestino.StatusMesa = StatusMesa.Ocupada;
        var existeOutroPedidoAtivoNaOrigem = await _context.Pedidos.AnyAsync(x =>
            x.MesaId == mesaOrigemId && x.LojaId == lojaId &&
            x.Status != StatusPedido.Finalizado && x.Status != StatusPedido.Cancelado);
        if(!existeOutroPedidoAtivoNaOrigem)
            mesaOrigem.StatusMesa = StatusMesa.Livre;
        await _context.SaveChangesAsync();
    }

    private static PedidoResponse MapearResponse(Pedido pedido) {
        return new PedidoResponse
        {
            Id = pedido.Id,
            LojaId = pedido.LojaId,
            MesaId = pedido.MesaId,
            ClienteId = pedido.ClienteId,
            FuncionarioId = pedido.FuncionarioId,
            Status = pedido.Status,
            TipoPedido = pedido.TipoPedido,
            DataPedidoHora = pedido.DataPedidoHora,
            Observacao = pedido.Observacao,
            Subtotal = pedido.Subtotal,
            Desconto = pedido.Desconto,
            TaxaServico = pedido.TaxaServico,
            TaxaEntrega = pedido.TaxaEntrega,
            Total = pedido.Total,
            Itens = pedido.Itens.Select(x => new ItemPedidoResponse
            {
                Id = x.Id,
                ProdutoId = x.ProdutoId,
                Quantidade = x.Quantidade,
                ValorUnitario = x.ValorUnitario,
                Desconto = x.Desconto,
                Total = x.Total,
                Observacao = x.Observacao,
                Nome = x.Produto.Nome
            }).ToList()
        };
    }
}

