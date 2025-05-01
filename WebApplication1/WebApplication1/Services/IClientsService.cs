using WebApplication1.Models;

namespace WebApplication1.Services;

public interface IClientsService
{
    Task<IEnumerable<TripForClient>> GetTripsForClientAsync(int id, CancellationToken cancellationToken);
}