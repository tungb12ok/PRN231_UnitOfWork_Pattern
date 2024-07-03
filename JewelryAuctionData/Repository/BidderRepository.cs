using System.Xml.Schema;
using BadmintonReservationData.Base;
using JewelryAuctionData.Entity;
using Microsoft.EntityFrameworkCore;

namespace JewelryAuctionData.Repository
{
    public class BidderRepository : GenericRepository<Bidder>
    {
        public BidderRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<int> GetLastAsync()
        {
            return await _dbSet.OrderByDescending(x => x.BidderId).Select(x => x.BidderId).FirstOrDefaultAsync();
        }
    }

}
