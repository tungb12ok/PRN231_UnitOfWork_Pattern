using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApplication1.ViewModel;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Pages.Customers.Auction
{
    public class ApproveAuctionModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<ApproveAuctionModel> _logger;

        public List<RequestAuctionDetailVM> AuctionRequests { get; set; }
        
        public ApproveAuctionModel(IHttpClientFactory clientFactory, ILogger<ApproveAuctionModel> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        [BindProperty] 
        public ApproveViewModel Approve { get; set; }

        public async Task OnGetAsync()
        {
            var httpClient = _clientFactory.CreateClient("MyApi");
            var response = await httpClient.GetAsync("odata/RequestAuctionDetail");

            if (response.IsSuccessStatusCode)
            {
                var odataResponse = await response.Content.ReadFromJsonAsync<ODataResponse<RequestAuctionDetailVM>>();
                AuctionRequests = odataResponse?.Value ?? new List<RequestAuctionDetailVM>();
            }
            else
            {
                AuctionRequests = new List<RequestAuctionDetailVM>();
            }
        }

        public async Task<IActionResult> OnPostApproveAsync()
        {
            return await UpdateAuctionRequestStatus("Approved");
        }

        public async Task<IActionResult> OnPostRejectAsync()
        {
            return await UpdateAuctionRequestStatus("Rejected");
        }

        private async Task<IActionResult> UpdateAuctionRequestStatus(string status)
        {
            if (Approve == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid request. Please try again.");
                return Page();
            }

            var httpClient = _clientFactory.CreateClient("MyApi");
            var requestUri = $"odata/RequestAuctionDetail({Approve.id})?status={status}";

            // Log the request URI for debugging
            _logger.LogInformation("Sending PUT request to {RequestUri} with status {Status}", requestUri, status);

            var response = await httpClient.PutAsync(requestUri, null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage();
            }
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to update auction request status. Status Code: {StatusCode}, Response: {Response}", response.StatusCode, errorContent);

            ModelState.AddModelError(string.Empty, "Failed to update auction request status.");
            return Page();
        }
    }
}
