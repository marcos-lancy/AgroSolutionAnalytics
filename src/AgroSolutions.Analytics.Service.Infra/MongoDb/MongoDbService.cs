using AgroSolutions.Analytics.Service.Application.Interfaces;
using AgroSolutions.Analytics.Service.Application.Dtos.Sensor;
using MongoDB.Driver;

namespace AgroSolutions.Analytics.Service.Infra.MongoDb;

public class MongoDbService : IMongoDbService
{
    private readonly IMongoCollection<DadosSensorDocument> _collection;

    public MongoDbService(IMongoDatabase database)
    {
        _collection = database.GetCollection<DadosSensorDocument>("sensores");
    }

    public async Task<List<DadosSensorDto>> ObterDadosPorTalhaoAsync(Guid talhaoId, DateTime? dataInicio = null, DateTime? dataFim = null)
    {
        var filterBuilder = Builders<DadosSensorDocument>.Filter;
        var filter = filterBuilder.Eq(x => x.TalhaoId, talhaoId);

        if (dataInicio.HasValue)
            filter &= filterBuilder.Gte(x => x.Timestamp, dataInicio.Value);

        if (dataFim.HasValue)
            filter &= filterBuilder.Lte(x => x.Timestamp, dataFim.Value);

        var documentos = await _collection
            .Find(filter)
            .SortByDescending(x => x.Timestamp)
            .ToListAsync();

        return documentos.Select(d => new DadosSensorDto
        {
            TalhaoId = d.TalhaoId,
            UmidadeSolo = d.UmidadeSolo,
            Temperatura = d.Temperatura,
            Precipitacao = d.Precipitacao
        }).ToList();
    }
}

public class DadosSensorDocument
{
    public Guid Id { get; set; }
    public Guid TalhaoId { get; set; }
    public decimal UmidadeSolo { get; set; }
    public decimal Temperatura { get; set; }
    public decimal Precipitacao { get; set; }
    public DateTime Timestamp { get; set; }
}
