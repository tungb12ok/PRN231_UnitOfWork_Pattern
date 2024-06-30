using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.ViewModel;

namespace WebApplication1.Pages.Customers.Auction;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;

    public IndexModel(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public List<AuctionResultVM> AuctionResults { get; set; }

    public async Task OnGetAsync()
    {
        var httpClient = _clientFactory.CreateClient("MyApi");
        var response = await httpClient.GetAsync("odata/AuctionResults?$expand=Auction,Bidder($expand=CustomerDto)");

        if (response.IsSuccessStatusCode)
        {
            var odataResponse = await response.Content.ReadFromJsonAsync<ODataResponse<AuctionResultVM>>();

            if (odataResponse != null)
            {
                AuctionResults = odataResponse.Value ?? new List<AuctionResultVM>();
            }
            else
            {
                AuctionResults = new List<AuctionResultVM>();
            }
        }
        else
        {
            AuctionResults = new List<AuctionResultVM>();
        }
    }
}