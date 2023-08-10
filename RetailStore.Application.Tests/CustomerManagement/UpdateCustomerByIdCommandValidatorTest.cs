using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using MockQueryable.Moq;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.CustomerManagement;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RetailStore.Tests.Requests.CustomerManagement;

public class UpdateCustomerCommandValidatorTests
{
    private readonly Mock<IRetailStoreDbContext> _dbContextMock;
    private readonly UpdateCustomerCommandValidator _validator;

    public UpdateCustomerCommandValidatorTests()
    {
        _dbContextMock = new Mock<IRetailStoreDbContext>();
        _validator = new UpdateCustomerCommandValidator(_dbContextMock.Object);
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

        _dbContextMock.Setup(db => db.Customers).Returns(customersDbSetMock.Object);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.PhoneNumber);
    }

    #region DatabaseInitialization
    /// <summary>
    /// Initializes Mock database with mocked object
    /// </summary>
    private void MockCustomerdata()
    {
        var customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "Austin", PhoneNumber = 9947003224 }
        }.AsQueryable().BuildMockDbSet();

        _dbContextMock.Setup(x => x.Customers).Returns(customers.Object);
    }
    #endregion
}
