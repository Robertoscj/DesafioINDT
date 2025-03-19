using Xunit;
using FluentAssertions;
using TravelRoutes.Application.Validators;
using TravelRoutes.Application.DTOs;

namespace TravelRoutes.Tests.Validators
{
    public class RouteValidatorTests
    {
        private readonly RouteValidator _validator;

        public RouteValidatorTests()
        {
            _validator = new RouteValidator();
        }

        [Fact]
        public void Validate_WithValidRoute_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var route = new RouteDto
            {
                Id = 1,
                Origin = "GRU",
                Destination = "CDG",
                Cost = 75.00m
            };

            // Act
            var result = _validator.Validate(route);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Validate_WithInvalidId_ShouldHaveValidationError(int id)
        {
            // Arrange
            var route = new RouteDto
            {
                Id = id,
                Origin = "GRU",
                Destination = "CDG",
                Cost = 75.00m
            };

            // Act
            var result = _validator.Validate(route);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Id");
        }

        [Theory]
        [InlineData("")]
        [InlineData("GR")]
        [InlineData("GRUU")]
        [InlineData("123")]
        [InlineData("gru")]
        public void Validate_WithInvalidOrigin_ShouldHaveValidationError(string origin)
        {
            // Arrange
            var route = new RouteDto
            {
                Id = 1,
                Origin = origin,
                Destination = "CDG",
                Cost = 75.00m
            };

            // Act
            var result = _validator.Validate(route);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Origin");
        }

        [Theory]
        [InlineData("")]
        [InlineData("CD")]
        [InlineData("CDGG")]
        [InlineData("123")]
        [InlineData("cdg")]
        public void Validate_WithInvalidDestination_ShouldHaveValidationError(string destination)
        {
            // Arrange
            var route = new RouteDto
            {
                Id = 1,
                Origin = "GRU",
                Destination = destination,
                Cost = 75.00m
            };

            // Act
            var result = _validator.Validate(route);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Destination");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Validate_WithInvalidCost_ShouldHaveValidationError(decimal cost)
        {
            // Arrange
            var route = new RouteDto
            {
                Id = 1,
                Origin = "GRU",
                Destination = "CDG",
                Cost = cost
            };

            // Act
            var result = _validator.Validate(route);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Cost");
        }

        [Fact]
        public void Validate_WithSameOriginAndDestination_ShouldHaveValidationError()
        {
            // Arrange
            var route = new RouteDto
            {
                Id = 1,
                Origin = "GRU",
                Destination = "GRU",
                Cost = 75.00m
            };

            // Act
            var result = _validator.Validate(route);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage == "Origem e destino não podem ser os mesmos");
        }
    }
} 