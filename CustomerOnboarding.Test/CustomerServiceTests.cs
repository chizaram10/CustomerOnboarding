using Common.Core;
using Common.Core.Entities;
using Common.Infrastructure.Data;
using Common.Infrastructure.Implementations.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CustomerOnboarding.Test
{
    public class CustomerServiceTests
    {
        private readonly CustomerService _customerService;
        private readonly AppDbContext _context;

        public CustomerServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new AppDbContext(options);

            _customerService = new CustomerService(_context);
        }

        [Fact]
        public async Task GetAllCustomersAsync_ShouldReturnAllCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Email = "test1@example.com", PhoneNumber = "1234567890", LGA = new LGA { Name = "LGA1", State = new State { Name = "State1" } }, IsVerified = true },
                new Customer { Email = "test2@example.com", PhoneNumber = "0987654321", LGA = new LGA { Name = "LGA2", State = new State { Name = "State2" } }, IsVerified = false }
            };

            _context.Customers.AddRange(customers);
            await _context.SaveChangesAsync();

            // Act
            var result = await _customerService.GetAllCustomersAsync();

            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task OnboardCustomerAsync_ShouldCreateCustomer()
        {
            // Arrange
            var lga = new LGA { Id = 1, Name = "LGA1", StateId = 1 };
            var state = new State { Id = 1, Name = "State1" };
            _context.LGAs.Add(lga);
            _context.States.Add(state);
            await _context.SaveChangesAsync();

            var request = new OnboardCustomerRequestDTO("1234567890", "test@example.com", "password", 1, 1);

            // Act
            var result = await _customerService.OnboardCustomerAsync(request);

            // Assert
            Assert.True(result.Success);
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == request.Email);
            Assert.NotNull(customer);
            Assert.Equal(request.Email, customer.Email);
        }

        [Fact]
        public async Task VerifyOtpAsync_ShouldVerifyCustomer()
        {
            // Arrange
            var customer = new Customer
            {
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Password = "encryptedpassword",
                LGAId = 1,
                IsVerified = false
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var request = new OTPVerificationRequestDTO("test@example.com",  "123456");

            // Act
            var result = await _customerService.VerifyOtpAsync(request);

            // Assert
            Assert.True(result.Success);
            var updatedCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == request.Email);
            Assert.True(updatedCustomer!.IsVerified);
        }
    }
}