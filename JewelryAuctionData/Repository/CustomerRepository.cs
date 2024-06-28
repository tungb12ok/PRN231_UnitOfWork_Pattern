using BadmintonReservationData.Base;
using JewelryAuctionData.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JewelryAuctionData.Repository
{
    public class CustomerRepository : GenericRepository<Customer>
    {
        public CustomerRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _dbSet
                .Include(c => c.Company) 
                .ToListAsync();
        }
        public async Task<Customer> GetByIdAsync(int customerId)
        {
            return await _dbSet
                .Include(c => c.Company) 
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }
    }
}