using FluentValidation;
using TravelRoutes.Application.DTOs;

namespace TravelRoutes.Application.Validators
{
    public class RouteResponseValidator : AbstractValidator<RouteResponseDto>
    {
        public RouteResponseValidator()
        {
            RuleFor(x => x.Path)
                .NotEmpty()
                .WithMessage("O caminho é obrigatório")
                .Matches("^[A-Z]{3}( -> [A-Z]{3})*$")
                .WithMessage("Path must be in the format 'XXX -> YYY -> ZZZ'");

            RuleFor(x => x.TotalCost)
                .GreaterThan(0)
                .WithMessage("O custo total deve ser maior que 0");
        }
    }
} 