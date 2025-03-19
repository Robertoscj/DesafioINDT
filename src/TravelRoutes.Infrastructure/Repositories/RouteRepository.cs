using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using TravelRoutes.Domain.Entities;
using TravelRoutes.Domain.Interfaces;

namespace TravelRoutes.Infrastructure.Repositories
{
    public class RouteRepository : IRouteRepository
    {
        private readonly string _connectionString;

        public RouteRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<int> AddAsync(Route route)
        {
            using var connection = CreateConnection();
            var sql = @"INSERT INTO Routes (Origin, Destination, Cost) 
                       VALUES (@Origin, @Destination, @Cost);
                       SELECT CAST(SCOPE_IDENTITY() as int)";
            return await connection.QuerySingleAsync<int>(sql, route);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = CreateConnection();
            var sql = "DELETE FROM Routes WHERE Id = @Id";
            await connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IEnumerable<Route>> GetAllAsync()
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Routes";
            return await connection.QueryAsync<Route>(sql);
        }

        public async Task<Route?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Routes WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<Route>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Route>> GetAllRoutesFromOriginAsync(string origin)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Routes WHERE Origin = @Origin";
            return await connection.QueryAsync<Route>(sql, new { Origin = origin });
        }

        public async Task<Route?> GetRouteByOriginAndDestinationAsync(string origin, string destination)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Routes WHERE Origin = @Origin AND Destination = @Destination";
            return await connection.QuerySingleOrDefaultAsync<Route>(sql, new { Origin = origin, Destination = destination });
        }

        public async Task UpdateAsync(Route route)
        {
            using var connection = CreateConnection();
            var sql = @"UPDATE Routes 
                        SET Origin = @Origin, 
                            Destination = @Destination, 
                            Cost = @Cost 
                        WHERE Id = @Id;
                        SELECT @@ROWCOUNT;";
            var rowsAffected = await connection.ExecuteScalarAsync<int>(sql, route);
            if (rowsAffected == 0)
            {
                throw new ArgumentException($"Route with id {route.Id} not found");
            }
        }
    }
} 