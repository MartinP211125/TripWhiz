using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Core.DTOs.Request;

namespace TripApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransportationController : ControllerBase
    {
        private readonly ITransportationService transportationService;

        public TransportationController(ITransportationService transportationService)
        {
            this.transportationService = transportationService;
        }

        [HttpPost("availability")]
        public async Task<IActionResult> CreateAvailability([FromBody] TransportationAvailabilityDto request)
        {
            var result = await transportationService.CreateTransportationAvailabilityAsync(request);
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var results = await transportationService.GetAllTransportationsAsync();
            return Ok(results);
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopular()
        {
            var results = await transportationService.GetPopular();
            return Ok(results);
        }

        [HttpGet("recommended")]
        public async Task<IActionResult> GetRecommended()
        {
            var results = await transportationService.GetRecomendations();
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, [FromQuery] DateTime? departureTime, [FromQuery] DateTime? arrivalTime)
        {
            var result = await transportationService.GetTransportationAsync(id, departureTime, arrivalTime);
            return Ok(result);
        }
    }
}
