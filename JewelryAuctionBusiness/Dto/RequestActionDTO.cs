using JewelryAuctionData.Dto;

namespace JewelryAuctionBusiness.Dto;

public class CreateRequestAuctionDTO
{
    public int CustomerId { get; set; }
    public int JewelryId { get; set; }
}

public class CreateRequestAuctionDetailDTO
{
    public int CustomerId { get; set; }
    public int JewelryId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
public class RequestAuctionDTO
{
    public int RequestID { get; set; }
    public int CustomerID { get; set; }
    public int JewelryID { get; set; }
    public JewelryDTO Jewelry { get; set; }
    public CustomerDTO Customer { get; set; }
}

public partial class JewelryDTO
{
}
