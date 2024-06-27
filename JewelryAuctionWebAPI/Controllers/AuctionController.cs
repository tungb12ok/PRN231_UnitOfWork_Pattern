using JewelryAuctionBusiness.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;

namespace JewelryAuctionWebAPI.Controllers;

using JewelryAuctionBusiness;
using JewelryAuctionData.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Threading.Tasks;

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

    public async Task<IActionResult> Post([FromBody] AuctionSectionDto auctionSectionDto)
    {
        var result = await _auctionBusiness.CreateAuctionSection(auctionSectionDto);
        return GenerateActionResult(result);
    }

    public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] AuctionSectionDto auctionSectionDto)
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