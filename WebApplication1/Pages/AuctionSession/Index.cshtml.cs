using JewelryAuctionBusiness.Dto;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.ViewModel;

namespace WebApplication1.Pages.AuctionSession;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;

    public IndexModel(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public IList<AuctionSectionDto> AuctionSections { get; set; } = new List<AuctionSectionDto>();

    public async Task OnGetAsync()
    {
        var httpClient = _clientFactory.CreateClient("MyApi");
        var response = await httpClient.GetFromJsonAsync<ODataResponse<AuctionSectionDto>>("odata/Auction");
    
        AuctionSections = response?.Value.Where(x => x.Status.Equals("Active")).ToList() ?? new List<AuctionSectionDto>().Where(x => x.Status.Equals("Active")).ToList();
    }
}