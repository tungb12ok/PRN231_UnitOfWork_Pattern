using BadmintonReservationData.Base;
using JewelryAuctionData.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryAuctionData.Repository
{
    public class AuctionResultRepository : GenericRepository<AuctionResult>
    {
        public AuctionResultRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
