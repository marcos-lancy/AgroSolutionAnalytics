using AgroSolutions.Analytics.Service.Application.Dtos.Alerta;
using AgroSolutions.Analytics.Service.Application.Dtos.Dashboard;
using AgroSolutions.Analytics.Service.Application.Interfaces;
using AgroSolutions.Analytics.Service.Domain.Enums;
using AgroSolutions.Analytics.Service.Domain.Interfaces;

namespace AgroSolutions.Analytics.Service.Application.AppServices;

public class AnalyticsAppService : IAnalyticsAppService
{
    private readonly IAlertaRepository _alertaRepository;

    public AnalyticsAppService(IAlertaRepository alertaRepository)
    {
        _alertaRepository = alertaRepository;
    }

    public async Task<List<AlertaDto>> ObterAlertasPorTalhaoAsync(Guid talhaoId)
    {
        var alertas = await _alertaRepository.ObterPorTalhaoIdAsync(talhaoId);
        
        return alertas.Select(a => new AlertaDto
        {
            Id = a.Id,
            TalhaoId = a.TalhaoId,
            TipoAlerta = a.TipoAlerta,
            Mensagem = a.Mensagem,
            DataOcorrencia = a.DataOcorrencia,
            Resolvido = a.Resolvido
        }).ToList();
    }

    public async Task<List<AlertaDto>> ObterAlertasAtivosAsync()
    {
        var alertas = await _alertaRepository.ObterAlertasAtivosAsync();
        
        return alertas.Select(a => new AlertaDto
        {
            Id = a.Id,
            TalhaoId = a.TalhaoId,
            TipoAlerta = a.TipoAlerta,
            Mensagem = a.Mensagem,
            DataOcorrencia = a.DataOcorrencia,
            Resolvido = a.Resolvido
        }).ToList();
    }

    public async Task<List<StatusTalhaoDto>> ObterStatusTalhoesAsync(List<Guid> talhaoIds)
    {
        var statusList = new List<StatusTalhaoDto>();

        foreach (var talhaoId in talhaoIds)
        {
            var alertas = await _alertaRepository.ObterPorTalhaoIdAsync(talhaoId);
            var alertaAtivo = alertas.FirstOrDefault(a => !a.Resolvido);

            var status = new StatusTalhaoDto
            {
                TalhaoId = talhaoId,
                Status = alertaAtivo?.TipoAlerta ?? TipoAlertaEnum.Normal,
                Descricao = alertaAtivo?.Mensagem ?? "Status normal",
                UltimaAtualizacao = alertaAtivo?.DataOcorrencia ?? DateTime.UtcNow
            };

            statusList.Add(status);
        }

        return statusList;
    }
}
