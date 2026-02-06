using Microsoft.AspNetCore.Mvc;

namespace AgroSolutions.Analytics.Service.Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class MainController : ControllerBase { }
