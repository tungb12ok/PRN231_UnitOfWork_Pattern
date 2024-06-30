using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.ViewModel;

namespace WebApplication1.Pages.Payment
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public List<PaymentVM> Payments { get; set; } 

        public IndexModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            Payments = new List<PaymentVM>(); 
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadPaymentsAsync(); 
            return Page(); 
        }

        private async Task LoadPaymentsAsync()
        {
            var httpClient = _clientFactory.CreateClient("MyApi");

            try
            {
                var response = await httpClient.GetAsync("odata/Payments");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    var oDataResponse = JsonSerializer.Deserialize<ODataResponse<PaymentVM>>(jsonString, 
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (oDataResponse != null)
                    {
                        Payments = oDataResponse.Value; 
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to load payments: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading payments: {ex.Message}");
            }
        }
    }
}
