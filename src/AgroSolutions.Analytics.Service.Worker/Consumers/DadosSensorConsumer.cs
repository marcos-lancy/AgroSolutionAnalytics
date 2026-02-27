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
        _logger.LogInformation("===============================================");
        _logger.LogInformation("MENSAGEM RECEBIDA NO WORKER!");
        _logger.LogInformation("TalhaoId: {TalhaoId}", context.Message.TalhaoId);
        _logger.LogInformation("Temperatura: {Temperatura}", context.Message.Temperatura);
        _logger.LogInformation("UmidadeSolo: {UmidadeSolo}", context.Message.UmidadeSolo);
        _logger.LogInformation("Precipitacao: {Precipitacao}", context.Message.Precipitacao);
        _logger.LogInformation("Timestamp: {Timestamp}", context.Message.Timestamp);
        _logger.LogInformation("===============================================");
        
        await _alertaService.ProcessarDadosSensorAsync(context.Message);
        
        _logger.LogInformation("Processamento concluido para talhao {TalhaoId}", context.Message.TalhaoId);
    }
}
