using AgroSolutions.Analytics.Service.Application.Interfaces;
using AgroSolutions.Analytics.Service.Domain.Entities;
using AgroSolutions.Analytics.Service.Domain.Enums;
using AgroSolutions.Analytics.Service.Domain.Events;
using AgroSolutions.Analytics.Service.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace AgroSolutions.Analytics.Service.Application.Services;

public class AlertaService : IAlertaService
{
    private readonly IAlertaRepository _alertaRepository;
    private readonly IMongoDbService _mongoDbService;
    private readonly ILogger<AlertaService> _logger;

    public AlertaService(
        IAlertaRepository alertaRepository,
        IMongoDbService mongoDbService,
        ILogger<AlertaService> logger)
    {
        _alertaRepository = alertaRepository;
        _mongoDbService = mongoDbService;
        _logger = logger;
    }

    public async Task ProcessarDadosSensorAsync(DadosSensorRecebidosEvent evento)
    {
        _logger.LogInformation("Processando dados de sensor para talhão {TalhaoId}", evento.TalhaoId);

        // Verificar regra de alerta: umidade < 30% por mais de 24 horas
        var dataLimite = DateTime.UtcNow.AddHours(-24);
        var dadosUltimas24h = await _mongoDbService.ObterDadosPorTalhaoAsync(
            evento.TalhaoId,
            dataLimite,
            DateTime.UtcNow);

        if (dadosUltimas24h.Count >= 24) // Assumindo 1 leitura por hora
        {
            var umidadeMedia = dadosUltimas24h.Average(d => d.UmidadeSolo);

            if (umidadeMedia < 30)
            {
                // Verificar se já existe alerta ativo
                var alertasExistentes = await _alertaRepository.ObterPorTalhaoIdAsync(evento.TalhaoId);
                var alertaSecaAtivo = alertasExistentes
                    .FirstOrDefault(a => a.TipoAlerta == TipoAlertaEnum.AlertaSeca && !a.Resolvido);

                if (alertaSecaAtivo == null)
                {
                    var novoAlerta = new AlertaEntity
                    {
                        TalhaoId = evento.TalhaoId,
                        TipoAlerta = TipoAlertaEnum.AlertaSeca,
                        Mensagem = $"Umidade do solo abaixo de 30% por mais de 24 horas. Umidade média: {umidadeMedia:F2}%",
                        DataOcorrencia = DateTime.UtcNow
                    };

                    await _alertaRepository.AdicionarAsync(novoAlerta);
                    _logger.LogWarning("Alerta de seca gerado para talhão {TalhaoId}", evento.TalhaoId);
                }
            }
        }

        // Verificar risco de praga (temperatura alta + umidade alta)
        if (evento.Temperatura > 35 && evento.UmidadeSolo > 70)
        {
            var alertasExistentes = await _alertaRepository.ObterPorTalhaoIdAsync(evento.TalhaoId);
            var alertaPragaAtivo = alertasExistentes
                .FirstOrDefault(a => a.TipoAlerta == TipoAlertaEnum.RiscoPraga && !a.Resolvido);

            if (alertaPragaAtivo == null)
            {
                var novoAlerta = new AlertaEntity
                {
                    TalhaoId = evento.TalhaoId,
                    TipoAlerta = TipoAlertaEnum.RiscoPraga,
                    Mensagem = $"Condições favoráveis para pragas detectadas. Temperatura: {evento.Temperatura}°C, Umidade: {evento.UmidadeSolo}%",
                    DataOcorrencia = DateTime.UtcNow
                };

                await _alertaRepository.AdicionarAsync(novoAlerta);
                _logger.LogWarning("Alerta de risco de praga gerado para talhão {TalhaoId}", evento.TalhaoId);
            }
        }
    }
}
