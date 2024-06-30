using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.ViewModel;

namespace WebApplication1.Pages.Customers.Auction;

public class CreateJewelryAndAuctionModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;

    [BindProperty]
    public CreateJewelryAndAuctionVM JewelryAndAuction { get; set; }

    public CreateJewelryAndAuctionModel(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {

        JewelryAndAuction.Status = "Pending";
        var httpClient = _clientFactory.CreateClient("MyApi");
        var response = await httpClient.PostAsJsonAsync("odata/CreateJewelryAndAuction", JewelryAndAuction);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToPage("/Auction/Success");
        }

        ModelState.AddModelError(string.Empty, "Failed to create jewelry and auction request.");
        return Page();
    }
}