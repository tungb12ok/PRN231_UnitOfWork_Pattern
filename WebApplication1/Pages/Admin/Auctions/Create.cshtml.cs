using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Services;

namespace WebApplication1.Pages.Admin.Auctions
{
    public class CreateModel : PageModel
    {
        private readonly AuctionService _auctionService;

        public CreateModel(AuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        [BindProperty]
        public ViewModel.Auction Auction { get; set; }
        
        [BindProperty]
        public List<AuctionRequest> AuctionRequests { get; set; }

        public List<SelectListItem> JewelryItems { get; set; }

        public async Task OnGetAsync()
        {
            AuctionRequests = await _auctionService.GetAuctionRequestsAsync();
            JewelryItems = AuctionRequests.Select(ar => new SelectListItem
            {
                Value = ar.RequestID.ToString(),
                Text = ar.Jewelry.JewelryName
            }).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Auction.Status = "Active";
            await _auctionService.CreateAuctionAsync(Auction);

            return RedirectToPage("Index");
        }
    }

    public class ODataResponse<T>
    {
        public List<T> Value { get; set; }
    }
}