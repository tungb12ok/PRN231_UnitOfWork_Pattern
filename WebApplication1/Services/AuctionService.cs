using System.Text;
using Newtonsoft.Json;
using WebApplication1.Pages.Admin.Auctions;
using WebApplication1.Pages.AuctionResults;
using WebApplication1.ViewModel;

namespace WebApplication1.Services;

public class AuctionService
{
    private readonly HttpClient _httpClient;

    public AuctionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<AuctionResult>> GetAuctionResultsAsync()
    {
        var response = await _httpClient.GetAsync("https://localhost:7241/odata/AuctionResults");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Pages.AuctionResults.ODataResponse<AuctionResult>>(content);
        return result.Value;
    }

    public async Task<AuctionResult> GetAuctionResultAsync(int auctionId)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7241/odata/AuctionResults({auctionId})?$expand=Bidder($expand=CustomerDto)");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AuctionResult>(content);
    }
    public async Task CreateAuctionAsync(Auction auction)
    {
        var content = new StringContent(JsonConvert.SerializeObject(auction), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("https://localhost:7241/odata/Auction", content);
        response.EnsureSuccessStatusCode();
    }
    public async Task<List<AuctionRequest>> GetAuctionRequestsAsync()
    {
        var response = await _httpClient.GetAsync("https://localhost:7241/odata/AuctionRequest?$expand=Jewelry");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Pages.AuctionResults.ODataResponse<AuctionRequest>>(content);
        return result.Value;
    }

}

public class AuctionRequest
{
    public int RequestID { get; set; }
    public int CustomerID { get; set; }
    public int JewelryID { get; set; }
    public Jewelry Jewelry { get; set; }  
}

public class Jewelry
{
    public int JewelryID { get; set; }
    public string JewelryName { get; set; }
    public string Discription { get; set; }
    public decimal Price { get; set; }
}