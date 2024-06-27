using JewelryAuctionBusiness;
using JewelryAuctionBusiness.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace JewelryAuctionWebAPI.Controllers;

public class PaymentsController : ODataController
{
    private readonly PaymentBusiness _paymentBusiness;

    public PaymentsController(PaymentBusiness paymentBusiness)
    {
        _paymentBusiness = paymentBusiness;
    }

    [EnableQuery]
    public async Task<IActionResult> Get()
    {
        var result = await _paymentBusiness.GetAllPayments();
        return GenerateActionResult(result);
    }

    [EnableQuery]
    public async Task<IActionResult> Get([FromODataUri] int key)
    {
        var result = await _paymentBusiness.GetPaymentById(key);
        return GenerateActionResult(result);
    }

    public async Task<IActionResult> Post([FromBody] PaymentDto paymentDto)
    {
        var result = await _paymentBusiness.RecordPayment(paymentDto);
        return GenerateActionResult(result);
    }

    public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] PaymentDto paymentDto)
    {
        var result = await _paymentBusiness.UpdatePayment(key, paymentDto);
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