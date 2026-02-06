using AgroSolutions.Analytics.Service.Application.Dtos.Alerta;
using AgroSolutions.Analytics.Service.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace AgroSolutions.Analytics.Service.Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class AlertasController : MainController
{
    private readonly IAnalyticsAppService _analyticsAppService;

    public AlertasController(IAnalyticsAppService analyticsAppService)
    {
        _analyticsAppService = analyticsAppService;
    }

    [HttpGet("talhao/{talhaoId}")]
    [SwaggerOperation(Summary = "Lista alertas de um talhão")]
    [ProducesResponseType(typeof(List<AlertaDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> ObterPorTalhao(Guid talhaoId) 
        => Ok(await _analyticsAppService.ObterAlertasPorTalhaoAsync(talhaoId));

    [HttpGet("ativos")]
    [SwaggerOperation(Summary = "Lista todos os alertas ativos")]
    [ProducesResponseType(typeof(List<AlertaDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> ObterAtivos() 
        => Ok(await _analyticsAppService.ObterAlertasAtivosAsync());
}
