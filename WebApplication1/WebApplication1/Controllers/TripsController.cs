using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController: ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetTripsAsync()
    {

        return Ok();
    }
    
    
}