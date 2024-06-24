using BadmintonReservationData.Base;
using JewelryAuctionData.Entity;

namespace JewelryAuctionData.Repository
{
    public class BidderRepository : GenericRepository<Bidder>
    {
        public BidderRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
