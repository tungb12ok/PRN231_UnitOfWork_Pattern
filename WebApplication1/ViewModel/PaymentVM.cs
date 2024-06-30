namespace WebApplication1.ViewModel;

public class AuctionResultDto
{
    public int AuctionResultId { get; set; }
    public int AuctionId { get; set; }
    public int BidderId { get; set; }
    public decimal FinalPrice { get; set; }
    public DateTime TransactionTime { get; set; }
    public decimal Amount { get; set; }
}

public class JewelryDto
{
    public int JewelryId { get; set; }
    public string JewelryName { get; set; }
    public string Description { get; set; }
}

public class PaymentVM
{
    public int PaymentId { get; set; }
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
    public AuctionResultDto AuctionResultDto { get; set; }
    public JewelryDto JewelryDto { get; set; }
}