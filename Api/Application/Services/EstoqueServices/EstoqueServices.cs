using MenuFast.Api.Api.Domain.Entities.Models.Cardapio;
using MenuFast.Api.Api.Domain.Entities.Models.Pedido;
using MenuFast.Api.Api.Persistence.Context;
using MenuFast.Api.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace MenuFast.Api.Api.Application.Services.EstoqueServices;

public class EstoqueServices {
    private readonly MenuFastContext _context;

    public EstoqueServices(MenuFastContext context) {
        _context = context;
    }

    public async Task BaixaEstoqueProduto(Pedido pedido) {
        foreach(var item in pedido.Itens)
        {
            var pedidoProduto = await _context.Produtos
                .Include(p => p.EstoqueProduto)
                .FirstOrDefaultAsync(p => p.Id == item.ProdutoId && !p.EnviaParaProducao && p.LojaId == pedido.LojaId );

            if(pedidoProduto == null)
                throw new BusinessLogicException($"Produto {item.ProdutoId} não encontrado.");

            if(!pedidoProduto.ControlaEstoque)
                continue;

            if(pedidoProduto.EstoqueProduto == null)
                throw new BusinessLogicException($"O produto {pedidoProduto.Nome} não possui estoque cadastrado.");

            var quantidadeAnterior = pedidoProduto.EstoqueProduto.Quantidade;

            if(quantidadeAnterior < item.Quantidade)
            {
                throw new BusinessLogicException(
                    $"Estoque insuficiente para o produto {pedidoProduto.Nome}. " +
                    $"Quantidade em estoque: {quantidadeAnterior}, " +
                    $"quantidade solicitada: {item.Quantidade}."
                );
            }

            pedidoProduto.EstoqueProduto.Quantidade -= (int)item.Quantidade;

            var quantidadeAtual = pedidoProduto.EstoqueProduto.Quantidade;

            var movimentoEstoque = new MovimentacaoEstoque
            {
                DataCadastro = DateTime.Now,
                EstoqueProdutoId = pedidoProduto.EstoqueProduto.Id,
                Quantidade = (int)item.Quantidade,
                QuantidadeAnterior = quantidadeAnterior,
                QuantidadeAtual = quantidadeAtual,
                Tipo = Domain.Enum.TipoMovimentacaoEstoque.Saida,
                Observacao = $"Baixa de estoque do produto {pedidoProduto.Nome} referente ao pedido {pedido.Id}.",
                PedidoId = pedido.Id
            };

            _context.MovimentoEstoques.Add(movimentoEstoque);
        }
    }

    public async Task DevolverEstoqueProduto(Pedido pedido) {
        foreach(var item in pedido.Itens)
        {
            var pedidoProduto = await _context.Produtos
                .Include(p => p.EstoqueProduto)
                .FirstOrDefaultAsync(p => p.Id == item.ProdutoId && p.LojaId == pedido.LojaId);

            if(pedidoProduto == null)
                throw new BusinessLogicException($"Produto {item.ProdutoId} não encontrado.");

            if(!pedidoProduto.ControlaEstoque)
                continue;

            if(pedidoProduto.EstoqueProduto == null)
                throw new BusinessLogicException($"O produto {pedidoProduto.Nome} não possui estoque cadastrado.");

            var quantidadeAnterior = pedidoProduto.EstoqueProduto.Quantidade;

            pedidoProduto.EstoqueProduto.Quantidade += (int)item.Quantidade;

            var quantidadeAtual = pedidoProduto.EstoqueProduto.Quantidade;

            var movimentoEstoque = new MovimentacaoEstoque
            {
                DataCadastro = DateTime.Now,
                EstoqueProdutoId = pedidoProduto.EstoqueProduto.Id,
                Quantidade = (int)item.Quantidade,
                QuantidadeAnterior = quantidadeAnterior,
                QuantidadeAtual = quantidadeAtual,
                Tipo = Domain.Enum.TipoMovimentacaoEstoque.Entrada,
                Observacao = $"Devolução de estoque do produto {pedidoProduto.Nome} referente ao pedido {pedido.Id}.",
                PedidoId = pedido.Id
            };

            _context.MovimentoEstoques.Add(movimentoEstoque);
        }
    }
}
