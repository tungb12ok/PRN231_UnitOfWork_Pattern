using JewelryAuctionData;
using JewelryAuctionData.Entity;
using JewelryAuctionData.Repository;
using System;
using System.Threading.Tasks;

namespace JewelryAuctionBusiness
{
    public class CustomerBusiness
    {
        private readonly UnitOfWork _unitOfWork;

        public CustomerBusiness(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBusinessResult> GetAllCustomers()
        {
            try
            {
                var customers = await _unitOfWork.CustomerRepository.GetAllAsync();

                if (customers == null || customers.Count == 0)
                {
                    return new BusinessResult(404, "No customers found.");
                }

                return new BusinessResult(200, "Successfully retrieved customers.", customers);
            }
            catch (Exception ex)
            {
                return new BusinessResult(500, $"Failed to retrieve customers: {ex.Message}");
            }
        }

        public async Task<IBusinessResult> GetCustomerById(int customerId)
        {
            try
            {
                var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerId);

                if (customer == null)
                {
                    return new BusinessResult(404, $"Customer with ID {customerId} not found.");
                }

                return new BusinessResult(200, $"Successfully retrieved customer with ID {customerId}.", customer);
            }
            catch (Exception ex)
            {
                return new BusinessResult(500, $"Failed to retrieve customer with ID {customerId}: {ex.Message}");
            }
        }

        public async Task<IBusinessResult> CreateCustomer(Customer customer)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.CustomerRepository.Create(customer);
                await _unitOfWork.CommitTransactionAsync();

                return new BusinessResult(200, "Customer created successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new BusinessResult(500, $"Failed to create customer: {ex.Message}");
            }
        }

        public async Task<IBusinessResult> UpdateCustomer(Customer customer)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existingCustomer = await _unitOfWork.CustomerRepository.GetByIdAsync(customer.CustomerId);

                if (existingCustomer == null)
                {
                    return new BusinessResult(404, $"Customer with ID {customer.CustomerId} not found.");
                }

                existingCustomer.CustomerName = customer.CustomerName; // Update other properties as needed
                _unitOfWork.CustomerRepository.Update(existingCustomer);
                await _unitOfWork.CommitTransactionAsync();

                return new BusinessResult(200, $"Customer with ID {customer.CustomerId} updated successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new BusinessResult(500, $"Failed to update customer with ID {customer.CustomerId}: {ex.Message}");
            }
        }

        public async Task<IBusinessResult> DeleteCustomer(int customerId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existingCustomer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerId);

                if (existingCustomer == null)
                {
                    return new BusinessResult(404, $"Customer with ID {customerId} not found.");
                }

                _unitOfWork.CustomerRepository.Remove(existingCustomer);
                await _unitOfWork.CommitTransactionAsync();

                return new BusinessResult(200, $"Customer with ID {customerId} deleted successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new BusinessResult(500, $"Failed to delete customer with ID {customerId}: {ex.Message}");
            }
        }
    }
}
