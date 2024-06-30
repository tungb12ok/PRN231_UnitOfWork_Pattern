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
    public class RequestAuctionDetailsRepository : GenericRepository<RequestAuctionDetail>
    {
        public RequestAuctionDetailsRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public async Task<List<RequestAuctionDetail>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.Jewelry)
                .Include(x => x.Customer)
                .ThenInclude(x => x.Company)
                .ToListAsync();
        }

        public async Task<RequestAuctionDetail?> GetByKeyAsync(int id)
        {
            return await _dbSet.Include(x => x.Jewelry)
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.RequestId == id);
        }
    }
}
