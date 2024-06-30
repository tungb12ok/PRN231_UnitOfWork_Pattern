namespace WebApplication1.ViewModel;

public class AuctionSessionVM
{
    
}
public class AuctionSectionUpdateVM
{
    public int AuctionID { get; set; } 
    public int? JewelryID { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string Status { get; set; }
    public string Discription { get; set; }
    public decimal? InitialPrice { get; set; }
    public int? BidderID { get; set; }
    public int? RequestDetailID { get; set; }
}