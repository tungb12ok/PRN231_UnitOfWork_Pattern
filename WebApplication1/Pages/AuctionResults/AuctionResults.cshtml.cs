using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApplication1.Pages.AuctionResults
{
    public class AuctionResultsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public AuctionResultsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public AuctionResult AuctionResult { get; private set; }

        public async Task OnGetAsync(int id)
        {
            AuctionResult = await GetAuctionResultAsync(id);
        }

        private async Task<AuctionResult> GetAuctionResultAsync(int auctionId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7241/odata/AuctionResults({auctionId})?$expand=Bidder($expand=CustomerDto)");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AuctionResult>(content);
        }
    }

    public class ODataResponse<T>
    {
        public List<T> Value { get; set; }
    }

    public class AuctionResult
    {
        public int AuctionId { get; set; }
        public int BidderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionTime { get; set; }
        public decimal FinalPrice { get; set; }
        public Bidder Bidder { get; set; }
    }

    public class Bidder
    {
        public int BidderId { get; set; }
        public int CustomerId { get; set; }
        public decimal CurrentBidPrice { get; set; }
        public CustomerDto CustomerDto { get; set; }
    }

    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public int CompanyId { get; set; }
        public string Email { get; set; }
    }
}