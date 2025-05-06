using WebApplication1.Models;

namespace WebApplication1.Repositories;

public interface ITripsRepository
{
    Task<IEnumerable<TripDTO>> GetTripsAsync(CancellationToken cancellationToken);
}