using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelRoutes.Domain.Entities;
using TravelRoutes.Domain.Interfaces;

namespace TravelRoutes.Application.Services
{
    public class RouteService : IRouteService
    {
        private readonly IRouteRepository _routeRepository;

        public RouteService(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository ?? throw new ArgumentNullException(nameof(routeRepository));
        }

        public async Task<Route> CreateRoute(Route route)
        {
            var id = await _routeRepository.AddAsync(route);
            var createdRoute = await _routeRepository.GetByIdAsync(id);
            if (createdRoute == null)
            {
                throw new InvalidOperationException($"Could not find route with id {id} after creation");
            }
            return createdRoute;
        }

        public async Task DeleteRoute(int id)
        {
            await _routeRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Route>> GetAllRoutes()
        {
            return await _routeRepository.GetAllAsync();
        }

        public async Task<Route?> GetRoute(int id)
        {
            return await _routeRepository.GetByIdAsync(id);
        }

        public async Task UpdateRoute(Route route)
        {
            var existingRoute = await _routeRepository.GetByIdAsync(route.Id);
            if (existingRoute == null)
            {
                throw new ArgumentException($"Route with id {route.Id} not found");
            }
            await _routeRepository.UpdateAsync(route);
        }

        public async Task<(List<string> Path, decimal TotalCost)> FindCheapestRoute(string origin, string destination)
        {
            var routes = await _routeRepository.GetAllAsync();
            var graph = BuildGraph(routes);
            
            if (!graph.ContainsKey(origin) || !graph.ContainsKey(destination))
            {
                throw new ArgumentException("Origin or destination not found in routes");
            }

            var distances = new Dictionary<string, decimal>();
            var previous = new Dictionary<string, string?>();
            var unvisited = new HashSet<string>();

            foreach (var vertex in graph.Keys)
            {
                distances[vertex] = decimal.MaxValue;
                previous[vertex] = null;
                unvisited.Add(vertex);
            }

            distances[origin] = 0;

            while (unvisited.Count > 0)
            {
                var current = unvisited.OrderBy(v => distances[v]).First();
                
                if (current == destination)
                    break;

                unvisited.Remove(current);

                foreach (var neighbor in graph[current])
                {
                    var alt = distances[current] + neighbor.Cost;
                    if (alt < distances[neighbor.Destination])
                    {
                        distances[neighbor.Destination] = alt;
                        previous[neighbor.Destination] = current;
                    }
                }
            }

            var path = new List<string>();
            var current_vertex = destination;

            while (current_vertex != null)
            {
                path.Insert(0, current_vertex);
                current_vertex = previous[current_vertex];
            }

            return (path, distances[destination]);
        }

        private Dictionary<string, List<Route>> BuildGraph(IEnumerable<Route> routes)
        {
            var graph = new Dictionary<string, List<Route>>();

            foreach (var route in routes)
            {
                if (!graph.ContainsKey(route.Origin))
                    graph[route.Origin] = new List<Route>();
                
                if (!graph.ContainsKey(route.Destination))
                    graph[route.Destination] = new List<Route>();

                graph[route.Origin].Add(route);
            }

            return graph;
        }
    }
} 