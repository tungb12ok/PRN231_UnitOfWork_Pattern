using JewelryAuctionBusiness.Dto;
using JewelryAuctionData;
using JewelryAuctionData.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JewelryAuctionData.Enum;
namespace JewelryAuctionBusiness
{
    public class RequestAuctionBusiness
    {
        private readonly UnitOfWork _unitOfWork;

        public RequestAuctionBusiness(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBusinessResult> GetAllRequestAuctions()
        {
            try
            {
                var auctions = await _unitOfWork.RequestAuctionRepository.GetAllAsync();

                if (auctions == null || auctions.Count == 0)
                {
                    return new BusinessResult(404, "No auction requests found.");
                }

                return new BusinessResult(200, "Successfully retrieved all auction requests.", auctions);
            }
            catch (Exception ex)
            {
                return new BusinessResult(500, $"Failed to retrieve auction requests: {ex.Message}");
            }
        }
        public async Task<IBusinessResult> CreateRequestAuction(RequestAuction requestAuction)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.RequestAuctionRepository.Create(requestAuction);
                await _unitOfWork.CommitTransactionAsync();

                return new BusinessResult(200, "Auction request created successfully.", requestAuction);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new BusinessResult(500, $"Failed to create auction request: {ex.Message}");
            }
        }
        public async Task<IBusinessResult> UpdateRequestAuction(RequestAuction requestAuction)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existingAuction = await _unitOfWork.RequestAuctionRepository.GetByIdAsync(requestAuction.RequestId);

                if (existingAuction == null)
                {
                    return new BusinessResult(404, $"Auction request with ID {requestAuction.RequestId} not found.");
                }

                // Update properties
                existingAuction.JewelryId = requestAuction.JewelryId;
                existingAuction.CustomerId = requestAuction.CustomerId;
                // Add more properties as needed

                _unitOfWork.RequestAuctionRepository.Update(existingAuction);
                await _unitOfWork.CommitTransactionAsync();

                return new BusinessResult(200, $"Auction request with ID {requestAuction.RequestId} updated successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new BusinessResult(500, $"Failed to update auction request with ID {requestAuction.RequestId}: {ex.Message}");
            }
        }
        public async Task<IBusinessResult> DeleteRequestAuction(int requestId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existingAuction = await _unitOfWork.RequestAuctionRepository.GetByIdAsync(requestId);

                if (existingAuction == null)
                {
                    return new BusinessResult(404, $"Auction request with ID {requestId} not found.");
                }

                _unitOfWork.RequestAuctionRepository.Remove(existingAuction);
                await _unitOfWork.CommitTransactionAsync();

                return new BusinessResult(200, $"Auction request with ID {requestId} deleted successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new BusinessResult(500, $"Failed to delete auction request with ID {requestId}: {ex.Message}");
            }
        }
        public async Task<IBusinessResult> ApproveRequestAuction(RequestAuctionDetailsDto detailsDto, bool approve)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var requestDetails = await _unitOfWork.RequestAuctionDetailsRepository.GetByIdAsync(detailsDto.RequestDetailID);
                if (requestDetails == null)
                {
                    return new BusinessResult
                    {
                        Status = 404,
                        Message = "Request details not found."
                    };
                }

                // Update the details based on the approve parameter
                switch (approve)
                {
                    case true:
                        requestDetails.Status = AuctionStatusEnum.Approved.ToString();
                        break;
                    case false:
                        requestDetails.Status = AuctionStatusEnum.Rejected.ToString();
                        break;
                }

                // Update other details as needed
                requestDetails.Quantity = detailsDto.Quantity;
                requestDetails.Price = detailsDto.Price;

                _unitOfWork.RequestAuctionDetailsRepository.Update(requestDetails);
                await _unitOfWork.CommitTransactionAsync();

                return new BusinessResult
                {
                    Status = 200,
                    Message = approve ? "Request auction approved successfully." : "Request auction rejected successfully.",
                    Data = requestDetails
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new BusinessResult
                {
                    Status = 500,
                    Message = $"Failed to process request auction: {ex.Message}"
                };
            }
        }
        public async Task<IBusinessResult> UpdateRequestAuctionDetails(RequestAuctionDetailsDto detailsDto)
        {
            // Example business logic for updating an auction request detail
            var details = await _unitOfWork.RequestAuctionDetailsRepository.GetByIdAsync(detailsDto.RequestDetailID);
            if (details == null)
            {
                return new BusinessResult { Status = 404, Message = "Request details not found." };
            }

            details.Quantity = detailsDto.Quantity;
            details.Price = detailsDto.Price;
            details.Status = detailsDto.Status;

            _unitOfWork.RequestAuctionDetailsRepository.Update(details);
            await _unitOfWork.CommitTransactionAsync();

            return new BusinessResult { Status = 200, Message = "Details updated successfully.", Data = details };
        }
        public async Task<IBusinessResult> CreateJewelryAndRequestAuction(JewelryDTO jewelryDto, int customerId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Create a new Jewelry item from the DTO
                var jewelry = new Jewelry
                {
                    JewelryName = jewelryDto.JewelryName,
                    Discription = jewelryDto.Description
                };

                _unitOfWork.JewelryRepository.Create(jewelry);

                // Create a new Request Auction linked to the Jewelry item
                var requestAuction = new RequestAuction
                {
                    JewelryId = jewelry.JewelryId,
                    CustomerId = customerId
                };

                _unitOfWork.RequestAuctionRepository.Create(requestAuction);
                await _unitOfWork.CommitTransactionAsync();

                return new BusinessResult
                {
                    Status = 200,
                    Message = "Successfully created jewelry and request auction.",
                    Data = new { JewelryId = jewelry.JewelryId, RequestAuctionId = requestAuction.RequestId }
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new BusinessResult
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
        public async Task<IBusinessResult> GetRequestAuctionById(int key)
        {
            try
            {
                var auctions = await _unitOfWork.RequestAuctionRepository.GetByIdAsync(key);

                if (auctions == null)
                {
                    return new BusinessResult(404, "No auction requests found.");
                }

                return new BusinessResult(200, "Successfully retrieved all auction requests.", auctions);
            }
            catch (Exception ex)
            {
                return new BusinessResult(500, $"Failed to retrieve auction requests: {ex.Message}");
            }
        }
        
    }
}