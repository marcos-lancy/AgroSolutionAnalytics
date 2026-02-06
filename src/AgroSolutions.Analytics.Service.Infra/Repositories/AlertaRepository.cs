using AgroSolutions.Analytics.Service.Domain.Entities;
using AgroSolutions.Analytics.Service.Domain.Interfaces;
using AgroSolutions.Analytics.Service.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AgroSolutions.Analytics.Service.Infra.Repositories;

public class AlertaRepository(AppDbContext context) : Repository<AlertaEntity>(context), IAlertaRepository
{
    public async Task<List<AlertaEntity>> ObterPorTalhaoIdAsync(Guid talhaoId)
    {
        return await _dbSet
            .Where(a => a.TalhaoId == talhaoId)
            .OrderByDescending(a => a.DataOcorrencia)
            .ToListAsync();
    }

    public async Task<List<AlertaEntity>> ObterAlertasAtivosAsync()
    {
        return await _dbSet
            .Where(a => !a.Resolvido)
            .OrderByDescending(a => a.DataOcorrencia)
            .ToListAsync();
    }
}
