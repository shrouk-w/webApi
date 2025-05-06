using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Services;

public interface IClientsService
{
    Task<IEnumerable<TripForClientDTO>> GetTripsForClientAsync(int id, CancellationToken cancellationToken);
    Task<int> CreateNewClientAsync(CreateClientDTO dto, CancellationToken cancellationToken);
    Task AssignClientToTripAsync(int id, int tripId, CancellationToken cancellationToken);
}