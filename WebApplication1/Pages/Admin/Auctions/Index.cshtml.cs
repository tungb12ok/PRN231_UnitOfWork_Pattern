using JewelryAuctionBusiness.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApplication1.ViewModel;

namespace WebApplication1.Pages.Admin.Auctions
{
    public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;

    public IndexModel(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public IList<AuctionSectionDto> AuctionSections { get; set; } = new List<AuctionSectionDto>();

    [BindProperty] public AuctionSectionUpdateVM? AuctionSection { get; set; } = null;

    public SelectList StatusOptions { get; set; }

    public async Task OnGetAsync()
    {
        await LoadModelAuctions();
        PopulateStatusOptions();
    }

    public async Task<IActionResult> OnPostEditAsync(int id)
    {
        var httpClient = _clientFactory.CreateClient("MyApi");
        var response = await httpClient.GetFromJsonAsync<AuctionSectionUpdateVM>($"odata/Auction({id})");

        if (response != null)
        {
            return new JsonResult(response);
        }

        return new JsonResult(new { success = false, message = "Failed to load auction details." });
    }


    public async Task<IActionResult> OnPostSaveAsync()
    {
        var httpClient = _clientFactory.CreateClient("MyApi");
        HttpResponseMessage response;

        if (AuctionSection.AuctionID == 0)
        {
            response = await httpClient.PostAsJsonAsync("odata/Auction", AuctionSection);
        }
        else
        {
            response = await httpClient.PutAsJsonAsync($"odata/Auction({AuctionSection.AuctionID})", AuctionSection);
        }

        if (response.IsSuccessStatusCode)
        {
            return RedirectToPage("./Index");
        }

        ModelState.AddModelError(string.Empty, "Failed to save auction.");
        await LoadModelAuctions();
        PopulateStatusOptions();
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await LoadModelAuctions();
        PopulateStatusOptions();

        var httpClient = _clientFactory.CreateClient("MyApi");
        var response = await httpClient.GetFromJsonAsync<AuctionSectionUpdateVM>($"odata/Auction({id})");

        if (response != null)
        {
            AuctionSection = response;
            return Page();
        }

        ModelState.AddModelError(string.Empty, "Failed to load auction details.");
        return Page();
    }

    public async Task<IActionResult> OnPostConfirmDeleteAsync()
    {
        var httpClient = _clientFactory.CreateClient("MyApi");
        var response = await httpClient.DeleteAsync($"odata/Auction({AuctionSection.AuctionID})");

        if (response.IsSuccessStatusCode)
        {
            return RedirectToPage("./Index");
        }

        ModelState.AddModelError(string.Empty, "Failed to delete auction.");
        await LoadModelAuctions();
        PopulateStatusOptions();
        return Page();
    }

    private async Task LoadModelAuctions()
    {
        var httpClient = _clientFactory.CreateClient("MyApi");
        var response = await httpClient.GetFromJsonAsync<ODataResponse<AuctionSectionDto>>("odata/Auction");

        AuctionSections = response?.Value ?? new List<AuctionSectionDto>();
    }

    private void PopulateStatusOptions()
    {
        var statusOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "Active", Text = "Active" },
            new SelectListItem { Value = "Closed", Text = "Closed" }
        };

        StatusOptions = new SelectList(statusOptions, "Value", "Text");
    }
}

}
