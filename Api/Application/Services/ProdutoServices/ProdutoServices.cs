using MenuFast.Api.Api.Application.DTOs.Request;
using MenuFast.Api.Api.Application.DTOs.Response;
using MenuFast.Api.Api.Application.Services.Email;
using MenuFast.Api.Api.Application.Services.Seguranca;
using MenuFast.Api.Api.Domain.Constantes;
using MenuFast.Api.Api.Domain.Entities.Models.Cardapio;
using MenuFast.Api.Api.Domain.Entities.Models.Funcionario;
using MenuFast.Api.Api.Domain.Enum;
using MenuFast.Api.Api.Persistence.Context;
using MenuFast.Api.Api.Util.Helpers;
using MenuFast.Api.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Serilog.Core;

namespace MenuFast.Api.Api.Application.Services.ProdutoServices {
    public class ProdutoServices {
        private readonly MenuFastContext _menuFastContext;
        private readonly EmailService _emailService;
        public readonly ILogger<ProdutoServices> _logger;

        public ProdutoServices(
             MenuFastContext menuFastContext
            , EmailService emailService,
             ILogger<ProdutoServices> logger) { _menuFastContext = menuFastContext; _emailService = emailService;  _logger = _logger; }

        public async Task<DetalheProdutosResponse> CadastrarProduto(ProdutoRequest request) {
            var agora = DateTime.UtcNow;

            var produto = new Produto
            {
                CategoriaProdutoId = request.CategoriaProdutoId,
                LojaId = request.LojaId,
                FotoProduto = request.FotoProduto,
                Preco = request.Preco,
                Nome = request.Nome,
                DataCadastro = agora,
                Codigo = UtilHelper.GerarCodigoProduto(_menuFastContext),
                Descricao = request.Descricao,
                Ativo = request.Ativo,
                ControlaEstoque = request.ControlaEstoque
            };

            if(request.ControlaEstoque)
            {
                var estoque = new EstoqueProduto
                {
                    Quantidade = request.QuantidadeEstoque,
                    EstoqueMinimo = request.EstoqueMinimo,
                    DataCadastro = agora,
                    DataAtualizacao = agora
                };

                estoque.Movimentacoe.Add(new MovimentacaoEstoque
                {
                    Tipo = TipoMovimentacaoEstoque.Entrada,
                    Quantidade = request.QuantidadeEstoque,
                    QuantidadeAnterior = 0,
                    QuantidadeAtual = request.QuantidadeEstoque,
                    Observacao = "Estoque inicial",
                    DataCadastro = agora
                });
                produto.EstoqueProduto = estoque;
            }
            await _menuFastContext.Produtos.AddAsync(produto);
            await _menuFastContext.SaveChangesAsync();
            return ConverterParaDetalhe(produto);
        }
        public async Task<DetalheProdutosResponse> AtualizarProduto(int idProduto, ProdutoRequest request) {
            var produto = await _menuFastContext.Produtos
                .Include(p => p.EstoqueProduto)
                .ThenInclude(e => e.Movimentacoe)
                .FirstOrDefaultAsync(p => p.Id == idProduto);

            if(produto == null) throw new BusinessLogicException("Produto não encontrado");

            var agora = DateTime.UtcNow;

            // Dados do produto
            produto.Nome = request.Nome;
            produto.FotoProduto = request.FotoProduto;
            produto.Preco = request.Preco;
            produto.CategoriaProdutoId = request.CategoriaProdutoId;
            produto.Descricao = request.Descricao;
            produto.Ativo = request.Ativo;
            produto.ControlaEstoque = request.ControlaEstoque;

            // Ativou o controle de estoque
            if(request.ControlaEstoque)
            {
                // Produto ainda não possui estoque
                if(produto.EstoqueProduto == null)
                {
                    var estoque = new EstoqueProduto
                    {
                        ProdutoId = produto.Id,
                        Quantidade = request.QuantidadeEstoque,
                        EstoqueMinimo = request.EstoqueMinimo,
                        DataCadastro = agora,
                        DataAtualizacao = agora
                    };

                    estoque.Movimentacoe.Add(new MovimentacaoEstoque
                    {
                        Tipo = TipoMovimentacaoEstoque.Entrada,
                        Quantidade = request.QuantidadeEstoque,
                        QuantidadeAnterior = 0,
                        QuantidadeAtual = request.QuantidadeEstoque,
                        Observacao = "Estoque inicial",
                        DataCadastro = agora
                    });

                    produto.EstoqueProduto = estoque;
                }
                else
                {
                    var estoque = produto.EstoqueProduto;

                    estoque.EstoqueMinimo = request.EstoqueMinimo;

                    // Só cria movimentação se a quantidade realmente mudou
                    if(estoque.Quantidade != request.QuantidadeEstoque)
                    {
                        var quantidadeAnterior = estoque.Quantidade;
                        var quantidadeAtual = request.QuantidadeEstoque;

                        estoque.Quantidade = quantidadeAtual;
                        estoque.DataAtualizacao = agora;

                        if(estoque.Quantidade > estoque.EstoqueMinimo) {
                            estoque.AlertaEstoqueEnviado = false;
                        }

                        estoque.Movimentacoe.Add(new MovimentacaoEstoque
                        {
                            Tipo = TipoMovimentacaoEstoque.Ajuste,
                            Quantidade = Math.Abs(quantidadeAtual - quantidadeAnterior),
                            QuantidadeAnterior = quantidadeAnterior,
                            QuantidadeAtual = quantidadeAtual,
                            Observacao = "Ajuste de estoque pelo cadastro do produto",
                            DataCadastro = agora
                        });
                    }
                    else
                    {
                        estoque.DataAtualizacao = agora;
                    }
                }
            }

            await _menuFastContext.SaveChangesAsync();

            return ConverterParaDetalhe(produto);
        }

        public async Task<DetalheProdutosResponse> DetalharProduto(int idProduto) {
            var produto = await _menuFastContext.Produtos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == idProduto);

            if(produto == null) throw new BusinessLogicException("Produto não encontrado");
            return ConverterParaDetalhe(produto);
        }


        public async Task<List<DetalheProdutosResponse>> BuscarProdutos(int idLoja, FiltroProdutoRequest? filtro, string tipoFiltro) {
            var produtos = _menuFastContext.Produtos.AsNoTracking().Where(p => p.LojaId == idLoja);

            if(filtro != null)
            {
                if(tipoFiltro == "NOME" && !string.IsNullOrWhiteSpace(filtro.Nome))
                {
                    produtos = produtos.Where(p => EF.Functions.Like(p.Nome, $"%{filtro.Nome}%"));
                }
                else if(tipoFiltro == "CATEGORIA" && filtro.CategoriaId != null)
                {
                    produtos = produtos.Where(p => p.CategoriaProdutoId == filtro.CategoriaId);
                }
                else if(tipoFiltro == "CODIGO" && !string.IsNullOrWhiteSpace(filtro.Codigo))
                {
                    produtos = produtos.Where(p => p.Codigo == filtro.Codigo);
                }
                else
                {
                    produtos = produtos.Where(p => p.Ativo);
                }
            }

            return await produtos
                .OrderBy(p => p.Nome)
                .Select(p => new DetalheProdutosResponse
                {
                    Id = p.Id,
                    Codigo = p.Codigo,
                    FotoProduto = p.FotoProduto,
                    Nome = p.Nome,
                    Preco = MoedaHelper.FormatarReal(p.Preco),
                    Descricao = p.Descricao,
                    Ativo = p.Ativo
                })
                .ToListAsync();
        }
        public async Task RemoverProdutoCardapio(int idProduto) {
            var produto = await _menuFastContext.Produtos.FirstOrDefaultAsync(p => p.Id == idProduto);

            if(produto == null) throw new BusinessLogicException("Produto não encontrado");
            _menuFastContext.Produtos.Remove(produto);
            await _menuFastContext.SaveChangesAsync();
        }
        public async Task<List<DetalheProdutosResponse>> ListaProdutosEmEstoque(int idLoja) {
            return await _menuFastContext.Produtos.AsNoTracking().Where(p => p.LojaId == idLoja).OrderBy(p => p.Nome)
                .Select(p => new DetalheProdutosResponse
                {
                    Id = p.Id,
                    FotoProduto = p.FotoProduto,
                    Nome = p.Nome,
                    Preco = MoedaHelper.FormatarReal(p.Preco),
                    Codigo = p.Codigo,
                    Descricao = p.Descricao,
                    Ativo = p.Ativo,
                    QuantidadeEstoque = p.ControlaEstoque ? p.EstoqueProduto!.Quantidade : null,
                    EstoqueMinimo = p.ControlaEstoque ? p.EstoqueProduto!.EstoqueMinimo : null,
                    StatusEstoque = !p.ControlaEstoque ? null : p.EstoqueProduto!.Quantidade == 0 ? "ESGOTADO" : p.EstoqueProduto.Quantidade <= p.EstoqueProduto.EstoqueMinimo ? "ESTOQUE_BAIXO" : "NORMAL"
                })
                .ToListAsync();
        }

        private static DetalheProdutosResponse ConverterParaDetalhe(Produto produto) {
            return new DetalheProdutosResponse
            {
                Id = produto.Id,
                Nome = produto.Nome,
                FotoProduto = produto.FotoProduto,
                Preco = MoedaHelper.FormatarReal(produto.Preco),
                Codigo = produto.Codigo,
                Descricao = produto.Descricao,
                Ativo = produto.Ativo
            };
        }

        public async Task EnviarProdutosEsgotadosEmail() {
            try
            {
                var funcionario = await _menuFastContext.Funcionarios
                    .FirstOrDefaultAsync(f =>f.PerfilId == (int)PerfilUsuario.Administrador && f.Ativo);

                if(funcionario == null || string.IsNullOrWhiteSpace(funcionario.Email))
                    return;

                var produtos = await _menuFastContext.Produtos
                    .Include(p => p.EstoqueProduto)
                    .Where(p =>
                        p.Ativo &&
                        p.ControlaEstoque &&
                        p.EstoqueProduto != null &&
                        p.EstoqueProduto.Quantidade <= p.EstoqueProduto.EstoqueMinimo)
                    .OrderBy(p => p.EstoqueProduto.Quantidade)
                    .ToListAsync();

                if(!produtos.Any())
                    return;

                var templateEmail = await _menuFastContext.TemplatesEmail.FirstOrDefaultAsync(e => e.Nome == "ALERTA DE ESTOQUE" && e.Ativo);

                if(templateEmail == null)throw new BusinessLogicException("Não foi possível localizar o modelo de e-mail para alerta de estoque.");

                var produtosEstoque = string.Join("<br>", produtos.Select(p =>
                {
                    var quantidade = p.EstoqueProduto!.Quantidade;
                    var status = quantidade == 0? "ACABOU": "QUASE ACABANDO";
                    return $"{p.Nome} - Quantidade: {quantidade} - Status: {status}";
                }));

                var conteudo = templateEmail.Conteudo.Replace("{{PRODUTOS_ESTOQUE}}", produtosEstoque);

                foreach( var produto in produtos)
                {

                    if(produto.EstoqueProduto.Quantidade <= 0 && !produto.EstoqueProduto.AlertaEstoqueEnviado)

                    await _emailService.EnviarAsync(funcionario.Email,templateEmail.Assunto,conteudo);
                    produto!.EstoqueProduto!.AlertaEstoqueEnviado = true;
                    produto!.EstoqueProduto!.UltimoAlertaEstoque = DateTime.Now ;

                }
               await  _menuFastContext.SaveChangesAsync();
            }
            catch(BusinessLogicException)
            {
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError(
                    $"Erro inesperado ao enviar alerta de estoque: {ex.Message} " +
                    $"Data: {DateTime.UtcNow}",
                    $"Tipo de log: {TipoLog.ErroEnvioEmail.GetDisplayName()}"
                );

                throw new BusinessLogicException(
                    "Não foi possível enviar o alerta de estoque.");
            }
        }
    }
}