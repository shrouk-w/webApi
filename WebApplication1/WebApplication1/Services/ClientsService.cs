using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services;

public class ClientsService: IClientsService
{
    private readonly IClientsRepository _clientsRepository;
    public ClientsService(IClientsRepository clientsRepository)
    {
        _clientsRepository = clientsRepository;
    }

    public async Task<IEnumerable<TripForClient>> GetTripsForClientAsync(int id, CancellationToken cancellationToken)
    {
        return await _clientsRepository.GetTripsForClientAsync(id, cancellationToken);
    }
    
}

