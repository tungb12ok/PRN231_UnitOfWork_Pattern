using JewelryAuctionBusiness;
using JewelryAuctionBusiness.Dto;
using JewelryAuctionData.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Formatter;

namespace JewelryAuctionWebAPI.Controllers
{
    public class AuctionController : ODataController
    {
        private readonly AuctionBusiness _auctionBusiness;

        public AuctionController(AuctionBusiness auctionBusiness)
        {
            _auctionBusiness = auctionBusiness;
        }

        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var result = await _auctionBusiness.GetScheduledAuctions();
            return GenerateActionResult(result);
        }

        [EnableQuery]
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            var result = await _auctionBusiness.GetAuctionSectionById(key);
            return GenerateActionResult(result);
        }

        public async Task<IActionResult> Post([FromBody] AuctionSectionUpdateDto auctionSectionDto)
        {
            var result = await _auctionBusiness.CreateAuctionSection(auctionSectionDto);
            return GenerateActionResult(result);
        }

        public async Task<IActionResult> Put(int key, [FromBody] AuctionSectionUpdateDto auctionSectionDto)
        {
            if (key != auctionSectionDto.AuctionID)
            {
                return BadRequest("The ID in the URL does not match the ID in the entity.");
            }

            var result = await _auctionBusiness.UpdateAuctionSection(key, auctionSectionDto);
            return GenerateActionResult(result);
        }

        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var result = await _auctionBusiness.DeleteAuctionSection(key);
            return GenerateActionResult(result);
        }

        private IActionResult GenerateActionResult(IBusinessResult result)
        {
            return result.Status switch
            {
                200 => Ok(result.Data),
                400 => BadRequest(result),
                404 => NotFound(result),
                _ => StatusCode(500, "An internal server error occurred. Please try again later.")
            };
        }
    }
}
