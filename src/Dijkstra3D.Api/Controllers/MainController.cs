using Dijkstra3D.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Dijkstra3D.Core;

namespace Dijkstra3D.Api.Controllers;

[ApiController]
[Route("api")]
public class MainController : ControllerBase
{
    private readonly ILogger<MainController> _logger;
    private readonly Core.IDijkstra3D _dijkstra3D;

    public MainController(ILogger<MainController> logger)
    {
        _logger = logger;
        _dijkstra3D = new Core.Dijkstra3D();
    }
    
    [HttpPost]
    [Route("great-circle/{hours}")]
    public IEnumerable<Waypoint> GetGreatCirclePath([FromBody] PathRequest request, double hours = 3) =>
        _dijkstra3D.GetGreatCirclePath(request.Departure, request.Arrival, hours);
    
}