using BadmintonReservationData.Base;
using System;
using System.Collections.Generic;

namespace JewelryAuctionData.Entity
{
    public partial class AuctionResult : BaseEntity
    {
        public AuctionResult()
        {
            Payments = new HashSet<Payment>();
        }

        public int AuctionResultId { get; set; }
        public int? AuctionId { get; set; }
        public int? BidderId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? TransactionTime { get; set; }
        public decimal? FinalPrice { get; set; }

        public virtual AuctionSection? Auction { get; set; }
        public virtual Bidder? Bidder { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
