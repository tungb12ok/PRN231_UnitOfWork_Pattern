namespace JewelryAuctionBusiness.Dto;

public class RequestAuctionDto
{
    public int RequestAuctionId { get; set; }
    public int JewelryId { get; set; }
    public DateTime RequestDate { get; set; }
    public string Status { get; set; } // New, Pending, Approved, Rejected
}
