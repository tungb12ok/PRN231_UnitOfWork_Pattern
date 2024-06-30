using System.Text.Json;
using JewelryAuctionData.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.ViewModel;

namespace WebApplication1.Pages.Customers;

public class RegisterCustomerModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;

    public RegisterCustomerModel(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    [BindProperty] public CustomerRegistrationViewModel Customer { get; set; }

    public List<SelectListItem> Companies { get; set; }

    public async Task OnGetAsync()
    {
        await LoadCompaniesAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadCompaniesAsync();
            return Page();
        }

        var httpClient = _clientFactory.CreateClient("MyApi");
        var response = await httpClient.PostAsJsonAsync("odata/Customers", Customer);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToPage("CustomerList");
        }

        ModelState.AddModelError(string.Empty, "An error occurred while creating the customer.");
        await LoadCompaniesAsync();
        return RedirectToPage("/Index");
    }

    private async Task LoadCompaniesAsync()
    {
        var httpClient = _clientFactory.CreateClient("MyApi");
        var response = await httpClient.GetStringAsync("odata/Companies");

        var companiesJson = JsonDocument.Parse(response);
        var companies = companiesJson.RootElement.GetProperty("value").EnumerateArray()
            .Select(company => new CompanyVM
            {
                CompanyId = company.GetProperty("CompanyId").GetInt32(),
                CompanyName = company.GetProperty("CompanyName").GetString()
            }).ToList();

        Companies = companies.Select(c => new SelectListItem
        {
            Value = c.CompanyId.ToString(),
            Text = c.CompanyName
        }).ToList();
    }
}