using WebApplication1.DTOs;
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

    public async Task<IEnumerable<TripForClientDTO>> GetTripsForClientAsync(int id, CancellationToken cancellationToken)
    {
        if(id<0)
            throw new BadRequestException("id must be greater than 0");
        
        return await _clientsRepository.GetTripsForClientAsync(id, cancellationToken);
    }

    public async Task<int> CreateNewClientAsync(CreateClientDTO dto, CancellationToken cancellationToken)
    {
        
        var client = new Client()
        {
            Email = dto.Email,
            FristName = dto.FirstName,
            LastName = dto.LastName,
            Telephone = dto.Telephone,
            Pesel = dto.Pesel
        };
        
        return await _clientsRepository.CreateNewClientAsync(client, cancellationToken);
    }
}

