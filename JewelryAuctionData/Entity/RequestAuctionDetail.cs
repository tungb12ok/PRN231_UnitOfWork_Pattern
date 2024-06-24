using BadmintonReservationData.Base;
using System;
using System.Collections.Generic;

namespace JewelryAuctionData.Entity
{
    public partial class RequestAuctionDetail : BaseEntity
    {
        public RequestAuctionDetail()
        {
            AuctionSections = new HashSet<AuctionSection>();
        }

        public int RequestDetailId { get; set; }
        public int? RequestId { get; set; }
        public int? CustomerId { get; set; }
        public int? JewelryId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public string? Status { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Jewelry? Jewelry { get; set; }
        public virtual RequestAuction? Request { get; set; }
        public virtual ICollection<AuctionSection> AuctionSections { get; set; }
    }
}
