using JewelryAuctionBusiness;
using JewelryAuctionData.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Threading.Tasks;
using JewelryAuctionBusiness.Dto;
using Microsoft.AspNetCore.OData.Formatter;

namespace JewelryAuctionWebAPI.Controllers
{
    public class AuctionRequestController : ODataController
    {
        private readonly RequestAuctionBusiness _auctionBusiness;

        public AuctionRequestController(RequestAuctionBusiness auctionBusiness)
        {
            _auctionBusiness = auctionBusiness;
        }

        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var result = await _auctionBusiness.GetAllRequestAuctions();
            return GenerateActionResult(result);
        }

        [EnableQuery]
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            var result = await _auctionBusiness.GetRequestAuctionById(key);
            return GenerateActionResult(result);
        }

        [HttpPost("odata/CreateJewelryAndAuction")]
        public async Task<IActionResult> CreateJewelryAndAuction([FromBody] CreateJewelryAndAuctionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _auctionBusiness.CreateJewelryAndRequestAuction(dto, dto.CustomerID);
            return GenerateActionResult(result);
        }

        [HttpPost("odata/ApproveAuction")]
        public async Task<IActionResult> ApproveAuction([FromBody] RequestAuctionDetailsDto detailsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _auctionBusiness.ApproveRequestAuction(detailsDto, true);
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
                    return StatusCode(500, result.Message);
            }
        }
    }
}
