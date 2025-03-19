namespace TravelRoutes.Application.DTOs
{
    public class RouteDto
    {
        public int Id { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public decimal Cost { get; set; }
    }

    public class CreateRouteDto
    {
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public decimal Cost { get; set; }
    }

    public class RouteResponseDto
    {
        public string Path { get; set; } = string.Empty;
        public decimal TotalCost { get; set; }
    }
} 