using MenuFast.Api.Api.Application.Services.ProdutoServices;

namespace MenuFast.Api.BackgroundServices {
    public class AlertaEstoqueBackgroundService : BackgroundService {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<AlertaEstoqueBackgroundService> _logger;

        public AlertaEstoqueBackgroundService(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<AlertaEstoqueBackgroundService> logger) {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {_logger.LogInformation("Serviço de alerta de estoque iniciado.");

            while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope =_serviceScopeFactory.CreateScope();
                    var service = scope.ServiceProvider.GetRequiredService<ProdutoServices>();
                    await service.EnviarProdutosEsgotadosEmail();
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex,"Erro ao executar o serviço de alerta de estoque." , $" Data e dia do erro  - {DateTime.Now}");
                }

                await Task.Delay(
                    TimeSpan.FromMinutes(5),
                    stoppingToken);
            }
        }
    }
}
