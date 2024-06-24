using BadmintonReservationData.Base;
using JewelryAuctionData.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionData.Repository
{
    public class CompanyRepository : GenericRepository<AuctionResult>
    {
        public CompanyRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
