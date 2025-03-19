using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using TravelRoutes.Domain.Entities;
using TravelRoutes.Domain.Interfaces;
using TravelRoutes.Application.Services;
using TravelRoutes.Application.DTOs;
using Xunit;
using FluentAssertions;

namespace TravelRoutes.Tests
{
    public class RouteServiceTests
    {
        private readonly Mock<IRouteRepository> _mockRepository;
        private readonly RouteService _service;

        public RouteServiceTests()
        {
            _mockRepository = new Mock<IRouteRepository>();
            _service = new RouteService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllRoutes_ShouldReturnAllRoutes()
        {
            // Arrange
            var expectedRoutes = new List<Route>
            {
                new Route("GRU", "BRC", 10) { Id = 1 },
                new Route("BRC", "SCL", 5) { Id = 2 }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(expectedRoutes);

            // Act
            var result = await _service.GetAllRoutes();

            // Assert
            result.Should().BeEquivalentTo(expectedRoutes);
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetRoute_WithValidId_ShouldReturnRoute()
        {
            // Arrange
            var expectedRoute = new Route("GRU", "BRC", 10) { Id = 1 };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(expectedRoute);

            // Act
            var result = await _service.GetRoute(1);

            // Assert
            result.Should().BeEquivalentTo(expectedRoute);
            _mockRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task CreateRoute_WithValidRoute_ShouldReturnCreatedRoute()
        {
            // Arrange
            var routeToCreate = new Route("GRU", "BRC", 10);
            var createdRoute = new Route("GRU", "BRC", 10) { Id = 1 };

            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Route>()))
                .ReturnsAsync(1);

            _mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(createdRoute);

            // Act
            var result = await _service.CreateRoute(routeToCreate);

            // Assert
            result.Should().BeEquivalentTo(createdRoute);
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Route>()), Times.Once);
        }

        [Fact]
        public async Task UpdateRoute_WithValidRoute_ShouldNotThrowException()
        {
            // Arrange
            var routeToUpdate = new Route("GRU", "BRC", 10) { Id = 1 };
            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Route>()))
                .Returns(Task.CompletedTask);

            // Act
            Func<Task> act = async () => await _service.UpdateRoute(routeToUpdate);

            // Assert
            await act.Should().NotThrowAsync();
            _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Route>()), Times.Once);
        }

        [Fact]
        public async Task DeleteRoute_WithValidId_ShouldNotThrowException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            Func<Task> act = async () => await _service.DeleteRoute(1);

            // Assert
            await act.Should().NotThrowAsync();
            _mockRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task FindCheapestRoute_WithValidOriginAndDestination_ShouldReturnCheapestRoute()
        {
            // Arrange
            var routes = new List<Route>
            {
                new Route("GRU", "BRC", 10) { Id = 1 },
                new Route("BRC", "SCL", 5) { Id = 2 },
                new Route("GRU", "SCL", 20) { Id = 3 }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(routes);

            // Act
            var result = await _service.FindCheapestRoute("GRU", "SCL");

            // Assert
            result.Path.Should().BeEquivalentTo(new List<string> { "GRU", "BRC", "SCL" });
            result.TotalCost.Should().Be(15);
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Theory]
        [InlineData("", "SCL")]
        [InlineData("GRU", "")]
        public async Task FindCheapestRoute_WithInvalidParameters_ShouldThrowArgumentException(string origin, string destination)
        {
            // Act
            Func<Task> act = async () => await _service.FindCheapestRoute(origin, destination);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task FindCheapestRoute_WhenNoRouteExists_ShouldThrowArgumentException()
        {
            // Arrange
            var routes = new List<Route>
            {
                new Route("GRU", "BRC", 10) { Id = 1 }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(routes);

            // Act
            Func<Task> act = async () => await _service.FindCheapestRoute("GRU", "SCL");

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Origin or destination not found in routes");
        }
    }
} 