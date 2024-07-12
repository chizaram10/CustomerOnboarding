using Common.Core;
using Common.Core.Entities;
using Common.Core.Interfaces.Services;
using Common.Infrastructure.Data;
using Common.Infrastructure.Implementations.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CustomerOnboarding.Test
{
    public class CustomerServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        private readonly CustomerService _customerService;

        public CustomerServiceTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _customerService = new CustomerService(_fixture.Context);
        }

        [Fact]
        public async Task GetAllCustomersAsync_ShouldReturnAllCustomers()
        {
            // Arrange
            var expectedCount = 4;

            // Act
            var result = await _customerService.GetAllCustomersAsync();

            // Assert
            Assert.True(result.Success);
            Assert.Equal(expectedCount, result.Data.Count());
        }

        [Fact]
        public async Task OnboardCustomerAsync_ShouldCreateCustomer()
        {
            // Arrange
            var request = new OnboardCustomerRequestDTO("567-890-1234", "newcustomer@example.com", "newpassword", 1, 2);

            // Act
            var result = await _customerService.OnboardCustomerAsync(request);

            // Assert
            Assert.True(result.Success);
            var customer = await _fixture.Context.Customers.FirstOrDefaultAsync(c => c.Email == request.Email);
            Assert.NotNull(customer);
            Assert.Equal(request.Email, customer.Email);
        }

        [Fact]
        public async Task OnboardCustomerAsync_ShouldFailIfLGANotFound()
        {
            // Arrange
            var request = new OnboardCustomerRequestDTO("567-890-1234", "newcustomer@example.com", "newpassword", 99, 99);

            // Act
            var result = await _customerService.OnboardCustomerAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("LGA not found.", result.Message);
        }

        [Fact]
        public async Task OnboardCustomerAsync_ShouldFailIfLGAStateMismatch()
        {
            // Arrange
            var request = new OnboardCustomerRequestDTO("567-890-1234", "newcustomer@example.com", "newpassword", 2, 1);

            // Act
            var result = await _customerService.OnboardCustomerAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("LGA not mapped to State.", result.Message);
        }

        [Fact]
        public async Task OnboardCustomerAsync_ShouldFailIfCustomerAlreadyExists()
        {
            // Arrange
            var request = new OnboardCustomerRequestDTO("123-456-7890", "customer1@example.com", "password", 1, 1);

            // Act
            var result = await _customerService.OnboardCustomerAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal($"Account with email {request.Email} already exists.", result.Message);
        }

        [Fact]
        public async Task VerifyOtpAsync_ShouldVerifyCustomer()
        {
            // Arrange
            var request = new OTPVerificationRequestDTO("customer2@example.com", "123456");

            // Act
            var result = await _customerService.VerifyOtpAsync(request);

            // Assert
            Assert.True(result.Success);
            var customer = await _fixture.Context.Customers.FirstOrDefaultAsync(c => c.Email == request.Email);
            Assert.True(customer!.IsVerified);
        }

        [Fact]
        public async Task VerifyOtpAsync_ShouldFailIfCustomerDoesNotExist()
        {
            // Arrange
            var request = new OTPVerificationRequestDTO("nonexistent@example.com", "123456");

            // Act
            var result = await _customerService.VerifyOtpAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Account with email nonexistent@example.com does not exists.", result.Message);
        }

        [Fact]
        public async Task VerifyOtpAsync_ShouldFailIfOtpIsInvalid()
        {
            // Arrange
            var request = new OTPVerificationRequestDTO("customer2@example.com", "wrongotp");

            // Act
            var result = await _customerService.VerifyOtpAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid OTP.", result.Message);
        }
        [Fact]
        public async Task VerifyOtpAsync_ShouldPassIfOtpIsInvalid()
        {
            // Arrange
            var request = new OTPVerificationRequestDTO("customer2@example.com", "123456");

            // Act
            var result = await _customerService.VerifyOtpAsync(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Account verified successfully.", result.Message);
        }
    }
}
