using Core.DTOs.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace TripApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService offerService;

        public OfferController(IOfferService offerService)
        {
            this.offerService = offerService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOffer([FromBody] CreateOfferRequest offerDto)
        {
            var result = await offerService.CreateOfferAsync(offerDto);
            return CreatedAtAction(nameof(GetOfferById), new { offerId = result.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOffers()
        {
            var offers = await offerService.GetAllAsync();
            return Ok(offers);
        }
        
        [HttpGet("{offerId}")]
        public async Task<IActionResult> GetOfferById(Guid offerId)
        {
            var offer = await offerService.GetOfferByIdAsync(offerId);
            return Ok(offer);
        }

        [HttpDelete("{offerId}")]
        public async Task<IActionResult> DeleteOffer(Guid offerId)
        {
            var result = await offerService.DeleteOfferAsync(offerId);
            return Ok(result);
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularOffers()
        {
            var offers = await offerService.GetPopular();
            return Ok(offers);
        }

        [HttpGet("recommended")]
        public async Task<IActionResult> GetRecommendedOffers()
        {
            var offers = await offerService.GetRecomendations();
            return Ok(offers);
        }
    }
}
