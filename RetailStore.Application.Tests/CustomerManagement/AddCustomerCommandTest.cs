using System;
using System.Linq.Expressions;
using System.Threading;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RetailStore.Constants;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.CustomerManagement;
using Xunit;

namespace RetailStore.Tests.Requests.CustomerManagement;

public class AddCustomerCommandHandlerTests
{
    private readonly Mock<IRetailStoreDbContext> _dbContext;
    private readonly Mock<ILogger<AddCustomerCommandHandler>> _logger;
    private readonly AddCustomerCommandHandler _handler;

    public AddCustomerCommandHandlerTests()
    {
        _dbContext = new Mock<IRetailStoreDbContext>();
        _logger = new Mock<ILogger<AddCustomerCommandHandler>>();
        _handler = new AddCustomerCommandHandler(_dbContext.Object, _logger.Object);
    }

    [Theory]
    [InlineData("Test1", 1234567890)] // Add more test cases with different data values
    [InlineData("Test2", 9876543210)]
    public async Task Handle_ValidData_Success(string customerName, long phoneNumber)
    {
        // Arrange
        var command = new AddCustomerCommand
        {
            CustomerName = customerName,
            PhoneNumber = phoneNumber
        };

        // Mock dbContext so the SaveChangesAsync method returns 1
        _dbContext.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var customerId = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(1, customerId); // Modify the expected customer ID value

        _dbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        _logger.Verify(log => log.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => string.Equals(v.ToString(), string.Format(LogMessage.NewItem, customerId))),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_SaveChangesAsyncReturnsZero_Failure()
    {
        // Arrange
        var command = new AddCustomerCommand
        {
            CustomerName = "Test3",
            PhoneNumber = 5555555555
        };

        // Mock dbContext so the SaveChangesAsync method returns 0
        _dbContext.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

        // Act and Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_SaveChangesAsyncThrowsException_Failure()
    {
        // Arrange
        var command = new AddCustomerCommand
        {
            CustomerName = "Test4",
            PhoneNumber = 9999999999
        };

        // Mock dbContext so the SaveChangesAsync method throws an exception
        _dbContext.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Database error"));

        // Act and Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Theory]
    [InlineData("", 1234567890)]
    [InlineData("Test5", 0)]
    [InlineData("Test6", -9876543210)]
    public async Task Handle_InvalidData_ThrowsValidationException(string customerName, long phoneNumber)
    {
        // Arrange
        var command = new AddCustomerCommand
        {
            CustomerName = customerName,
            PhoneNumber = phoneNumber
        };

        // Act and Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_CustomerAlreadyExists_ThrowsDatabaseException()
    {
        // Arrange
        var command = new AddCustomerCommand
        {
            CustomerName = "Existing Customer",
            PhoneNumber = 5555555555
        };

        _dbContext.Setup(d => d.Customers.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act and Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ValidData_SaveChangesException()
    {
        // Arrange
        var command = new AddCustomerCommand
        {
            CustomerName = "Test7",
            PhoneNumber = 7777777777
        };

        _dbContext.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Save changes error"));

        // Act and Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
    }
}
