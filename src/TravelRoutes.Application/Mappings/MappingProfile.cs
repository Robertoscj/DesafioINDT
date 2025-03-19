using AutoMapper;
using System.Linq;
using TravelRoutes.Application.DTOs;
using TravelRoutes.Domain.Entities;

namespace TravelRoutes.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Route, RouteDto>();
            CreateMap<CreateRouteDto, Route>();
            CreateMap<RouteDto, Route>();
            CreateMap<(System.Collections.Generic.List<string> Path, decimal TotalCost), RouteResponseDto>()
                .ForMember(dest => dest.Path, opt => opt.MapFrom(src => string.Join(" - ", src.Path)))
                .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.TotalCost));
        }
    }
} 