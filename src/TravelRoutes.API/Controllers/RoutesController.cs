using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelRoutes.Application.DTOs;
using TravelRoutes.Domain.Interfaces;
using RouteEntity = TravelRoutes.Domain.Entities.Route;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Filters;
using TravelRoutes.API.Examples;

namespace TravelRoutes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class RoutesController : ControllerBase
    {
        private readonly IRouteService _routeService;
        private readonly IMapper _mapper;

        public RoutesController(IRouteService routeService, IMapper mapper)
        {
            _routeService = routeService ?? throw new ArgumentNullException(nameof(routeService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Retorna todas as rotas cadastradas
        /// </summary>
        /// <returns>Lista de rotas</returns>
        /// <response code="200">Retorna a lista de rotas</response>
        [HttpGet]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(RouteDtoExample))]
        [ProducesResponseType(typeof(IEnumerable<RouteDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RouteDto>>> GetAllRoutes()
        {
            var routes = await _routeService.GetAllRoutes();
            return Ok(_mapper.Map<IEnumerable<RouteDto>>(routes));
        }

        /// <summary>
        /// Retorna uma rota específica pelo ID
        /// </summary>
        /// <param name="id">ID da rota</param>
        /// <returns>Dados da rota</returns>
        /// <response code="200">Retorna os dados da rota</response>
        /// <response code="404">Rota não encontrada</response>
        [HttpGet("{id}")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(RouteDtoExample))]
        [ProducesResponseType(typeof(RouteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RouteDto>> GetRoute(int id)
        {
            var route = await _routeService.GetRoute(id);
            if (route == null)
                return NotFound();

            return Ok(_mapper.Map<RouteDto>(route));
        }

        /// <summary>
        /// Cria uma nova rota
        /// </summary>
        /// <param name="routeDto">Dados da rota</param>
        /// <returns>Rota criada</returns>
        /// <response code="201">Rota criada com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerRequestExample(typeof(CreateRouteDto), typeof(CreateRouteDtoExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(RouteDtoExample))]
        [ProducesResponseType(typeof(RouteDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RouteDto>> CreateRoute(CreateRouteDto routeDto)
        {
            var route = _mapper.Map<RouteEntity>(routeDto);
            var createdRoute = await _routeService.CreateRoute(route);
            return CreatedAtAction(nameof(GetRoute), new { id = createdRoute.Id }, _mapper.Map<RouteDto>(createdRoute));
        }

        /// <summary>
        /// Atualiza uma rota existente
        /// </summary>
        /// <param name="id">ID da rota</param>
        /// <param name="routeDto">Dados atualizados da rota</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Rota atualizada com sucesso</response>
        /// <response code="400">ID da rota não corresponde ao ID do objeto</response>
        /// <response code="404">Rota não encontrada</response>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerRequestExample(typeof(RouteDto), typeof(RouteDtoExample))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRoute(int id, RouteDto routeDto)
        {
            if (id != routeDto.Id)
                return BadRequest("Route ID mismatch");

            try
            {
                var route = _mapper.Map<RouteEntity>(routeDto);
                await _routeService.UpdateRoute(route);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while updating the route" });
            }
        }

        /// <summary>
        /// Remove uma rota
        /// </summary>
        /// <param name="id">ID da rota</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Rota removida com sucesso</response>
        /// <response code="404">Rota não encontrada</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            await _routeService.DeleteRoute(id);
            return NoContent();
        }

        /// <summary>
        /// Encontra a rota mais barata entre dois pontos
        /// </summary>
        /// <param name="origin">Código do aeroporto de origem (3 letras)</param>
        /// <param name="destination">Código do aeroporto de destino (3 letras)</param>
        /// <returns>Rota mais barata encontrada</returns>
        /// <response code="200">Retorna a rota mais barata</response>
        /// <response code="400">Parâmetros inválidos</response>
        [HttpGet("cheapest")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(RouteResponseDtoExample))]
        [ProducesResponseType(typeof(RouteResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RouteResponseDto>> FindCheapestRoute(
            [FromQuery, Required, RegularExpression("^[A-Z]{3}$")] string origin,
            [FromQuery, Required, RegularExpression("^[A-Z]{3}$")] string destination)
        {
            try
            {
                var result = await _routeService.FindCheapestRoute(origin, destination);
                var response = _mapper.Map<RouteResponseDto>(result);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
} 