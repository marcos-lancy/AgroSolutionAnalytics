using AgroSolutions.Analytics.Service.Application.Dtos.Dashboard;
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
public class DashboardController : MainController
{
    private readonly IAnalyticsAppService _analyticsAppService;

    public DashboardController(IAnalyticsAppService analyticsAppService)
    {
        _analyticsAppService = analyticsAppService;
    }

    [HttpPost("status-talhoes")]
    [SwaggerOperation(Summary = "Obtém status de múltiplos talhões")]
    [ProducesResponseType(typeof(List<StatusTalhaoDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> ObterStatusTalhoes([FromBody] List<Guid> talhaoIds) 
        => Ok(await _analyticsAppService.ObterStatusTalhoesAsync(talhaoIds));
}
