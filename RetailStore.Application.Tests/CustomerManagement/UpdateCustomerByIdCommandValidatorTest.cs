using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.CustomerManagement;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RetailStore.Tests.Requests.CustomerManagement
{
    public class UpdateCustomerCommandValidatorTests
    {
        private readonly Mock<IRetailStoreDbContext> _dbContextMock;
        private readonly UpdateCustomerCommandValidator _validator;

        public UpdateCustomerCommandValidatorTests()
        {
            _dbContextMock = new Mock<IRetailStoreDbContext>();
            _validator = new UpdateCustomerCommandValidator(_dbContextMock.Object);
        }
        [Theory]
        [InlineData(1, "John Doe", 1234567890)]
        public void ValidCommand_ShouldNotHaveValidationError(int customerId, string customerName, int phoneNumber)
        {
            // Arrange
            var command = new UpdateCustomerCommand
            {
                CustomerId = customerId,
                CustomerName = customerName,
                PhoneNumber = phoneNumber
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(1, "John Doe", 12345)] 
        public void InvalidPhoneNumber_ShouldHaveValidationError(int customerId, string customerName, int phoneNumber)
        {
            // Arrange
            var command = new UpdateCustomerCommand
            {
                CustomerId = customerId,
                CustomerName = customerName,
                PhoneNumber = phoneNumber
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.PhoneNumber);
        }


        [Fact]
        public void DuplicatePhoneNumber_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateCustomerCommand
            {
                CustomerId = 1,
                CustomerName = "John Doe",
                PhoneNumber = 1234567890
            };

            var customers = new List<Customer>
            {
                new Customer { Id = 1, PhoneNumber = 1234567890 },
                new Customer { Id = 2, PhoneNumber = 9876543210 }
            }.AsQueryable();

            var customersDbSetMock = new Mock<DbSet<Customer>>();
            customersDbSetMock.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(customers.Provider);
            customersDbSetMock.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(customers.Expression);
            customersDbSetMock.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(customers.ElementType);
            customersDbSetMock.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(customers.GetEnumerator());

            _dbContextMock.Setup(db => db.Customers).Returns(customersDbSetMock.Object);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.PhoneNumber);
        }
    }
}
