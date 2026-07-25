using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuFast.Api.Migrations
{
    /// <inheritdoc />
    public partial class BancoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cardapio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único do cardápio.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Nome do cardápio."),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Descrição do cardápio."),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o cardápio está ativo."),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Data de cadastro do cardápio.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cardapio", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único do cliente.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    EmpresaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Empresa à qual o cliente pertence."),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "Nome completo do cliente."),
                    CPF = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false, comment: "CPF do cliente."),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Data de nascimento do cliente."),
                    Telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Telefone do cliente."),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "WhatsApp do cliente."),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "E-mail do cliente."),
                    CEP = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false, comment: "CEP do endereço."),
                    Logradouro = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Logradouro do endereço."),
                    Numero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Número do endereço."),
                    Complemento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Complemento do endereço."),
                    Bairro = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Bairro do endereço."),
                    Cidade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Cidade do endereço."),
                    Estado = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false, comment: "UF do endereço."),
                    PontoReferencia = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Ponto de referência do endereço."),
                    Observacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "Observações do cliente."),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o cliente está ativo."),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Data de cadastro do cliente."),
                    Latitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false, comment: "Latitude do endereço do cliente."),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false, comment: "Longitude do endereço do cliente.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Complemento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único do complemento.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Nome do complemento."),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Valor adicional do complemento."),
                    Obrigatorio = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o complemento é obrigatório."),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o complemento está ativo.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complemento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContaPagar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único da conta a pagar.")
                        .Annotation("SqlServer:Identity", "13001, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false, comment: "Empresa responsável pela conta a pagar."),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Descrição da conta a pagar."),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Valor da conta a pagar."),
                    DataVencimento = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Data de vencimento da conta."),
                    DataPagamento = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Data de pagamento da conta."),
                    Pago = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se a conta foi paga."),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Status atual da conta financeira.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContaPagar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContaReceber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único da conta a receber.")
                        .Annotation("SqlServer:Identity", "12001, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false, comment: "Empresa responsável pela conta a receber."),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Descrição da conta a receber."),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Valor da conta a receber."),
                    DataVencimento = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Data de vencimento da conta."),
                    DataRecebimento = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Data de recebimento da conta."),
                    Recebido = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se a conta foi recebida."),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Status atual da conta financeira.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContaReceber", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empresa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único da empresa.")
                        .Annotation("SqlServer:Identity", "100, 1"),
                    Slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Slug único utilizado para identificar a empresa na URL."),
                    RazaoSocial = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Razão social da empresa."),
                    NomeFantasia = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Nome fantasia da empresa."),
                    Cnpj = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false, comment: "CNPJ da empresa."),
                    InscricaoEstadual = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Inscrição estadual da empresa."),
                    Telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Telefone principal da empresa."),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "E-mail principal da empresa."),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se a empresa está ativa no sistema."),
                    Cep = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false, comment: "CEP do endereço da empresa."),
                    Bairro = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Bairro da empresa."),
                    Cidade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Cidade da empresa."),
                    Estado = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Estado da empresa."),
                    Logradouro = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Logradouro da empresa."),
                    Numero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Número do endereço."),
                    Complemento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Complemento do endereço."),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Data de cadastro da empresa."),
                    Facebook = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "URL do perfil ou página da empresa no Facebook."),
                    Instagram = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "URL do perfil da empresa no Instagram."),
                    WhatsApp = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Número ou link do WhatsApp da empresa."),
                    TikTok = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "URL do perfil da empresa no TikTok."),
                    YouTube = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "URL do canal da empresa no YouTube."),
                    LinkedIn = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "URL da página da empresa no LinkedIn."),
                    Site = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Site oficial da empresa."),
                    Uf = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false, comment: "Sigla da unidade federativa."),
                    Logo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Caminho ou URL da logomarca da empresa.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entregador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único do entregador.")
                        .Annotation("SqlServer:Identity", "5001, 1"),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "Nome do entregador."),
                    Telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Telefone do entregador."),
                    MarcaMoto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Marca da motocicleta."),
                    Modelo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Modelo da motocicleta."),
                    Cor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Cor da motocicleta."),
                    Ano = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Ano da motocicleta."),
                    Placa = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "Placa da motocicleta.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entregador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Funcao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mesa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único da mesa.")
                        .Annotation("SqlServer:Identity", "7001, 1"),
                    Numero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Número ou identificação da mesa."),
                    ImagemUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "URL da imagem da mesa.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrdemProducao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único da ordem de produção.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    PedidoId = table.Column<int>(type: "int", nullable: false, comment: "Pedido vinculado à ordem de produção."),
                    FuncionarioId = table.Column<int>(type: "int", nullable: true, comment: "Funcionário responsável pela produção."),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Status atual da ordem de produção."),
                    Prioridade = table.Column<int>(type: "int", nullable: false, comment: "Prioridade da ordem de produção."),
                    DataEntrada = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Data e hora de entrada da ordem na produção."),
                    InicioPreparo = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Data e hora de início do preparo."),
                    FimPreparo = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Data e hora de término do preparo."),
                    Observacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Observações da ordem de produção.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdemProducao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Perfil",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfil", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProvedorPagamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único do provedor de pagamento.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Nome do provedor de pagamento."),
                    Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Código interno do provedor de pagamento."),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o provedor de pagamento está ativo.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvedorPagamento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoriaProduto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único da categoria de produtos.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    CardapioId = table.Column<int>(type: "int", nullable: false, comment: "Cardápio ao qual a categoria pertence."),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Nome da categoria."),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Descrição da categoria."),
                    Ordem = table.Column<int>(type: "int", nullable: false, comment: "Ordem de exibição da categoria no cardápio."),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se a categoria está ativa.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaProduto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriaProduto_Cardapio_CardapioId",
                        column: x => x.CardapioId,
                        principalTable: "Cardapio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracaoRestaurante",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único da configuração do restaurante.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false, comment: "Empresa vinculada à configuração do restaurante."),
                    TrabalhaComMesa = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o restaurante trabalha com controle de mesas."),
                    TrabalhaComComanda = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o restaurante trabalha com comandas."),
                    TrabalhaComDelivery = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o restaurante trabalha com pedidos delivery."),
                    TrabalhaComRetirada = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o restaurante trabalha com retirada no balcão."),
                    ControlaEstoque = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o restaurante utiliza controle de estoque."),
                    PermiteVendaSemEstoque = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se permite realizar venda de produtos sem estoque."),
                    CobraTaxaServico = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se cobra taxa de serviço."),
                    PercentualTaxaServico = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false, comment: "Percentual aplicado para cobrança da taxa de serviço."),
                    ExigirGarcomNaMesa = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se é obrigatório informar garçom responsável pela mesa."),
                    ImprimirPedidoAutomaticamente = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o pedido deve ser impresso automaticamente."),
                    EnviarPedidoAutomaticamenteCozinha = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o pedido deve ser enviado automaticamente para a cozinha."),
                    ExigirCaixaAberto = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se exige caixa aberto para realizar vendas."),
                    ImprimirComprovanteFechamento = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se imprime comprovante no fechamento do caixa."),
                    IdentificarClienteObrigatorio = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se a identificação do cliente é obrigatória."),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se a configuração está ativa."),
                    EmpresaId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracaoRestaurante", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracaoRestaurante_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConfiguracaoRestaurante_Empresa_EmpresaId1",
                        column: x => x.EmpresaId1,
                        principalTable: "Empresa",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContaBancaria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único da conta bancária.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false, comment: "Empresa proprietária da conta bancária."),
                    Banco = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Nome da instituição financeira."),
                    Agencia = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Número da agência."),
                    Conta = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Número da conta bancária."),
                    Digito = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "Dígito verificador da conta."),
                    Titular = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Nome do titular da conta."),
                    DocumentoTitular = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "CPF ou CNPJ do titular da conta.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContaBancaria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContaBancaria_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HorarioFuncionamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único do horário de funcionamento.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false, comment: "Empresa vinculada ao horário de funcionamento."),
                    DiaSemana = table.Column<int>(type: "int", nullable: false, comment: "Dia da semana em que o horário é aplicado."),
                    HoraAbertura = table.Column<TimeSpan>(type: "time", nullable: false, comment: "Horário de abertura do estabelecimento."),
                    HoraFechamento = table.Column<TimeSpan>(type: "time", nullable: false, comment: "Horário de fechamento do estabelecimento."),
                    Fechado = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o estabelecimento não funciona neste dia.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorarioFuncionamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HorarioFuncionamento_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Terminal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único do terminal.")
                        .Annotation("SqlServer:Identity", "200, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false, comment: "Empresa vinculada ao terminal."),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Nome do terminal."),
                    Identificacao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Identificador único do dispositivo."),
                    Tipo = table.Column<int>(type: "int", nullable: false, comment: "Tipo de utilização do terminal."),
                    Dispositivo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Nome ou modelo do dispositivo."),
                    SistemaOperacional = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Sistema operacional utilizado."),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o terminal está ativo."),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Data de cadastro do terminal.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terminal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Terminal_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedido",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único do pedido.")
                        .Annotation("SqlServer:Identity", "1001, 10"),
                    MesaId = table.Column<int>(type: "int", nullable: true, comment: "Mesa vinculada ao pedido."),
                    ClienteId = table.Column<int>(type: "int", nullable: true, comment: "Cliente vinculado ao pedido."),
                    FuncionarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Funcionário responsável pelo pedido."),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Status atual do pedido."),
                    TipoPedido = table.Column<int>(type: "int", nullable: false, comment: "Tipo do pedido realizado."),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Valor subtotal dos itens do pedido."),
                    Desconto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Valor de desconto aplicado."),
                    TaxaServico = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Taxa de serviço aplicada."),
                    TaxaEntrega = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Taxa de entrega aplicada."),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Valor total do pedido."),
                    DataPedidoHora = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Data e hora da criação do pedido."),
                    Observacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Observações do pedido.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedido_Mesa_MesaId",
                        column: x => x.MesaId,
                        principalTable: "Mesa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Funcionario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1000, 1"),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SenhaHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PrimeiroAcesso = table.Column<bool>(type: "bit", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataAdmissao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Salario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UltimoLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PerfilId = table.Column<int>(type: "int", nullable: false),
                    FuncaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Funcionario_Funcao_FuncaoId",
                        column: x => x.FuncaoId,
                        principalTable: "Funcao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Funcionario_Perfil_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfil",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PerfilPermissao",
                columns: table => new
                {
                    PerfilId = table.Column<int>(type: "int", nullable: false),
                    PermissaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilPermissao", x => new { x.PerfilId, x.PermissaoId });
                    table.ForeignKey(
                        name: "FK_PerfilPermissao_Perfil_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfil",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerfilPermissao_Permissao_PermissaoId",
                        column: x => x.PermissaoId,
                        principalTable: "Permissao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracaoProvedorPagamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único da configuração do provedor de pagamento.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false, comment: "Empresa vinculada ao provedor de pagamento."),
                    ProvedorPagamentoId = table.Column<int>(type: "int", nullable: false, comment: "Provedor de pagamento utilizado pela empresa."),
                    ChaveApi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Chave de acesso da API do provedor de pagamento."),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Token de autenticação do provedor de pagamento."),
                    SecretKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Chave secreta do provedor de pagamento."),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se a configuração do provedor está ativa.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracaoProvedorPagamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracaoProvedorPagamento_ProvedorPagamento_ProvedorPagamentoId",
                        column: x => x.ProvedorPagamentoId,
                        principalTable: "ProvedorPagamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormaPagamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único da forma de pagamento.")
                        .Annotation("SqlServer:Identity", "100, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Nome da forma de pagamento."),
                    PermiteTroco = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se a forma de pagamento permite troco."),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se a forma de pagamento está ativa."),
                    Foto = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Imagem ou ícone da forma de pagamento."),
                    ProvedorPagamentoId = table.Column<int>(type: "int", nullable: true, comment: "Provedor de pagamento vinculado à forma de pagamento.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormaPagamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormaPagamento_ProvedorPagamento_ProvedorPagamentoId",
                        column: x => x.ProvedorPagamentoId,
                        principalTable: "ProvedorPagamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Produto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único do produto.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    CategoriaProdutoId = table.Column<int>(type: "int", nullable: false, comment: "Categoria à qual o produto pertence."),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Nome do produto."),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Descrição do produto."),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Preço de venda do produto."),
                    Custo = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Custo do produto."),
                    CodigoBarras = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Código de barras do produto."),
                    ControlaEstoque = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o produto controla estoque."),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o produto está ativo."),
                    FotoProduto = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Caminho ou URL da foto do produto.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produto_CategoriaProduto_CategoriaProdutoId",
                        column: x => x.CategoriaProdutoId,
                        principalTable: "CategoriaProduto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChavePix",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único da chave Pix.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    ContaBancariaId = table.Column<int>(type: "int", nullable: false, comment: "Identificador da conta bancária vinculada à chave Pix."),
                    Tipo = table.Column<int>(type: "int", nullable: false, comment: "Tipo da chave Pix: CPF, CNPJ, e-mail, telefone ou chave aleatória."),
                    Chave = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "Valor da chave Pix cadastrada."),
                    Principal = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica se esta é a chave Pix principal da conta bancária.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChavePix", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChavePix_ContaBancaria_ContaBancariaId",
                        column: x => x.ContaBancariaId,
                        principalTable: "ContaBancaria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Impressora",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único da impressora.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    TerminalId = table.Column<int>(type: "int", nullable: false, comment: "Terminal ao qual a impressora está vinculada."),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Nome da impressora."),
                    Modelo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Modelo da impressora."),
                    EnderecoIp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Endereço IP ou hostname da impressora."),
                    Porta = table.Column<int>(type: "int", nullable: false, comment: "Porta de comunicação da impressora."),
                    Padrao = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se esta é a impressora padrão do terminal."),
                    Ativa = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se a impressora está ativa.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impressora", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Impressora_Terminal_TerminalId",
                        column: x => x.TerminalId,
                        principalTable: "Terminal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Entrega",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único da entrega.")
                        .Annotation("SqlServer:Identity", "6001, 1"),
                    PedidoId = table.Column<int>(type: "int", nullable: false, comment: "Pedido vinculado à entrega."),
                    ClienteEnderecoId = table.Column<int>(type: "int", nullable: true, comment: "Endereço do cliente para entrega."),
                    MotoboyId = table.Column<int>(type: "int", nullable: true, comment: "Entregador responsável pela entrega."),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Status atual da entrega."),
                    TaxaEntrega = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Valor da taxa de entrega."),
                    DataSaida = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Data e hora de saída para entrega."),
                    DataEntrega = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Data e hora da entrega realizada.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrega", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entrega_Entregador_MotoboyId",
                        column: x => x.MotoboyId,
                        principalTable: "Entregador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entrega_Pedido_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoricoPedido",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único do histórico do pedido.")
                        .Annotation("SqlServer:Identity", "4001, 1"),
                    PedidoId = table.Column<int>(type: "int", nullable: false, comment: "Pedido relacionado ao histórico."),
                    Acao = table.Column<int>(type: "int", nullable: false, comment: "Ação realizada no histórico do pedido."),
                    Observacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Observação do histórico."),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Data e hora do registro do histórico."),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Usuário responsável pela ação.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoPedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoPedido_Pedido_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemPedido",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único do item do pedido.")
                        .Annotation("SqlServer:Identity", "3001, 1"),
                    PedidoId = table.Column<int>(type: "int", nullable: false, comment: "Pedido vinculado ao item."),
                    ProdutoId = table.Column<int>(type: "int", nullable: false, comment: "Produto vinculado ao item."),
                    Quantidade = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false, comment: "Quantidade do produto no pedido."),
                    ValorUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Valor unitário do produto."),
                    Desconto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Desconto aplicado no item."),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Valor total do item."),
                    Observacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Observação do item do pedido.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemPedido_Pedido_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Caixa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único do caixa.")
                        .Annotation("SqlServer:Identity", "100, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Nome do caixa."),
                    Aberto = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o caixa está aberto."),
                    ValorAbertura = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Valor informado na abertura do caixa."),
                    ValorFechamento = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Valor informado no fechamento do caixa."),
                    DataAbertura = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Data e hora de abertura do caixa."),
                    DataFechamento = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Data e hora de fechamento do caixa."),
                    FuncioanrioId = table.Column<int>(type: "int", nullable: false, comment: "Funcionário responsável pelo caixa.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caixa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Caixa_Funcionario_FuncioanrioId",
                        column: x => x.FuncioanrioId,
                        principalTable: "Funcionario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PagamentoPedido",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único do pagamento do pedido.")
                        .Annotation("SqlServer:Identity", "2001, 1"),
                    PedidoId = table.Column<int>(type: "int", nullable: false, comment: "Pedido vinculado ao pagamento."),
                    FormaPagamentoId = table.Column<int>(type: "int", nullable: false, comment: "Forma de pagamento utilizada."),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Valor pago."),
                    DataPagamento = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Data e hora do pagamento.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagamentoPedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagamentoPedido_FormaPagamento_FormaPagamentoId",
                        column: x => x.FormaPagamentoId,
                        principalTable: "FormaPagamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PagamentoPedido_Pedido_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComplementoProduto",
                columns: table => new
                {
                    ComplementosId = table.Column<int>(type: "int", nullable: false),
                    ProdutosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplementoProduto", x => new { x.ComplementosId, x.ProdutosId });
                    table.ForeignKey(
                        name: "FK_ComplementoProduto_Complemento_ComplementosId",
                        column: x => x.ComplementosId,
                        principalTable: "Complemento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComplementoProduto_Produto_ProdutosId",
                        column: x => x.ProdutosId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpcaoProduto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único da opção do produto.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    ProdutoId = table.Column<int>(type: "int", nullable: false, comment: "Produto ao qual a opção pertence."),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Nome da opção do produto."),
                    Acrescimo = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Valor de acréscimo da opção.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpcaoProduto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpcaoProduto_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovimentoCaixa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único do movimento de caixa.")
                        .Annotation("SqlServer:Identity", "11001, 1"),
                    CaixaId = table.Column<int>(type: "int", nullable: false, comment: "Caixa vinculado ao movimento."),
                    FuncionarioId = table.Column<int>(type: "int", nullable: false, comment: "Funcionário responsável pelo movimento."),
                    Tipo = table.Column<int>(type: "int", nullable: false, comment: "Tipo do movimento realizado no caixa."),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Valor do movimento."),
                    Descricao = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "Descrição do movimento."),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Data e hora do movimento."),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimentoCaixa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimentoCaixa_Caixa_CaixaId",
                        column: x => x.CaixaId,
                        principalTable: "Caixa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovimentoCaixa_Funcionario_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Caixa_FuncioanrioId",
                table: "Caixa",
                column: "FuncioanrioId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaProduto_CardapioId",
                table: "CategoriaProduto",
                column: "CardapioId");

            migrationBuilder.CreateIndex(
                name: "IX_ChavePix_ContaBancariaId_Chave",
                table: "ChavePix",
                columns: new[] { "ContaBancariaId", "Chave" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComplementoProduto_ProdutosId",
                table: "ComplementoProduto",
                column: "ProdutosId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoProvedorPagamento_ProvedorPagamentoId",
                table: "ConfiguracaoProvedorPagamento",
                column: "ProvedorPagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoRestaurante_EmpresaId",
                table: "ConfiguracaoRestaurante",
                column: "EmpresaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoRestaurante_EmpresaId1",
                table: "ConfiguracaoRestaurante",
                column: "EmpresaId1",
                unique: true,
                filter: "[EmpresaId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ContaBancaria_EmpresaId",
                table: "ContaBancaria",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_Slug",
                table: "Empresa",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entrega_MotoboyId",
                table: "Entrega",
                column: "MotoboyId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrega_PedidoId",
                table: "Entrega",
                column: "PedidoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormaPagamento_ProvedorPagamentoId",
                table: "FormaPagamento",
                column: "ProvedorPagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Funcao_Nome",
                table: "Funcao",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionario_Cpf",
                table: "Funcionario",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionario_Email",
                table: "Funcionario",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionario_FuncaoId",
                table: "Funcionario",
                column: "FuncaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionario_Login",
                table: "Funcionario",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionario_PerfilId",
                table: "Funcionario",
                column: "PerfilId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoPedido_PedidoId",
                table: "HistoricoPedido",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_HorarioFuncionamento_EmpresaId",
                table: "HorarioFuncionamento",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Impressora_TerminalId",
                table: "Impressora",
                column: "TerminalId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPedido_PedidoId",
                table: "ItemPedido",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentoCaixa_CaixaId",
                table: "MovimentoCaixa",
                column: "CaixaId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentoCaixa_FuncionarioId",
                table: "MovimentoCaixa",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_OpcaoProduto_ProdutoId",
                table: "OpcaoProduto",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_PagamentoPedido_FormaPagamentoId",
                table: "PagamentoPedido",
                column: "FormaPagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_PagamentoPedido_PedidoId",
                table: "PagamentoPedido",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_MesaId",
                table: "Pedido",
                column: "MesaId");

            migrationBuilder.CreateIndex(
                name: "IX_Perfil_Nome",
                table: "Perfil",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PerfilPermissao_PermissaoId",
                table: "PerfilPermissao",
                column: "PermissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissao_Codigo",
                table: "Permissao",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Produto_CategoriaProdutoId",
                table: "Produto",
                column: "CategoriaProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Terminal_EmpresaId",
                table: "Terminal",
                column: "EmpresaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChavePix");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "ComplementoProduto");

            migrationBuilder.DropTable(
                name: "ConfiguracaoProvedorPagamento");

            migrationBuilder.DropTable(
                name: "ConfiguracaoRestaurante");

            migrationBuilder.DropTable(
                name: "ContaPagar");

            migrationBuilder.DropTable(
                name: "ContaReceber");

            migrationBuilder.DropTable(
                name: "Entrega");

            migrationBuilder.DropTable(
                name: "HistoricoPedido");

            migrationBuilder.DropTable(
                name: "HorarioFuncionamento");

            migrationBuilder.DropTable(
                name: "Impressora");

            migrationBuilder.DropTable(
                name: "ItemPedido");

            migrationBuilder.DropTable(
                name: "MovimentoCaixa");

            migrationBuilder.DropTable(
                name: "OpcaoProduto");

            migrationBuilder.DropTable(
                name: "OrdemProducao");

            migrationBuilder.DropTable(
                name: "PagamentoPedido");

            migrationBuilder.DropTable(
                name: "PerfilPermissao");

            migrationBuilder.DropTable(
                name: "ContaBancaria");

            migrationBuilder.DropTable(
                name: "Complemento");

            migrationBuilder.DropTable(
                name: "Entregador");

            migrationBuilder.DropTable(
                name: "Terminal");

            migrationBuilder.DropTable(
                name: "Caixa");

            migrationBuilder.DropTable(
                name: "Produto");

            migrationBuilder.DropTable(
                name: "FormaPagamento");

            migrationBuilder.DropTable(
                name: "Pedido");

            migrationBuilder.DropTable(
                name: "Permissao");

            migrationBuilder.DropTable(
                name: "Empresa");

            migrationBuilder.DropTable(
                name: "Funcionario");

            migrationBuilder.DropTable(
                name: "CategoriaProduto");

            migrationBuilder.DropTable(
                name: "ProvedorPagamento");

            migrationBuilder.DropTable(
                name: "Mesa");

            migrationBuilder.DropTable(
                name: "Funcao");

            migrationBuilder.DropTable(
                name: "Perfil");

            migrationBuilder.DropTable(
                name: "Cardapio");
        }
    }
}
