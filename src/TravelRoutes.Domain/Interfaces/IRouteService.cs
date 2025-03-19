using System.Collections.Generic;
using System.Threading.Tasks;
using TravelRoutes.Domain.Entities;

namespace TravelRoutes.Domain.Interfaces
{
    public interface IRouteService
    {
        Task<(List<string> Path, decimal TotalCost)> FindCheapestRoute(string origin, string destination);
        Task<Route> CreateRoute(Route route);
        Task<Route?> GetRoute(int id);
        Task<IEnumerable<Route>> GetAllRoutes();
        Task UpdateRoute(Route route);
        Task DeleteRoute(int id);
    }
} 