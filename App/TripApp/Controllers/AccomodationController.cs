using Core.DTOs.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace TripApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccomodationController : ControllerBase
    {
        private readonly IAccomodationService accomodationService;

        public AccomodationController(IAccomodationService accomodationService)
        {
            this.accomodationService = accomodationService;
        }

        [HttpPost("availability")]
        public async Task<IActionResult> CreateAvailability([FromBody] AccomodationAvailabilityDto request)
        {
            var result = await accomodationService.CreateAccomodationAvailabilityAsync(request);
            return Ok(result);
        }

        [HttpGet("{accomodationId}")]
        public async Task<IActionResult> GetById(Guid accomodationId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var result = await accomodationService.GetAccomodationAsync(accomodationId, fromDate, toDate);
            return Ok(result);
        }

        [HttpGet("place/{place}")]
        public async Task<IActionResult> GetByPlace(string place)
        {
            var results = await accomodationService.GetAccomodationsByPlaceAsync(place);
            return Ok(results);
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopular()
        {
            var results = await accomodationService.GetPopular();
            return Ok(results);
        }

        [HttpGet("recommended")]
        public async Task<IActionResult> GetRecommended()
        {
            var results = await accomodationService.GetRecomendations();
            return Ok(results);
        }
    }
}
