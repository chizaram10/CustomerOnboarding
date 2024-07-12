using Common.Core;
using Common.Core.Entities;
using Common.Core.Interfaces.Services;
using Common.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Common.Infrastructure.Implementations.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<IEnumerable<CustomerDTO>>> GetAllCustomersAsync()
        {
            try
            {
                var customers = await _context.Customers
                .Include(c => c.LGA)
                .ThenInclude(l => l!.State)
                .Select(x => new CustomerDTO
                {
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    State = x.LGA != null ? x.LGA.State!.Name ?? string.Empty : string.Empty,
                    LGA = x.LGA!.Name ?? string.Empty,
                    IsVerified = x.IsVerified
                })
                .ToListAsync();

                return new ServiceResponse<IEnumerable<CustomerDTO>>(true, "All customers retrieved successfully.", customers);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ServiceResponse<IEnumerable<CustomerDTO>>(false, "An error occured while retrieving customers.", null!);
            }
            

        }

        public async Task<ServiceResponse> OnboardCustomerAsync(OnboardCustomerRequestDTO request)
        {
            try
            {
                var lga = await _context.LGAs.FirstOrDefaultAsync(x => request.LGAId == x.Id);
                if (lga == null)
                {
                    return new ServiceResponse(false, "LGA not found.");
                }

                if (lga.StateId != request.StateId)
                {
                    return new ServiceResponse(false, "LGA not mapped to State.");
                }

                var existingCustomer = await _context.Customers.FirstOrDefaultAsync(x => request.Email == x.Email);
                if (existingCustomer != null)
                {
                    return new ServiceResponse(false, $"Account with email {request.Email} already exists.");
                }

                // Mock sending OTP (business rule iii)
                string otp = GenerateOTP();
                Console.WriteLine($"OTP sent to {request.PhoneNumber}: {otp}");

                // Save the customer without verification
                var customer = new Customer
                {
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    Password = EncryptionHelper.Encrypt(request.Password, request.Email),
                    LGAId = request.LGAId,
                    IsVerified = false
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                return new ServiceResponse(true, "Customer created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ServiceResponse(false, "An error occured while creating customer.");
            }
        }

        public async Task<ServiceResponse> VerifyOtpAsync(OTPVerificationRequestDTO request)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(x => request.Email == x.Email);
                if (customer == null)
                {
                    return new ServiceResponse(false, $"Account with email {request.Email} does not exists.");
                }

                if (request.Otp == "" || request.Otp != "123456")
                {
                    return new ServiceResponse(false, "Invalid OTP.");
                }

                customer.IsVerified = true;
                customer.DateUpdated = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return new ServiceResponse(true, "Account verified successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ServiceResponse(false, "An error occured while verifying account.");
            }
        }
        
        private static string GenerateOTP()
        {
            return "123456";
        }
    }
}
