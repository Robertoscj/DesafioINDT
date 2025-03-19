using System.Collections.Generic;
using System.Threading.Tasks;
using TravelRoutes.Domain.Entities;

namespace TravelRoutes.Domain.Interfaces
{
    public interface IRouteRepository
    {
        Task<Route?> GetByIdAsync(int id);
        Task<IEnumerable<Route>> GetAllAsync();
        Task<int> AddAsync(Route route);
        Task UpdateAsync(Route route);
        Task DeleteAsync(int id);
        Task<IEnumerable<Route>> GetAllRoutesFromOriginAsync(string origin);
        Task<Route?> GetRouteByOriginAndDestinationAsync(string origin, string destination);
    }
} 