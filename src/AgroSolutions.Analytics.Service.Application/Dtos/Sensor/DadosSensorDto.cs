namespace AgroSolutions.Analytics.Service.Application.Dtos.Sensor;

public class DadosSensorDto
{
    public Guid TalhaoId { get; set; }
    public decimal UmidadeSolo { get; set; }
    public decimal Temperatura { get; set; }
    public decimal Precipitacao { get; set; }
}
