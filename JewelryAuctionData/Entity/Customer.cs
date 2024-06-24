﻿using BadmintonReservationData.Base;
using System;
using System.Collections.Generic;

namespace JewelryAuctionData.Entity
{
    public partial class Customer : BaseEntity
    {
        public Customer()
        {
            Bidders = new HashSet<Bidder>();
            Payments = new HashSet<Payment>();
            RequestAuctionDetails = new HashSet<RequestAuctionDetail>();
            RequestAuctions = new HashSet<RequestAuction>();
        }

        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? Phone { get; set; }
        public int? CompanyId { get; set; }
        public string? Email { get; set; }

        public virtual Company? Company { get; set; }
        public virtual ICollection<Bidder> Bidders { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<RequestAuctionDetail> RequestAuctionDetails { get; set; }
        public virtual ICollection<RequestAuction> RequestAuctions { get; set; }
    }
}
