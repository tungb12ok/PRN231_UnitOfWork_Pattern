using JewelryAuctionBusiness;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Formatter;

namespace JewelryAuctionWebAPI.Controllers
{
    public class AuctionResultsController : ControllerBase
    {
        private readonly AuctionResultBusiness _auctionResultBusiness;

        public AuctionResultsController(AuctionResultBusiness auctionResultBusiness)
        {
            _auctionResultBusiness = auctionResultBusiness;
        }
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var result = await _auctionResultBusiness.GetAllAuctionResultsAsync();
            return GenerateActionResult(result);
        }
        [EnableQuery]
        public async Task<IActionResult> Get(int key)
        {
            var result = await _auctionResultBusiness.GetAuctionResultByIdAsync(key);
            return GenerateActionResult(result);
        }

        private IActionResult GenerateActionResult(IBusinessResult result)
        {
            switch (result.Status)
            {
                case 400:
                    return BadRequest(result);
                case 404:
                    return NotFound(result);
                case 200:
                    return Ok(result.Data);
                default:
                    return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }
    }
}