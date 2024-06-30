using Microsoft.Build.Framework;
using WebApplication1.Validate;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModel;


public class CreateJewelryAndAuctionVM
{
    public int CustomerID { get; set; }
    public string? JewelryName { get; set; }
    public string? Discription { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [ValidStatus(ErrorMessage = "Invalid status")]
    public string? Status { get; set; }
}

public class RequestAuctionDetailVM
{
    public int RequestDetailID { get; set; }
    public int RequestID { get; set; }
    public int CustomerID { get; set; }
    public string CustomerName { get; set; }
    public int JewelryID { get; set; }
    public string JewelryName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; }
    
}


public partial class JewelryVM
{
    public int JewelryId { get; set; }
    public string? JewelryName { get; set; }
    public string? Discription { get; set; }
}