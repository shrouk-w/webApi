using WebApplication1.Models;

namespace WebApplication1.Repositories;

public interface IClientsRepository
{
    Task<IEnumerable<TripForClient>> GetTripsForClientAsync(int id, CancellationToken cancellationToken);
}