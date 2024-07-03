namespace WebApplication1.ViewModel;

public class Auction
{
    public int AuctionID { get; set; }
    public int JewelryID { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
    public decimal InitialPrice { get; set; }
    public int BidderID { get; set; }
    public int RequestDetailID { get; set; }
}
