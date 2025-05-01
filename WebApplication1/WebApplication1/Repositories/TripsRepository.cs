using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Repositories;

public class TripsRepository: ITripsRepository
{
    public async Task<IEnumerable<Trip>> GetTripsAsync()
    {
        throw new NotImplementedException();
    }
}