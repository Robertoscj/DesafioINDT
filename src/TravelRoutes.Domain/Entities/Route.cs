using System;

namespace TravelRoutes.Domain.Entities
{
    public class Route
    {
        public int Id { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public decimal Cost { get; set; }

        public Route(string origin, string destination, decimal cost)
        {
            Origin = origin ?? throw new ArgumentNullException(nameof(origin));
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));
            Cost = cost >= 0 ? cost : throw new ArgumentException("Cost must be non-negative", nameof(cost));
        }

        // For ORM
        protected Route() { }
    }
} 