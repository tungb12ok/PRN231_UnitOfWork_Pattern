using System.ComponentModel.DataAnnotations;

namespace JewelryAuctionBusiness.Dto;

public class RequestAuctionDetailsDto
{
    public int RequestDetailID { get; set; }
    public int RequestID { get; set; }
    public int CustomerID { get; set; }
    public int JewelryID { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; }  // Example values might include 'Pending', 'Approved', 'Rejected'
}
public class CreateJewelryAndAuctionDto
{
    // Jewelry properties
    [Required]
    [StringLength(255)]
    public string JewelryName { get; set; }

    [StringLength(500)]
    public string JewelryDescription { get; set; }

    // Auction properties
    [Required]
    public int CustomerId { get; set; }

    public DateTime? AuctionStartTime { get; set; }
    public DateTime? AuctionEndTime { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Initial price must be non-negative.")]
    public decimal? InitialPrice { get; set; }

    public JewelryDTO Jewelry { get; set; }
}