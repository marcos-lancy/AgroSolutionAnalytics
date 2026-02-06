using AgroSolutions.Analytics.Service.Application.Dtos.Alerta;
using AgroSolutions.Analytics.Service.Application.Dtos.Dashboard;

namespace AgroSolutions.Analytics.Service.Application.Interfaces;

public interface IAnalyticsAppService
{
    Task<List<AlertaDto>> ObterAlertasPorTalhaoAsync(Guid talhaoId);
    Task<List<AlertaDto>> ObterAlertasAtivosAsync();
    Task<List<StatusTalhaoDto>> ObterStatusTalhoesAsync(List<Guid> talhaoIds);
}
