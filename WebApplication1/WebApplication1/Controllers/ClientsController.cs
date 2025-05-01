using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers;



[ApiController]
[Route("api/[controller]")]

public class ClientsController: ControllerBase
{
    private readonly IClientsService _clientsService;
    
    public ClientsController(IClientsService clientsService)
    {
        _clientsService = clientsService;
    }

    [HttpGet("{id}/trips")]
    public async Task<IActionResult> GetTripsForClientAsync(int id, CancellationToken cancellationToken)
    {
        var response = await _clientsService.GetTripsForClientAsync(id, cancellationToken);
        return Ok(response);
    }
}