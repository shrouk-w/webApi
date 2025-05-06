using WebApplication1.Models;

namespace WebApplication1.Repositories;

public interface IClientsRepository
{
    Task<IEnumerable<TripForClientDTO>> GetTripsForClientAsync(int id, CancellationToken cancellationToken);
    Task<int> CreateNewClientAsync(Client client, CancellationToken cancellationToken);
}