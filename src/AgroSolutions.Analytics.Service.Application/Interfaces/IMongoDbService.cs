using AgroSolutions.Analytics.Service.Application.Dtos.Sensor;

namespace AgroSolutions.Analytics.Service.Application.Interfaces;

public interface IMongoDbService
{
    Task<List<DadosSensorDto>> ObterDadosPorTalhaoAsync(Guid talhaoId, DateTime? dataInicio = null, DateTime? dataFim = null);
}
