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

    public async Task<IBusinessResult> PlaceBid(BidderDto bidderDto)
    {
        int auctionId = bidderDto.AuctionId;
        decimal newBidAmount = bidderDto.Amount;
        
            
        var auctionSection = await _unitOfWork.AuctionSectionRepository.GetByIdAsync(auctionId);
        
        var bidderCurent = auctionSection.Bidder;
        
        if (auctionSection == null || auctionSection.EndTime < DateTime.Now || auctionSection.Status != "Active")
        {
            return new BusinessResult(400, "Auction is not active or does not exist.");
        }

        // Assuming InitialPrice is used to track the highest bid
        if (newBidAmount <= auctionSection.InitialPrice)
        {
            return new BusinessResult(400, "New bid must be higher than the current highest bid.");
        }

        _unitOfWork.BeginTransactionAsync();
        // Update the highest bid in the AuctionSection
        auctionSection.InitialPrice = newBidAmount;
        _unitOfWork.AuctionSectionRepository.Update(auctionSection);

        // Assuming we track bids in some way in the Bidder table
        
        if (bidderCurent == null)
        {
            // If no existing bid, create new
            bidderCurent = new Bidder
            {
                CustomerId = bidderDto.CustomerId,
                CurrentBidPrice = newBidAmount
            };
            _unitOfWork.BidderRepository.Create(bidderCurent);
        }
        else
        {
            bidderCurent.CurrentBidPrice = newBidAmount;
            _unitOfWork.BidderRepository.Update(bidderCurent);
        }

        await _unitOfWork.CommitTransactionAsync();
        return new BusinessResult(200, "Bid placed successfully.");
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