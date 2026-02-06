using AgroSolutions.Analytics.Service.Application.Interfaces;
using AgroSolutions.Analytics.Service.Domain.Events;
using MassTransit;

namespace AgroSolutions.Analytics.Service.Worker.Consumers;

public class DadosSensorConsumer : IConsumer<DadosSensorRecebidosEvent>
{
    private readonly IAlertaService _alertaService;
    private readonly ILogger<DadosSensorConsumer> _logger;

    public DadosSensorConsumer(
        IAlertaService alertaService,
        ILogger<DadosSensorConsumer> logger)
    {
        _alertaService = alertaService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DadosSensorRecebidosEvent> context)
    {
        _logger.LogInformation("Consumindo evento de dados de sensor para talhão {TalhaoId}", context.Message.TalhaoId);
        
        await _alertaService.ProcessarDadosSensorAsync(context.Message);
    }
}
