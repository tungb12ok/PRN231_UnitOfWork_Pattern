using JewelryAuctionBusiness;
using JewelryAuctionBusiness.Dto;
using Microsoft.AspNetCore.Mvc;

namespace JewelryAuctionWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BidderController : ControllerBase
{
    private readonly BidderBusiness _bidderBusiness;

    public BidderController(BidderBusiness bidderBusiness)
    {
        _bidderBusiness = bidderBusiness;
    }

    [HttpPost("place-bid")]
    public async Task<IActionResult> PlaceBid([FromBody] BidderAuction bidderDto)
    {
        var result = await _bidderBusiness.PlaceBid(bidderDto);
        return GenerateActionResult(result);
    }

    [HttpGet("get-bidder/{customerId}")]
    public async Task<IActionResult> GetBidderByCustomerId(int customerId)
    {
        var result = await _bidderBusiness.GetBidderByCustomerId(customerId);
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
                if (result.Data == null)
                {
                    return Ok(result);
                }
                return Ok(result.Data);
            default:
                return StatusCode(500, "An internal server error occurred. Please try again later.");
        }
    }
}