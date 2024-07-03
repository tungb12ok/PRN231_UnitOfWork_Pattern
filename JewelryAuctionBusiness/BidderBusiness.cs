using JewelryAuctionBusiness.Dto;
using JewelryAuctionData;
using JewelryAuctionData.Entity;
using System.Threading.Tasks;

namespace JewelryAuctionBusiness;

public class BidderBusiness
{
    private readonly UnitOfWork _unitOfWork;

    public BidderBusiness(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IBusinessResult> PlaceBid(BidderAuction bidderDto)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync().ConfigureAwait(false);

            int auctionId = bidderDto.AuctionId;
            decimal newBidAmount = bidderDto.CurrentBidPrice ?? 0;

            // Retrieve auction section
            var auctionSection = await _unitOfWork.AuctionSectionRepository.GetByIdAsync(auctionId).ConfigureAwait(false);

            if (auctionSection == null || auctionSection.EndTime < DateTime.Now || auctionSection.Status != "Active")
            {
                return new BusinessResult(400, "Auction is not active or does not exist.");
            }

            if (newBidAmount <= auctionSection.InitialPrice)
            {
                return new BusinessResult(400, "New bid must be higher than the Initial Price.");
            }

            if (auctionSection.Bidder != null && newBidAmount <= auctionSection.Bidder.CurrentBidPrice)
            {
                return new BusinessResult(400, "New bid must be higher than the current highest bid.");
            }


            // Create new bidder record
            var bidder = new Bidder
            {
                CurrentBidPrice = newBidAmount,
                CustomerId = bidderDto.CustomerId
            };

            await _unitOfWork.BidderRepository.CreateAsync(bidder);
             var bidderId = await _unitOfWork.BidderRepository.GetLastAsync();
            auctionSection.BidderId = bidderId;
            auctionSection.InitialPrice = newBidAmount;
            await _unitOfWork.AuctionSectionRepository.UpdateAsync(auctionSection).ConfigureAwait(false);

            await _unitOfWork.CommitTransactionAsync();
            return new BusinessResult(200, "Bid placed successfully.");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new BusinessResult(500, $"An error occurred while placing the bid: {ex.Message}");
        }
    }

    public async Task<IBusinessResult> GetBidderByCustomerId(int customerId)
    {
        try
        {
            var auctionSection = await _unitOfWork.BidderRepository.GetAllAsync();
            var bidderCustomer = auctionSection.Where(x => x.CustomerId == customerId).FirstOrDefault();
            if (auctionSection == null)
            {
                return new BusinessResult(404, "Auction section not found.");
            }

            return new BusinessResult(200, "Auction section retrieved successfully.", bidderCustomer);
        }
        catch (Exception ex)
        {
            return new BusinessResult(500, $"Failed to retrieve auction section: {ex.Message}");
        }
    }

    public async Task<IBusinessResult> RecordAuctionResult(int auctionId)
    {
        var auctionSection = await _unitOfWork.AuctionSectionRepository.GetByIdAsync(auctionId);
        if (auctionSection == null || auctionSection.EndTime > DateTime.Now || auctionSection.Status != "Closed")
        {
            return new BusinessResult(400, "Auction is not ended or does not exist.");
        }

        var winningBid = await _unitOfWork.BidderRepository.GetByIdAsync(auctionId);
        if (winningBid == null)
        {
            return new BusinessResult(404, "No bids found for this auction.");
        }

        var auctionResult = new AuctionResult
        {
            AuctionId = auctionId,
            BidderId = auctionSection.BidderId,
            Amount = auctionSection.Bidder.CurrentBidPrice,
            TransactionTime = DateTime.Now,
            FinalPrice = auctionSection.Bidder.CurrentBidPrice
        };

        _unitOfWork.AuctionResultRepository.Create(auctionResult);
        await _unitOfWork.CommitTransactionAsync();

        return new BusinessResult(200, "Auction result recorded successfully.");
    }
}