namespace JewelryAuctionBusiness.Dto;

public class PaymentDto
{
    public int AuctionResultID { get; set; }
    public string PaymentMethod { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime PaymentTime { get; set; }
    public int CustomerID { get; set; }
    public decimal FinalPrice { get; set; }
    public int JewelryID { get; set; }
    public decimal Fees { get; set; }
    public decimal Percent { get; set; }
    public string PaymentStatus { get; set; }
}
