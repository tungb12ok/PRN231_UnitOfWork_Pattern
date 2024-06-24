using BadmintonReservationData.Base;
using JewelryAuctionData.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionData.Repository
{
    public class AuctionSectionRepository : GenericRepository<AuctionResult>
    {
        public AuctionSectionRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
