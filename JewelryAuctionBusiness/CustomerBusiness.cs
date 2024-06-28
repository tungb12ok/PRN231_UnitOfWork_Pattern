using AutoMapper;
using JewelryAuctionData;
using JewelryAuctionData.Entity;
using JewelryAuctionData.Repository;
using System;
using System.Threading.Tasks;
using JewelryAuctionData.Dto;

namespace JewelryAuctionBusiness
{
    public class CustomerBusiness
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerBusiness(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IBusinessResult> GetAllCustomers()
        {
            try
            {
                var customers = await _unitOfWork
                    .CustomerRepository
                    .GetAllAsync();

                if (customers == null || customers.Count == 0)
                {
                    return new BusinessResult(404, "No customers found.");
                }

                var customerDTOs = _mapper.Map<List<CustomerDTO>>(customers);

                return new BusinessResult(200, "Successfully retrieved customers.", customerDTOs);
            }
            catch (Exception ex)
            {
                return new BusinessResult(500, $"Failed to retrieve customers: {ex.Message}");
            }
        }
        public async Task<IBusinessResult> GetAllCompany()
        {
            try
            {
                var customers = await _unitOfWork
                    .CompanyRepository
                    .GetAllAsync();

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

                var customerDTO = _mapper.Map<CustomerDTO>(customer);

                return new BusinessResult(200, $"Successfully retrieved customer with ID {customerId}.", customerDTO);
            }
            catch (Exception ex)
            {
                return new BusinessResult(500, $"Failed to retrieve customer with ID {customerId}: {ex.Message}");
            }
        }

        public async Task<IBusinessResult> CreateCustomer(CustomerDTO customerDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var customer = _mapper.Map<Customer>(customerDto);

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

        public async Task<IBusinessResult> UpdateCustomer(CustomerDTO customerDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existingCustomer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerDto.CustomerId);

                if (existingCustomer == null)
                {
                    return new BusinessResult(404, $"Customer with ID {customerDto.CustomerId} not found.");
                }

                _mapper.Map(customerDto, existingCustomer);

                _unitOfWork.CustomerRepository.Update(existingCustomer);
                await _unitOfWork.CommitTransactionAsync();

                return new BusinessResult(200, $"Customer with ID {customerDto.CustomerId} updated successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new BusinessResult(500, $"Failed to update customer with ID {customerDto.CustomerId}: {ex.Message}");
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
