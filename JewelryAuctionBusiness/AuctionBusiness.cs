using JewelryAuctionBusiness.Dto;
using JewelryAuctionData;
using JewelryAuctionData.Entity;
using System.Threading.Tasks;
using JewelryAuctionData.Enum;

namespace JewelryAuctionBusiness;

public class AuctionBusiness
{
    private readonly UnitOfWork _unitOfWork;

    public AuctionBusiness(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Thêm phiên đấu giá mới
    public async Task<IBusinessResult> CreateAuctionSection(AuctionSectionDto auctionSectionDto)
    {
        var auctionSection = new AuctionSection
        {
            JewelryId = auctionSectionDto.JewelryID,
            StartTime = auctionSectionDto.StartTime,
            EndTime = auctionSectionDto.EndTime,
            Status = auctionSectionDto.Status,
            Discription = auctionSectionDto.Discription,
            InitialPrice = auctionSectionDto.InitialPrice
        };

        _unitOfWork.AuctionSectionRepository.Create(auctionSection);
        await _unitOfWork.CommitTransactionAsync();

        return new BusinessResult(200, "Auction section created successfully.", auctionSection);
    }

    // Cập nhật thông tin phiên đấu giá
    public async Task<IBusinessResult> UpdateAuctionSection(int auctionId, AuctionSectionDto auctionSectionDto)
    {
        var auctionSection = await _unitOfWork.AuctionSectionRepository.GetByIdAsync(auctionId);
        if (auctionSection == null) {
            return new BusinessResult(404, "Auction section not found.");
        }

        // Update auction details
        auctionSection.JewelryId = auctionSectionDto.JewelryID;
        auctionSection.StartTime = auctionSectionDto.StartTime;
        auctionSection.EndTime = auctionSectionDto.EndTime;
        auctionSection.Status = auctionSectionDto.Status;
        auctionSection.Discription = auctionSectionDto.Discription;
        auctionSection.InitialPrice = auctionSectionDto.InitialPrice;

        _unitOfWork.AuctionSectionRepository.Update(auctionSection);
        await _unitOfWork.CommitTransactionAsync();

        return new BusinessResult(200, "Auction section updated successfully.");
    }

    // Lấy danh sách phiên đấu giá đã được lên lịch
    public async Task<IBusinessResult> GetScheduledAuctions()
    {
        var auctions = await _unitOfWork.AuctionSectionRepository.GetAllAsync();
        if (auctions == null || auctions.Count == 0)
        {
            return new BusinessResult(404, "No auctions found.");
        }

        return new BusinessResult(200, "Successfully retrieved all Auction Sections.", auctions);
    }

    public async Task<IBusinessResult> GetAuctionSectionById(int key)
    {
        try
        {
            var auctionSection = await _unitOfWork.AuctionSectionRepository.GetByIdAsync(key);
            if (auctionSection == null)
            {
                return new BusinessResult(404, "Auction section not found.");
            }
            return new BusinessResult(200, "Auction section retrieved successfully.", auctionSection);
        }
        catch (Exception ex)
        {
            return new BusinessResult(500, $"Failed to retrieve auction section: {ex.Message}");
        }
    }
    public async Task<IBusinessResult> DeleteAuctionSection(int key)
    {
        try
        {
            var auctionSection = await _unitOfWork.AuctionSectionRepository.GetByIdAsync(key);
            if (auctionSection == null)
            {
                return new BusinessResult(404, "Auction section not found.");
            }

            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.AuctionSectionRepository.Remove(auctionSection);
            await _unitOfWork.CommitTransactionAsync();
        
            return new BusinessResult(200, "Auction section deleted successfully.");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new BusinessResult(500, $"Failed to delete auction section: {ex.Message}");
        }
    }

    public async Task<IBusinessResult> UpdateAuctionStatus(int key)
    {
        try
        {
            var auctionSection = await _unitOfWork.AuctionSectionRepository.GetByIdAsync(key);
            if (auctionSection == null)
            {
                return new BusinessResult(404, "Auction section not found.");
            }

            if (auctionSection.EndTime >= DateTime.Now)
            {
                auctionSection.Status = AuctionSessionEnum.Close.ToString();
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.AuctionSectionRepository.Remove(auctionSection);
                await _unitOfWork.CommitTransactionAsync();
            }
            else
            {
                return new BusinessResult(400, "Auction section Closed.");
            }
        
            return new BusinessResult(200, "Auction section deleted successfully.");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new BusinessResult(500, $"Failed to delete auction section: {ex.Message}");
        }
    }
    public async Task<IBusinessResult> UpdateAuctionStatus()
    {
        try
        {
            var auctionSections = await _unitOfWork.AuctionSectionRepository.GetAllAsync();
            var expiredAuctionSections = auctionSections.Where(x => x.EndTime <= DateTime.Now).ToList();

            if (!expiredAuctionSections.Any())
            {
                return new BusinessResult(404, "No expired auction sections found.");
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                foreach (var auctionSection in expiredAuctionSections)
                {
                    auctionSection.Status = AuctionSessionEnum.Close.ToString();
                    _unitOfWork.AuctionSectionRepository.Update(auctionSection);
                }

                await _unitOfWork.CommitTransactionAsync();
                return new BusinessResult(200, "Expired auction sections updated successfully.");
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        catch (Exception ex)
        {
            return new BusinessResult(500, $"Failed to update auction sections: {ex.Message}");
        }
    }


}
