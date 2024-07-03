using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Services;

namespace WebApplication1.Pages.AuctionResults;

public class IndexModel : PageModel
{
    private readonly AuctionService _auctionService;

    public IndexModel(AuctionService auctionService)
    {
        _auctionService = auctionService;
    }

    public List<Pages.AuctionResults.AuctionResult> AuctionResults { get; private set; }

    public async Task OnGetAsync()
    {
        AuctionResults = await _auctionService.GetAuctionResultsAsync();
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

