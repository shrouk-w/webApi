using WebApplication1.Exceptions;
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
        if(id<0)
            throw new BadRequestException("id must be greater than 0");
        //
        return await _clientsRepository.GetTripsForClientAsync(id, cancellationToken);
    }
    
}

