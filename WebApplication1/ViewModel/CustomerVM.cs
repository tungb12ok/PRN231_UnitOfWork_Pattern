using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModel;

public partial class CustomerVM
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; }

    public string Phone { get; set; }

    public int? CompanyId { get; set; }

    public string Email { get; set; }

    public CompanyVM Company { get; set; }
}

public class CustomerRegistrationViewModel
{
    [Required(ErrorMessage = "Customer name is required"), MaxLength(100)]
    public string CustomerName { get; set; }

    [Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Company selection is required")]
    public int CompanyId { get; set; }
}

public class CompanyVM
{
    public int CompanyId { get; set; }
    public string CompanyName { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    public List<CustomerVM>? Customers { get; set; }
}