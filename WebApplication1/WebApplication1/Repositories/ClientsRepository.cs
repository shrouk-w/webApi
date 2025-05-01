using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Repositories;

public class ClientsRepository: IClientsRepository
{
    private readonly string _connectionString;
    
    public ClientsRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<TripForClient>> GetTripsForClientAsync(int id, CancellationToken cancellationToken)
    {
        var trips = new List<TripForClient>();

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
                        [dbo].[Client_Trip].[IdClient],
                        [dbo].[Client_Trip].[RegisteredAt],
                        [dbo].[Client_Trip].[PaymentDate]
                        FROM [dbo].[Trip] 
                        Inner Join [dbo].[Client_Trip] On [dbo].[Client_Trip].[IdTrip] = [dbo].[Trip].[IdTrip]
                        WHERE [dbo].[Client_Trip].[IdClient] = @id";

            await using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                await using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {

                    while (await reader.ReadAsync(cancellationToken))
                    {
                        var trip = new TripForClient()
                        {
                            IdTrip = reader.GetInt32(reader.GetOrdinal("IdTrip")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            DateFrom = reader.GetDateTime(reader.GetOrdinal("DateFrom")),
                            DateTo = reader.GetDateTime(reader.GetOrdinal("DateTo")),
                            MaxPeople = reader.GetInt32(reader.GetOrdinal("MaxPeople")),
                            IdClient = reader.GetInt32(reader.GetOrdinal("IdClient")),
                            RegisteredAt = reader.GetInt32(reader.GetOrdinal("RegisteredAt")),
                            PaymentDate = reader.GetInt32(reader.GetOrdinal("PaymentDate"))
                        };
                        
                        trips.Add(trip);
                        
                    }
                    
                }
                
                
            }
            
        }

        return trips;
    }
}