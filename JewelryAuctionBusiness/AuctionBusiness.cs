using AutoMapper;
using JewelryAuctionBusiness.Dto;
using JewelryAuctionData;
using JewelryAuctionData.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;
using JewelryAuctionData.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JewelryAuctionBusiness
{
    public class AuctionBusiness
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuctionBusiness(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Thêm phiên đấu giá mới
        public async Task<IBusinessResult> CreateAuctionSection(AuctionSectionDto auctionSectionDto)
        {
            var auctionSection = _mapper.Map<AuctionSection>(auctionSectionDto);

            _unitOfWork.AuctionSectionRepository.Create(auctionSection);
            await _unitOfWork.CommitTransactionAsync();

            var resultDto = _mapper.Map<AuctionSectionDto>(auctionSection);
            return new BusinessResult(200, "Auction section created successfully.", resultDto);
        }

        // Cập nhật thông tin phiên đấu giá
        public async Task<IBusinessResult> UpdateAuctionSection(int auctionId,
            AuctionSectionUpdateDto auctionSectionDto)
        {
            // Validate the input DTO
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(auctionSectionDto, null, null);
            if (!Validator.TryValidateObject(auctionSectionDto, validationContext, validationResults, true))
            {
                var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
                return new BusinessResult(400, "Validation failed: " + errors);
            }

            // Fetch the existing auction section from the repository
            var auctionSection = await _unitOfWork.AuctionSectionRepository.GetByIdAsync(auctionId);
            if (auctionSection == null)
            {
                return new BusinessResult(404, "Auction section not found.");
            }

            // Additional business logic validation for StartTime and EndTime
            if (auctionSectionDto.StartTime.HasValue && auctionSectionDto.EndTime.HasValue)
            {
                if (auctionSectionDto.EndTime <= auctionSectionDto.StartTime)
                {
                    return new BusinessResult(400, "End Time must be later than Start Time.");
                }
            }

            // Update only the necessary fields
            if (auctionSectionDto.StartTime.HasValue)
            {
                auctionSection.StartTime = auctionSectionDto.StartTime.Value;
            }

            if (auctionSectionDto.EndTime.HasValue)
            {
                auctionSection.EndTime = auctionSectionDto.EndTime.Value;
            }

            if (!string.IsNullOrEmpty(auctionSectionDto.Status))
            {
                auctionSection.Status = auctionSectionDto.Status;
            }

            if (!string.IsNullOrEmpty(auctionSectionDto.Discription))
            {
                auctionSection.Discription = auctionSectionDto.Discription;
            }

            if (auctionSectionDto.InitialPrice.HasValue)
            {
                auctionSection.InitialPrice = auctionSectionDto.InitialPrice.Value;
            }

            // Begin transaction and update the auction section
            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.AuctionSectionRepository.Update(auctionSection);
            await _unitOfWork.CommitTransactionAsync();

            // Map the updated entity to DTO and return the result
            var resultDto = _mapper.Map<AuctionSectionDto>(auctionSection);
            return new BusinessResult(200, "Auction section updated successfully.", resultDto);
        }


        // Lấy danh sách phiên đấu giá đã được lên lịch
        public async Task<IBusinessResult> GetScheduledAuctions()
        {
            var auctions = await _unitOfWork.AuctionSectionRepository.GetAllAsync();
            if (auctions == null || auctions.Count == 0)
            {
                return new BusinessResult(404, "No auctions found.");
            }

            var auctionDtos = _mapper.Map<List<AuctionSectionDto>>(auctions);
            return new BusinessResult(200, "Successfully retrieved all Auction Sections.", auctionDtos);
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

                var auctionSectionDto = _mapper.Map<AuctionSectionDto>(auctionSection);
                return new BusinessResult(200, "Auction section retrieved successfully.", auctionSectionDto);
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

                var auctionSectionDto = _mapper.Map<AuctionSectionDto>(auctionSection);
                return new BusinessResult(200, "Auction section deleted successfully.", auctionSectionDto);
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransactionAsync();
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

                if (auctionSection.EndTime <= DateTime.Now)
                {
                    auctionSection.Status = AuctionSessionEnum.Close.ToString();
                    await _unitOfWork.BeginTransactionAsync();
                    _unitOfWork.AuctionSectionRepository.Update(auctionSection);
                    await _unitOfWork.CommitTransactionAsync();

                    var auctionSectionDto = _mapper.Map<AuctionSectionDto>(auctionSection);
                    return new BusinessResult(200, "Auction section status updated to Closed.", auctionSectionDto);
                }
                else
                {
                    return new BusinessResult(400, "Auction section is not yet due to close.");
                }
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransactionAsync();
                return new BusinessResult(500, $"Failed to update auction section status: {ex.Message}");
            }
        }

        public async Task<IBusinessResult> UpdateAuctionStatuses()
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
                    var auctionSectionDtos = _mapper.Map<List<AuctionSectionDto>>(expiredAuctionSections);
                    return new BusinessResult(200, "Expired auction sections updated successfully.",
                        auctionSectionDtos);
                }
                catch
                {
                    _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(500, $"Failed to update auction sections: {ex.Message}");
            }
        }
    }
}