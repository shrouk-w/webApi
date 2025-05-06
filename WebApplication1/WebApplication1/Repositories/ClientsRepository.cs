using Microsoft.Data.SqlClient;
using WebApplication1.Exceptions;
using WebApplication1.Models;

namespace WebApplication1.Repositories;

public class ClientsRepository: IClientsRepository
{
    private readonly string _connectionString;
    
    public ClientsRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<TripForClientDTO>> GetTripsForClientAsync(int id, CancellationToken cancellationToken)
    {
        var trips = new List<TripForClientDTO>();

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
                        var trip = new TripForClientDTO()
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
        
        if(trips.Count<1)
            throw new NotFoundException("client with id: " + id + " is not assigned to any trips");

        return trips;
    }

    public async Task<int> CreateNewClientAsync(Client client, CancellationToken cancellationToken)
    {
        if (await DoesPeselExistAsync(client.Pesel, cancellationToken))
        {
            throw new ConflictException("client with this pesel already exists");
        }
        
        await using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);

            var query = @"
                        INSERT INTO [dbo].[Client]
                        ([FirstName], [LastName], [Email], [Telephone], [Pesel]) 
                        OUTPUT INSERTED.IdClient
                        VALUES
                            (@FirstName, @LastName, @Email, @Telephone, @Pesel)";
            await using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FirstName", client.FristName);
                command.Parameters.AddWithValue("@LastName", client.LastName);
                command.Parameters.AddWithValue("@Email", client.Email);
                command.Parameters.AddWithValue("@Telephone", client.Telephone);
                command.Parameters.AddWithValue("@Pesel", client.Pesel);
                
                var insertedid = await command.ExecuteScalarAsync(cancellationToken);
                
                return Convert.ToInt32(insertedid);
            }
        }
    }
    
    
    public async Task<bool> DoesPeselExistAsync(string pesel, CancellationToken cancellationToken)
    {
        
        await using (var connection = new SqlConnection(_connectionString)){
            
            await connection.OpenAsync(cancellationToken);

            var query = "SELECT COUNT(1) FROM [dbo].[Client] WHERE Pesel = @pesel";
            
            await using (var command = new SqlCommand(query, connection)){
                command.Parameters.AddWithValue("@pesel", pesel);
                
                var result = await command.ExecuteScalarAsync(cancellationToken);
                
                return Convert.ToInt32(result) > 0;
            }
        }
        
    }
}