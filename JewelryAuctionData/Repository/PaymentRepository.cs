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
    public class PaymentRepository : GenericRepository<Payment>
    {
        public PaymentRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<Payment>> GetAllAsync()
        {
            return await _dbSet
                .Include(x => x.Customer)
                .Include(x => x.Jewelry)
                .Include(x => x.AuctionResult)
                .ToListAsync();
        }

        public async Task<Payment> GetByIdAsync(int key)
        {
            return await _dbSet
                .Include(x => x.Customer)
                .Include(x => x.Jewelry)
                .Include(x => x.AuctionResult)
                .FirstOrDefaultAsync(x => x.PaymentId == key);
        }
    }
}