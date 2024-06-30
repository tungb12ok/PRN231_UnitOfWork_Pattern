using AutoMapper;
using JewelryAuctionBusiness.Dto;
using JewelryAuctionData;
using JewelryAuctionData.Entity;

namespace JewelryAuctionBusiness;

public class PaymentBusiness
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PaymentBusiness(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // Method to record a new payment
    public async Task<IBusinessResult> RecordPayment(PaymentDto paymentDto)
    {
        try
        {
            var payment = new Payment
            {
                AuctionResultId = paymentDto.AuctionResultID,
                PaymentMethod = paymentDto.PaymentMethod,
                TotalPrice = paymentDto.TotalPrice,
                PaymentTime = DateTime.UtcNow,
                CustomerId = paymentDto.CustomerID,
                FinalPrice = paymentDto.FinalPrice,
                JewelryId = paymentDto.JewelryID,
                Fees = paymentDto.Fees,
                Percent = paymentDto.Percent,
                PaymentStatus = paymentDto.PaymentStatus
            };

            _unitOfWork.PaymentRepository.Create(payment);
            await _unitOfWork.CommitTransactionAsync();
            return new BusinessResult(200, "Payment recorded successfully.", payment);
        }
        catch (Exception ex)
        {
            return new BusinessResult(500, $"Error recording payment: {ex.Message}");
        }
    }

    // Method to update payment details
    public async Task<IBusinessResult> UpdatePayment(int paymentId, PaymentDto paymentDto)
    {
        var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(paymentId);
        if (payment == null)
        {
            return new BusinessResult(404, "Payment not found.");
        }

        payment.PaymentMethod = paymentDto.PaymentMethod;
        payment.TotalPrice = paymentDto.TotalPrice;
        payment.FinalPrice = paymentDto.FinalPrice;
        payment.Fees = paymentDto.Fees;
        payment.Percent = paymentDto.Percent;
        payment.PaymentStatus = paymentDto.PaymentStatus;

        _unitOfWork.PaymentRepository.Update(payment);
        await _unitOfWork.CommitTransactionAsync();
        return new BusinessResult(200, "Payment updated successfully.", payment);
    }
    // Method to get all payment
    public async Task<IBusinessResult> GetAllPayments()
    {
        try
        {
            var payments = await _unitOfWork.PaymentRepository.GetAllAsync();
            var a = _mapper.Map<List<PaymentDto>>(payments);

            if (payments == null || payments.Count == 0)
            {
                return new BusinessResult(404, "No payments found.");
            }

            return new BusinessResult(200, "Payments retrieved successfully.", a);
        }
        catch (Exception ex)
        {
            return new BusinessResult(500, $"Error retrieving payments: {ex.Message}");
        }
    }
    // Method to get all payment
    public async Task<IBusinessResult> GetPaymentById(int paymentId)
    {
        try
        {
            var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(paymentId);
            var a = _mapper.Map<PaymentDto>(payment);

            if (payment == null)
            {
                return new BusinessResult(404, "Payment not found.");
            }

            return new BusinessResult(200, "Payment retrieved successfully.", a);
        }
        catch (Exception ex)
        {
            return new BusinessResult(500, $"Error retrieving payment: {ex.Message}");
        }
    }

    // Additional methods can be defined here...
}
