using JewelryAuctionBusiness;
using JewelryAuctionData.Entity;
using JewelryAuctionData.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace JewelryAuctionWebAPI.Controllers;

public class RequestAuctionDetailController : ODataController
{
    private readonly RequestAuctionBusiness _auctionBusiness;

    public RequestAuctionDetailController(RequestAuctionBusiness auctionBusiness)
    {
        _auctionBusiness = auctionBusiness;
    }

    [EnableQuery]
    public async Task<IActionResult> Get()
    {
        var result = await _auctionBusiness.GetAllRequestDetail();
        return GenerateActionResult(result);
    }

    [EnableQuery]
    public async Task<IActionResult> Get(int key)
    {
        var result = await _auctionBusiness.GetAllRequestDetailByKey(key);
        return GenerateActionResult(result);
    }

    public async Task<IActionResult> Put(int key, string status)
    {
        if (_auctionBusiness.GetAllRequestDetailByKey(key) == null)
        {
            return BadRequest("The ID in the URL does not match the ID in the entity.");
        }

        switch (status)
        {
            case "Pending":
            case "Rejected":
            case "Approved":
                var result = await _auctionBusiness.UpdateRequestAuctionDetailsStatus(key, status);
                return GenerateActionResult(result);
            default:
                return BadRequest("The status must be contain status enum.");
        }
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