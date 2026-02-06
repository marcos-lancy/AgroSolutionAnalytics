using AgroSolutions.Analytics.Service.Domain.Events;

namespace AgroSolutions.Analytics.Service.Application.Interfaces;

public interface IAlertaService
{
    Task ProcessarDadosSensorAsync(DadosSensorRecebidosEvent evento);
}
