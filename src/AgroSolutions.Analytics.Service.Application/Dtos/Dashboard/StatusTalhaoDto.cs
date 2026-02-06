using AgroSolutions.Analytics.Service.Domain.Enums;

namespace AgroSolutions.Analytics.Service.Application.Dtos.Dashboard;

public class StatusTalhaoDto
{
    public Guid TalhaoId { get; set; }
    public TipoAlertaEnum Status { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateTime UltimaAtualizacao { get; set; }
}
