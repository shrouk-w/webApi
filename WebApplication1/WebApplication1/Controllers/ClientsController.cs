using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Models;
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

    [HttpPost]
    public async Task<IActionResult> CreateNewClientAsync([FromBody] CreateClientDTO dto, CancellationToken cancellationToken)
    {
        var clientsid = await _clientsService.CreateNewClientAsync(dto, cancellationToken);
        return Created($"/api/clients", new { ClientsId = clientsid});
    }

    [HttpPut("{id}/trips/{tripId}")]
    public async Task<IActionResult> AssignClientToTripAsync(int id, int tripId, CancellationToken cancellationToken)
    {
        await _clientsService.AssignClientToTripAsync(id, tripId, cancellationToken);
        return Created($"/api/clients/{id}/trips/{tripId}", new { ClientsId = id, TripId = tripId });
    }
}