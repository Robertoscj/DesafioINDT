using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using TravelRoutes.API.Controllers;
using TravelRoutes.Domain.Interfaces;
using TravelRoutes.Application.DTOs;
using TravelRoutes.Application.Services;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using TravelRoutes.Domain.Entities;

namespace TravelRoutes.Tests.Controllers
{
    public class RoutesControllerTests
    {
        private readonly Mock<IRouteService> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly RoutesController _controller;

        public RoutesControllerTests()
        {
            _mockService = new Mock<IRouteService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new RoutesController(_mockService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllRoutes_ShouldReturnOkResult()
        {
            // Arrange
            var routes = new List<Route>
            {
                new Route("GRU", "CDG", 75) { Id = 1 }
            };
            var routeDtos = new List<RouteDto>
            {
                new RouteDto { Id = 1, Origin = "GRU", Destination = "CDG", Cost = 75 }
            };

            _mockService.Setup(s => s.GetAllRoutes()).ReturnsAsync(routes);
            _mockMapper.Setup(m => m.Map<IEnumerable<RouteDto>>(routes)).Returns(routeDtos);

            // Act
            var result = await _controller.GetAllRoutes();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(routeDtos);
        }

        [Fact]
        public async Task GetRoute_WithValidId_ShouldReturnOkResult()
        {
            // Arrange
            var route = new Route("GRU", "CDG", 75) { Id = 1 };
            var routeDto = new RouteDto { Id = 1, Origin = "GRU", Destination = "CDG", Cost = 75 };

            _mockService.Setup(s => s.GetRoute(1)).ReturnsAsync(route);
            _mockMapper.Setup(m => m.Map<RouteDto>(route)).Returns(routeDto);

            // Act
            var result = await _controller.GetRoute(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(routeDto);
        }

        [Fact]
        public async Task GetRoute_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            Route? nullRoute = null;
            _mockService.Setup(s => s.GetRoute(1)).ReturnsAsync(() => nullRoute);

            // Act
            var result = await _controller.GetRoute(1);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CreateRoute_WithValidRoute_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var createRouteDto = new CreateRouteDto { Origin = "GRU", Destination = "CDG", Cost = 75 };
            var route = new Route("GRU", "CDG", 75);
            var createdRoute = new Route("GRU", "CDG", 75) { Id = 1 };
            var routeDto = new RouteDto { Id = 1, Origin = "GRU", Destination = "CDG", Cost = 75 };

            _mockMapper.Setup(m => m.Map<Route>(createRouteDto)).Returns(route);
            _mockService.Setup(s => s.CreateRoute(route)).ReturnsAsync(createdRoute);
            _mockMapper.Setup(m => m.Map<RouteDto>(createdRoute)).Returns(routeDto);

            // Act
            var result = await _controller.CreateRoute(createRouteDto);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult!.ActionName.Should().Be(nameof(RoutesController.GetRoute));
            createdAtActionResult.RouteValues!["id"].Should().Be(1);
            createdAtActionResult.Value.Should().BeEquivalentTo(routeDto);
        }

        [Fact]
        public async Task UpdateRoute_WithValidRoute_ShouldReturnNoContent()
        {
            // Arrange
            var routeDto = new RouteDto { Id = 1, Origin = "GRU", Destination = "CDG", Cost = 75 };
            var route = new Route("GRU", "CDG", 75) { Id = 1 };

            _mockMapper.Setup(m => m.Map<Route>(routeDto)).Returns(route);
            _mockService.Setup(s => s.UpdateRoute(route)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateRoute(1, routeDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateRoute_WithMismatchedId_ShouldReturnBadRequest()
        {
            // Arrange
            var routeDto = new RouteDto { Id = 2, Origin = "GRU", Destination = "CDG", Cost = 75 };

            // Act
            var result = await _controller.UpdateRoute(1, routeDto);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task DeleteRoute_WithValidId_ShouldReturnNoContent()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteRoute(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteRoute(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task FindCheapestRoute_WithValidParameters_ShouldReturnOkResult()
        {
            // Arrange
            var routeResult = (new List<string> { "GRU", "CDG" }, 75m);
            var routeResponseDto = new RouteResponseDto { Path = "GRU -> CDG", TotalCost = 75 };

            _mockService.Setup(s => s.FindCheapestRoute("GRU", "CDG")).ReturnsAsync(routeResult);
            _mockMapper.Setup(m => m.Map<RouteResponseDto>(routeResult)).Returns(routeResponseDto);

            // Act
            var result = await _controller.FindCheapestRoute("GRU", "CDG");

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(routeResponseDto);
        }

        [Fact]
        public async Task FindCheapestRoute_WhenRouteNotFound_ShouldReturnBadRequest()
        {
            // Arrange
            _mockService.Setup(s => s.FindCheapestRoute("GRU", "CDG"))
                .ThrowsAsync(new ArgumentException("Rota não encontrada"));

            // Act
            var result = await _controller.FindCheapestRoute("GRU", "CDG");

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.Value.Should().Be("Rota não encontrada");
        }
    }
} 