using System.ComponentModel.DataAnnotations;

namespace JewelryAuctionBusiness.Dto;

public class BidderDto
{
    [Key]
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
    public int AuctionId { get; set; }
}