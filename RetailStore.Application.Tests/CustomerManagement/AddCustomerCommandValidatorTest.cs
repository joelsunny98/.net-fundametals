using System;
using System.Linq;
using FluentValidation.TestHelper;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.CustomerManagement;
using Xunit;

namespace RetailStore.Tests
{
    public class AddCustomerCommandValidatorTests
    {
        [Fact]
        public void Should_Have_Error_When_CustomerName_Is_Null()
        {
            // Arrange
            var dbContextMock = new Mock<IRetailStoreDbContext>();
            var validator = new AddCustomerCommandValidator(dbContextMock.Object);
            var command = new AddCustomerCommand { CustomerName = null };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.CustomerName);
        }

        [Fact]
        public void Should_Have_Error_When_PhoneNumber_Is_Invalid()
        {
            // Arrange
            var dbContextMock = new Mock<IRetailStoreDbContext>();
            var validator = new AddCustomerCommandValidator(dbContextMock.Object);
            var command = new AddCustomerCommand { PhoneNumber = 12345 };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.PhoneNumber);
        }

        [Fact]
        public void Should_Have_Error_When_PhoneNumber_Is_Not_Unique()
        {
            // Arrange
            var dbContextMock = new Mock<IRetailStoreDbContext>();
            dbContextMock.Setup(db => db.Customers.Any(It.IsAny<Func<Customer, bool>>())).Returns(true);

            var validator = new AddCustomerCommandValidator(dbContextMock.Object);
            var command = new AddCustomerCommand { PhoneNumber = 1234567890 };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.PhoneNumber);
        }

        [Fact]
        public void Should_Not_Have_Errors_When_Command_Is_Valid()
        {
            // Arrange
            var dbContextMock = new Mock<IRetailStoreDbContext>();
            dbContextMock.Setup(db => db.Customers.Any(It.IsAny<Func<Customer, bool>>())).Returns(false);

            var validator = new AddCustomerCommandValidator(dbContextMock.Object);
            var command = new AddCustomerCommand
            {
                CustomerName = "John Doe",
                PhoneNumber = 1234567890
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
