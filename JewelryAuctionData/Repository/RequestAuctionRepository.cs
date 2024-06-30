using BadmintonReservationData.Base;
using JewelryAuctionData.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace JewelryAuctionData.Repository
{
    public class RequestAuctionRepository : GenericRepository<RequestAuction>
    {
        public RequestAuctionRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<RequestAuction>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.Jewelry)
                .Include(x => x.Customer)
                .ToListAsync();
        }
    }
}
