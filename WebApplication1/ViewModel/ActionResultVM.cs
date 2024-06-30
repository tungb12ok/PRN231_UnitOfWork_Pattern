namespace WebApplication1.ViewModel;

public class AuctionResultVM
{
    public int AuctionResultId { get; set; }
    public int? AuctionId { get; set; }
    public int? BidderId { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? TransactionTime { get; set; }
    public decimal? FinalPrice { get; set; }

    public AuctionSectionVM Auction { get; set; }
    public BidderVM Bidder { get; set; }
}

public class AuctionSectionVM
{
    public int AuctionID { get; set; }
    public int JewelryID { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; }
    public string Discription { get; set; }
    public decimal InitialPrice { get; set; }
}

public class BidderVM
{
    public int CustomerId { get; set; }
    public decimal? CurrentBidPrice { get; set; }
    public int? AuctionId { get; set; }
    public CustomerVM CustomerDto { get; set; }
}


