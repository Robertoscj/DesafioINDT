using FluentValidation;
using TravelRoutes.Application.DTOs;

namespace TravelRoutes.Application.Validators
{
    public class CreateRouteValidator : AbstractValidator<CreateRouteDto>
    {
        public CreateRouteValidator()
        {
            RuleFor(x => x.Origin)
                .NotEmpty()
                .WithMessage("A origem é obrigatória")
                .Length(3)
                .WithMessage("A origem deve ter exatamente 3 caracteres")
                .Matches("^[A-Z]{3}$")
                .WithMessage("A origem deve ser 3 letras maiúsculas");

            RuleFor(x => x.Destination)
                .NotEmpty()
                .WithMessage("O destino é obrigatório")
                .Length(3)
                .WithMessage("O destino deve ter exatamente 3 caracteres")
                .Matches("^[A-Z]{3}$")
                .WithMessage("O destino deve ser 3 letras maiúsculas");

            RuleFor(x => x.Cost)
                .GreaterThan(0)
                .WithMessage("O custo deve ser maior que 0");

            RuleFor(x => x)
                .Must(x => x.Origin != x.Destination)
                .WithMessage("Origem e destino não podem ser os mesmos");
        }
    }
} 