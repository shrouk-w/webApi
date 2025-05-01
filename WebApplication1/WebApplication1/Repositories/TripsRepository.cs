using Microsoft.Data.SqlClient;
using WebApplication1.Models;


namespace WebApplication1.Repositories;

public class TripsRepository: ITripsRepository
{
    private readonly string _connectionString;
    public TripsRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<Trip>> GetTripsAsync(CancellationToken cancellationToken)
    {
        var trips = new List<Trip>();

        await using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            
            var query = @"SELECT 
                        [dbo].[Trip].[IdTrip], 
                        [dbo].[Trip].[Name], 
                        [dbo].[Trip].[Description],
                        [dbo].[Trip].[DateFrom],
                        [dbo].[Trip].[DateTo],
                        [dbo].[Trip].[MaxPeople],
                        [dbo].[Country].[Name] as [CountryName],
                        [dbo].[Country].[IdCountry]
                        FROM [dbo].[Trip] 
                        Inner Join [dbo].[Country_Trip] On [dbo].[Trip].[IdTrip] = [dbo].[Country_Trip].[IdTrip] 
                        Inner Join [dbo].[Country] On [dbo].[Country_Trip].[IdCountry] = [dbo].[Country].[IdCountry]";

            await using (var command = new SqlCommand(query, connection))
            {

                await using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {

                    while (await reader.ReadAsync(cancellationToken))
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("IdTrip"));
                        if(trips.Any(t => t.IdTrip == id))
                        {
                            trips.Find(t => t.IdTrip == id).Country.Add(
                                    new Country
                                    {
                                        Name = reader.GetString(reader.GetOrdinal("CountryName")),
                                        IdCountry = reader.GetInt32(reader.GetOrdinal("IdCountry"))
                                    }
                                    );
                        }
                        else
                        {
                            var trip = new Trip
                            {
                                IdTrip = id,
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                DateFrom = reader.GetDateTime(reader.GetOrdinal("DateFrom")),
                                DateTo = reader.GetDateTime(reader.GetOrdinal("DateTo")),
                                MaxPeople = reader.GetInt32(reader.GetOrdinal("MaxPeople")),
                            };

                            trip.Country = new List<Country>();

                            trip.Country.Add(
                                new Country
                                {
                                    Name = reader.GetString(reader.GetOrdinal("CountryName")),
                                    IdCountry = reader.GetInt32(reader.GetOrdinal("IdCountry"))
                                });
                            trips.Add(trip);
                        }
                    }
                    
                }
                
                
            }
            
        }

        return trips;
    }
}