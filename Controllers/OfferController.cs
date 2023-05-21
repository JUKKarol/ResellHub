using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellHub.DTOs.OfferDTOs;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Services.OfferServices;
using ResellHub.Services.UserServices;

namespace ResellHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpGet("offers")]
        public async Task<IActionResult> GetOffers()
        {
            return Ok(await _offerService.GetOffers());
        }

        [HttpGet("offers/{userId}")]
        public async Task<IActionResult> GetUserOffers(Guid userId)
        {
            return Ok(await _offerService.GetUserOffers(userId));
        }

        [HttpPost("offers")]
        public async Task<IActionResult> CreateOffer(OfferCreateDto offerDto)
        {
            var actionInfo = await _offerService.AddOffer(offerDto);

            return Ok(actionInfo);
        }

        [HttpPut("offers/{offerId}")]
        public async Task<IActionResult> UpdateOffer(Guid offerId, OfferCreateDto offerDto)
        {
            var actionInfo = await _offerService.UpdateOffer(offerId, offerDto);

            return Ok(actionInfo);
        }

        [HttpDelete("offers/{offerId}")]
        public async Task<IActionResult> DeleteOffer(Guid offerId)
        {
            var actionInfo = await _offerService.DeleteOffer(offerId);

            return Ok(actionInfo);
        }
    }
}
