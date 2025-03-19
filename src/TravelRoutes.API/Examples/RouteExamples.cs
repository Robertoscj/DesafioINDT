using Swashbuckle.AspNetCore.Filters;
using TravelRoutes.Application.DTOs;

namespace TravelRoutes.API.Examples
{
    public class CreateRouteDtoExample : IExamplesProvider<CreateRouteDto>
    {
        public CreateRouteDto GetExamples()
        {
            return new CreateRouteDto
            {
                Origin = "GRU",
                Destination = "CDG",
                Cost = 75.00m
            };
        }
    }

    public class RouteDtoExample : IExamplesProvider<RouteDto>
    {
        public RouteDto GetExamples()
        {
            return new RouteDto
            {
                Id = 1,
                Origin = "GRU",
                Destination = "CDG",
                Cost = 75.00m
            };
        }
    }

    public class RouteResponseDtoExample : IExamplesProvider<RouteResponseDto>
    {
        public RouteResponseDto GetExamples()
        {
            return new RouteResponseDto
            {
                Path = "GRU -> BRC -> SCL -> ORL -> CDG",
                TotalCost = 40.00m
            };
        }
    }
} 