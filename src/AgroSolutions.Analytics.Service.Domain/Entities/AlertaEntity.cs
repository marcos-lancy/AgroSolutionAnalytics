using AgroSolutions.Analytics.Service.Domain.Enums;

namespace AgroSolutions.Analytics.Service.Domain.Entities;

public class AlertaEntity : EntityBase
{
    public Guid TalhaoId { get; set; }
    public TipoAlertaEnum TipoAlerta { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public DateTime DataOcorrencia { get; set; }
    public bool Resolvido { get; set; } = false;

    public AlertaEntity()
    {
    }

    public AlertaEntity(
        Guid id,
        Guid talhaoId,
        TipoAlertaEnum tipoAlerta,
        string mensagem,
        DateTime dataOcorrencia)
    {
        Id = id;
        TalhaoId = talhaoId;
        TipoAlerta = tipoAlerta;
        Mensagem = mensagem;
        DataOcorrencia = dataOcorrencia;
    }
}
