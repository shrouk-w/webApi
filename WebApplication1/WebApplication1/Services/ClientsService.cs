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
        
        if(!await _clientsRepository.DoesClientExistAsync(id, cancellationToken))
            throw new NotFoundException("client with id: "+id+" doesnt exist");
        
        return await _clientsRepository.GetTripsForClientAsync(id, cancellationToken);
    }

    public async Task<int> CreateNewClientAsync(CreateClientDTO dto, CancellationToken cancellationToken)
    {
        if (await _clientsRepository.DoesPeselExistAsync(dto.Pesel, cancellationToken))
        {
            throw new ConflictException("client with this pesel already exists");
        }
        
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

    public async Task AssignClientToTripAsync(int id, int tripId, CancellationToken cancellationToken)
    {
        if(id<0)
            throw new BadRequestException("clients id must be greater than 0");
        
        if(tripId<0)
            throw new BadRequestException("trip id must be greater than 0");
        
        if(!await _clientsRepository.DoesClientExistAsync(id, cancellationToken))
            throw new NotFoundException("client with id: "+id+" doesnt exist");
        
        if(!await _clientsRepository.DoesTripExistAsync(tripId, cancellationToken))
            throw new NotFoundException("trip with id: "+tripId+" doesnt exist");
        
        if(await _clientsRepository.DoesClientTripAssignmentExist(id, tripId,cancellationToken))
            throw new ConflictException("client id: "+id+", trip id: "+tripId+" are already assigned to eachother");
        
        var alreadyassignednumber = await _clientsRepository.HowManyPeopleAreAssignedToTripAsync(tripId, cancellationToken);
        
        var maxpeople = await _clientsRepository.MaxPeopleOnTrip(tripId, cancellationToken);
        
        if(alreadyassignednumber>=maxpeople)
            throw new ConflictException("trip id: "+tripId+" is already at its full capacity");
        
        var currDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"));

        await _clientsRepository.AssignClientToTripAsync(id, tripId, currDate, null, cancellationToken);
    }

    public async Task DeleteClientToTripAssignmentAsync(int id, int tripId, CancellationToken cancellationToken)
    {
        if(! await _clientsRepository.DoesClientTripAssignmentExist(id, tripId,cancellationToken))
            throw new ConflictException("client id: "+id+", trip id: "+tripId+" arent assigned to eachother");
        await _clientsRepository.DeleteClientToTripAssignmentAsync(id, tripId, cancellationToken);
    }
}

