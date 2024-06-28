using System.Text.Json;
using JewelryAuctionData.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.ViewModel;

namespace WebApplication1.Pages.Customers
{
    public class CustomerListModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public CustomerListModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public List<CustomerVM> Customers { get; set; } = new List<CustomerVM>();

        [BindProperty]
        public CustomerVM CustomerVM { get; set; }

        [BindProperty]
        public CustomerVM NewCustomer { get; set; }

        public List<SelectListItem> Companies { get; set; } = new List<SelectListItem>();

        public async Task OnGetAsync()
        {
            await LoadCompaniesAsync();
            await LoadCustomersAsync();
        }

        private async Task LoadCustomersAsync()
        {
            var httpClient = _clientFactory.CreateClient("MyApi");
            var response = await httpClient.GetAsync("odata/Customers");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var oDataResponse = JsonSerializer.Deserialize<ODataResponse<CustomerVM>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (oDataResponse != null)
                {
                    Customers = oDataResponse.Value;
                }
            }
        }

        private async Task LoadCompaniesAsync()
        {
            var httpClient = _clientFactory.CreateClient("MyApi");
            var response = await httpClient.GetStringAsync("Companies");

            var companiesJson = JsonDocument.Parse(response);
            var companies = companiesJson.RootElement.GetProperty("$values").EnumerateArray()
                .Select(company => new CompanyVM
                {
                    CompanyId = company.GetProperty("companyId").GetInt32(),
                    CompanyName = company.GetProperty("companyName").GetString()
                }).ToList();

            Companies = companies.Select(c => new SelectListItem
            {
                Value = c.CompanyId.ToString(),
                Text = c.CompanyName
            }).ToList();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadCompaniesAsync();
                await LoadCustomersAsync();
                return Page();
            }

            var httpClient = _clientFactory.CreateClient("MyApi");
            var response = await httpClient.PostAsJsonAsync("odata/Customers", NewCustomer);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage();
            }

            ModelState.AddModelError(string.Empty, "Failed to create customer.");
            await LoadCompaniesAsync();
            await LoadCustomersAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadCompaniesAsync();
                await LoadCustomersAsync();
                return Page();
            }

            var httpClient = _clientFactory.CreateClient("MyApi");
            var response = await httpClient.PutAsJsonAsync($"odata/Customers({CustomerVM.CustomerId})", CustomerVM);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage();
            }

            ModelState.AddModelError(string.Empty, "Failed to update customer.");
            await LoadCompaniesAsync();
            await LoadCustomersAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var httpClient = _clientFactory.CreateClient("MyApi");
            var response = await httpClient.DeleteAsync($"odata/Customers({id})");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage();
            }

            ModelState.AddModelError(string.Empty, "Failed to delete customer.");
            await LoadCompaniesAsync();
            await LoadCustomersAsync();
            return Page();
        }
    }
}
