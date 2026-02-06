using AgroSolutions.Analytics.Service.Domain.Enums;

namespace AgroSolutions.Analytics.Service.Application.Dtos.Alerta;

public class AlertaDto
{
    public Guid Id { get; set; }
    public Guid TalhaoId { get; set; }
    public TipoAlertaEnum TipoAlerta { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public DateTime DataOcorrencia { get; set; }
    public bool Resolvido { get; set; }
}
