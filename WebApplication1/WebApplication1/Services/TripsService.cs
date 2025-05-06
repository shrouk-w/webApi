using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services;

public class TripsService: ITripsService
{
    private readonly ITripsRepository _repository;
    public TripsService(ITripsRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TripDTO>> GetTripsAsync(CancellationToken cancellationToken)
    {
        return await _repository.GetTripsAsync(cancellationToken);
    }
}