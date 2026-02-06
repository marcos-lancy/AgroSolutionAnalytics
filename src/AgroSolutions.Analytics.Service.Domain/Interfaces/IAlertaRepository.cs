using AgroSolutions.Analytics.Service.Domain.Entities;

namespace AgroSolutions.Analytics.Service.Domain.Interfaces;

public interface IAlertaRepository : IRepository<AlertaEntity>
{
    Task<List<AlertaEntity>> ObterPorTalhaoIdAsync(Guid talhaoId);
    Task<List<AlertaEntity>> ObterAlertasAtivosAsync();
}
