using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Application.Services.KdsServices;
using MenuFast.Api.Api.Domain.Entities.Models.Cardapio;
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

    public PedidoService(MenuFastContext context, KdsService kdsService) {
        _context = context;
        _kdsService = kdsService;
    }

    // 1. CRIAR PEDIDO
    public async Task<PedidoResponse> CriarPedidoAsync(CriarPedidoRequest request, int lojaId, int funcionarioId) {
        if(request.Itens == null || request.Itens.Count == 0)
            throw new BusinessLogicException("O pedido deve possuir pelo menos um item.");

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
            if(!request.ClienteId.HasValue)
                throw new BusinessLogicException("O cliente é obrigatório para delivery.");

            var clienteExiste = await _context.Clientes.AnyAsync(x => x.Id == request.ClienteId.Value && x.LojaId == lojaId);

            if(!clienteExiste)
                throw new BusinessLogicException("Cliente não encontrado.");
        }

        var produtoIds = request.Itens.Select(x => x.ProdutoId).Distinct().ToList();

        var produtos = await _context.Produtos.Where(x => produtoIds.Contains(x.Id) && x.LojaId == lojaId && x.Ativo).ToListAsync();

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
            TaxaEntrega = 0,
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

    // 2. ADICIONAR ITENS AO PEDIDO ABERTO
    public async Task<PedidoResponse> AdicionarItensAsync(int pedidoId, AdicionarItensPedidoRequest request, int lojaId) {
        if(request.Itens == null || request.Itens.Count == 0)
            throw new BusinessLogicException("Informe pelo menos um item.");

        var pedido = await BuscarPedidoAsync(pedidoId, lojaId);

        ValidarPedidoAberto(pedido);

        var produtoIds = request.Itens.Select(x => x.ProdutoId).Distinct().ToList();

        var produtos = await _context.Produtos.Where(x => produtoIds.Contains(x.Id) && x.LojaId == lojaId && x.Ativo).ToListAsync();

        if(produtos.Count != produtoIds.Count)
            throw new BusinessLogicException("Um ou mais produtos não foram encontrados ou estão inativos.");

        foreach(var itemRequest in request.Itens)
            AdicionarItemAoPedido(pedido, itemRequest, produtos);

        RecalcularPedido(pedido);

        await _context.SaveChangesAsync();

        return await BuscarPorIdAsync(pedidoId, lojaId);
    }

    // 3. ALTERAR QUANTIDADE
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

    // 4. REMOVER ITEM
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

    // 5. ENVIAR PEDIDO
    public async Task<PedidoResponse> EnviarPedidoAsync(int pedidoId, int lojaId) {
        var pedido = await BuscarPedidoAsync(pedidoId, lojaId);

        ValidarPedidoAberto(pedido);

        if(!pedido.Itens.Any())
            throw new BusinessLogicException("Não é possível enviar um pedido sem itens.");

        RecalcularPedido(pedido);

        pedido.Status = StatusPedido.Enviado;

        await _context.SaveChangesAsync();

        var pedidoProducao = MontarPedidoProducao(pedido);

        await _kdsService.EnviarPedidoAsync(pedidoProducao);

        return MapearResponse(pedido);
    }

    // 6. CANCELAR PEDIDO
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

    // 7. FINALIZAR PEDIDO
    public async Task<PedidoResponse> FinalizarPedidoAsync(int pedidoId, int lojaId) {
        var pedido = await BuscarPedidoAsync(pedidoId, lojaId);

        if(pedido.Status == StatusPedido.Cancelado)
            throw new BusinessLogicException("Não é possível finalizar um pedido cancelado.");

        if(pedido.Status == StatusPedido.Finalizado)
            throw new BusinessLogicException("O pedido já está finalizado.");

        if(pedido.Status != StatusPedido.Enviado)
            throw new BusinessLogicException("Somente pedidos enviados podem ser finalizados.");

        if(!pedido.Itens.Any())
            throw new BusinessLogicException("Não é possível finalizar um pedido sem itens.");

        RecalcularPedido(pedido);

        pedido.Status = StatusPedido.Finalizado;

        await VerificarLiberacaoMesaAsync(pedido);

        await _context.SaveChangesAsync();

        return await BuscarPorIdAsync(pedidoId, lojaId);
    }

    // 8. BUSCAR PEDIDO POR ID
    public async Task<PedidoResponse> BuscarPorIdAsync(int pedidoId, int lojaId) {
        var pedido = await _context.Pedidos.Include(x => x.Itens).FirstOrDefaultAsync(x => x.Id == pedidoId && x.LojaId == lojaId);

        if(pedido == null)
            throw new BusinessLogicException("Pedido não encontrado.");

        return MapearResponse(pedido);
    }

    // 9. LISTAR PEDIDOS DA MESA
    public async Task<List<PedidoResponse>> ListarPorMesaAsync(int mesaId, int lojaId) {
        var mesaExiste = await _context.Mesas.AnyAsync(x => x.Id == mesaId && x.LojaId == lojaId);

        if(!mesaExiste)
            throw new BusinessLogicException("Mesa não encontrada.");

        var pedidos = await _context.Pedidos.Include(x => x.Itens).Where(x => x.MesaId == mesaId && x.LojaId == lojaId).OrderBy(x => x.DataPedidoHora).ToListAsync();

        return pedidos.Select(MapearResponse).ToList();
    }

    // 10. LISTAR PEDIDOS
    public async Task<List<PedidoResponse>> ListarAsync(int lojaId) {
        var pedidos = await _context.Pedidos.Include(x => x.Itens).Where(x => x.LojaId == lojaId).OrderByDescending(x => x.DataPedidoHora).ToListAsync();

        return pedidos.Select(MapearResponse).ToList();
    }

    // 11. LISTAR POR STATUS
    public async Task<List<PedidoResponse>> ListarPorStatusAsync(int lojaId, StatusPedido status) {
        var pedidos = await _context.Pedidos.Include(x => x.Itens).Where(x => x.LojaId == lojaId && x.Status == status).OrderBy(x => x.DataPedidoHora).ToListAsync();

        return pedidos.Select(MapearResponse).ToList();
    }

    // 12. LISTAR PEDIDOS ABERTOS DA MESA
    public async Task<List<PedidoResponse>> ListarPedidosAbertosMesaAsync(int mesaId, int lojaId) {
        var pedidos = await _context.Pedidos.Include(x => x.Itens).Where(x => x.MesaId == mesaId && x.LojaId == lojaId && x.Status == StatusPedido.Aberto).OrderBy(x => x.DataPedidoHora).ToListAsync();

        return pedidos.Select(MapearResponse).ToList();
    }

    // 13. LISTAR PEDIDOS ATIVOS DA MESA
    public async Task<List<PedidoResponse>> ListarPedidosAtivosMesaAsync(int mesaId, int lojaId) {
        var pedidos = await _context.Pedidos.Include(x => x.Itens).Where(x => x.MesaId == mesaId && x.LojaId == lojaId && x.Status != StatusPedido.Finalizado && x.Status != StatusPedido.Cancelado).OrderBy(x => x.DataPedidoHora).ToListAsync();

        return pedidos.Select(MapearResponse).ToList();
    }

    // 14. TOTAL DOS PEDIDOS DA MESA
    public async Task<decimal> CalcularTotalMesaAsync(int mesaId, int lojaId) {
        var mesaExiste = await _context.Mesas.AnyAsync(x => x.Id == mesaId && x.LojaId == lojaId);

        if(!mesaExiste)
            throw new BusinessLogicException("Mesa não encontrada.");

        return await _context.Pedidos.Where(x => x.MesaId == mesaId && x.LojaId == lojaId && x.Status != StatusPedido.Cancelado).SumAsync(x => x.Total);
    }

    // 15. TOTAL DOS PEDIDOS ATIVOS DA MESA
    public async Task<decimal> CalcularTotalMesaAtivaAsync(int mesaId, int lojaId) {
        var mesaExiste = await _context.Mesas.AnyAsync(x => x.Id == mesaId && x.LojaId == lojaId);

        if(!mesaExiste)
            throw new BusinessLogicException("Mesa não encontrada.");

        return await _context.Pedidos.Where(x => x.MesaId == mesaId && x.LojaId == lojaId && x.Status != StatusPedido.Finalizado && x.Status != StatusPedido.Cancelado).SumAsync(x => x.Total);
    }

    // 16. TOTAL DOS PEDIDOS DO CLIENTE
    public async Task<decimal> CalcularTotalClienteAsync(int clienteId, int lojaId) {
        var clienteExiste = await _context.Clientes.AnyAsync(x => x.Id == clienteId && x.LojaId == lojaId);

        if(!clienteExiste)
            throw new BusinessLogicException("Cliente não encontrado.");

        return await _context.Pedidos.Where(x => x.ClienteId == clienteId && x.LojaId == lojaId && x.Status != StatusPedido.Cancelado).SumAsync(x => x.Total);
    }

    // 17. TOTAL DOS PEDIDOS ATIVOS DO CLIENTE
    public async Task<decimal> CalcularTotalClienteAtivoAsync(int clienteId, int lojaId) {
        var clienteExiste = await _context.Clientes.AnyAsync(x => x.Id == clienteId && x.LojaId == lojaId);

        if(!clienteExiste)
            throw new BusinessLogicException("Cliente não encontrado.");

        return await _context.Pedidos.Where(x => x.ClienteId == clienteId && x.LojaId == lojaId && x.Status != StatusPedido.Finalizado && x.Status != StatusPedido.Cancelado).SumAsync(x => x.Total);
    }

    // 18. BUSCAR PEDIDO
    private async Task<Pedido> BuscarPedidoAsync(int pedidoId, int lojaId) {
        var pedido = await _context.Pedidos.Include(x => x.Itens).FirstOrDefaultAsync(x => x.Id == pedidoId && x.LojaId == lojaId);

        if(pedido == null)
            throw new BusinessLogicException("Pedido não encontrado.");

        return pedido;
    }

    // 19. VALIDAR PEDIDO ABERTO
    private static void ValidarPedidoAberto(Pedido pedido) {
        if(pedido.Status != StatusPedido.Aberto)
            throw new BusinessLogicException("Essa operação só pode ser realizada em um pedido aberto.");
    }

    // 20. ADICIONAR ITEM
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

    // 21. RECALCULAR PEDIDO
    private static void RecalcularPedido(Pedido pedido) {
        pedido.Subtotal = pedido.Itens.Sum(x => x.ValorUnitario * x.Quantidade);
        pedido.Desconto = pedido.Itens.Sum(x => x.Desconto);
        pedido.Total = pedido.Subtotal - pedido.Desconto + pedido.TaxaServico + pedido.TaxaEntrega;

        if(pedido.Total < 0)
            pedido.Total = 0;
    }

    // 22. LIBERAR MESA SOMENTE QUANDO NÃO EXISTIR PEDIDO ATIVO
    private async Task VerificarLiberacaoMesaAsync(Pedido pedido) {
        if(!pedido.MesaId.HasValue)
            return;

        var existeOutroPedidoAtivo = await _context.Pedidos.AnyAsync(x => x.Id != pedido.Id && x.MesaId == pedido.MesaId && x.LojaId == pedido.LojaId && x.Status != StatusPedido.Finalizado && x.Status != StatusPedido.Cancelado);

        if(existeOutroPedidoAtivo)
            return;

        var mesa = await _context.Mesas.FirstOrDefaultAsync(x => x.Id == pedido.MesaId.Value && x.LojaId == pedido.LojaId);

        if(mesa != null)
            mesa.StatusMesa = StatusMesa.Livre;
    }

    // 23. MONTAR PEDIDO PARA PRODUÇÃO
    private static PedidoProducaoResponse MontarPedidoProducao(Pedido pedido) {
        return new PedidoProducaoResponse
        {
            PedidoId = pedido.Id,
            LojaId = pedido.LojaId,
            MesaId = pedido.MesaId,
            TipoPedido = pedido.TipoPedido,
            DataPedidoHora = pedido.DataPedidoHora,
            Observacao = pedido.Observacao,
            Itens = pedido.Itens.Select(x => new ItemPedidoProducaoResponse
            {
                ItemPedidoId = x.Id,
                ProdutoId = x.ProdutoId,
                Quantidade = x.Quantidade,
                Observacao = x.Observacao
            }).ToList()
        };
    }

    // 24. MAPEAR RESPONSE
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
                Observacao = x.Observacao
            }).ToList()
        };
    }
}
