using JewelryAuctionBusiness;
using JewelryAuctionData.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Threading.Tasks;
using JewelryAuctionData.Dto;
using Microsoft.AspNetCore.OData.Formatter;
using NuGet.Protocol;

namespace JewelryAuctionWebAPI.Controllers
{
    public class CustomersController : ControllerBase
    {
        private readonly CustomerBusiness _customerBusiness;

        public CustomersController(CustomerBusiness customerBusiness)
        {
            _customerBusiness = customerBusiness;
        }

        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var result = await _customerBusiness.GetAllCustomers();
            return GenerateActionResult(result);
        }

        [EnableQuery]
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            var result = await _customerBusiness.GetCustomerById(key);
            return GenerateActionResult(result);
        }
        [HttpGet("Companies")]
        public async Task<IActionResult> GetCompany()
        {
            var result = await _customerBusiness.GetAllCompany();
            return GenerateActionResult(result);
        }
        
        public async Task<IActionResult> Post([FromBody] CustomerDTO customer)
        {
            var result = await _customerBusiness.CreateCustomer(customer);
            return GenerateActionResult(result);
        }

        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] CustomerDTO customer)
        {
            if (key != customer.CustomerId)
            {
                return BadRequest("The ID in the URL does not match the ID in the entity.");
            }

            var result = await _customerBusiness.UpdateCustomer(customer);
            return GenerateActionResult(result);
        }

        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var result = await _customerBusiness.DeleteCustomer(key);
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
