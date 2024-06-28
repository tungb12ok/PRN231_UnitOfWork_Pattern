using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModel;

public class CustomerVM
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
    [Required, MaxLength(100)]
    public string CustomerName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Phone { get; set; }

    public int CompanyId { get; set; }
}

public class CompanyVM
{
    public int CompanyId { get; set; }
    public string CompanyName { get; set; }
}