using System;
using Microsoft.Extensions.Logging;
using Moq;
using RetailStore.Contracts;
using Xunit;
using RetailStore.Requests.CustomerManagement;
using RetailStore.Model;

namespace RetailStore.Tests.Requests.CustomerManagement;

public class UpdateCustomerCommandHandlerTests
{
private readonly Mock<IRetailStoreDbContext> _dbContextMock;
private readonly Mock<ILogger<UpdateCustomerCommandHandler>> _loggerMock;
private readonly UpdateCustomerCommandHandler _handler;
private readonly Customer existingCustomer;

public UpdateCustomerCommandHandlerTests()
{
    _dbContextMock = new Mock<IRetailStoreDbContext>();
    _loggerMock = new Mock<ILogger<UpdateCustomerCommandHandler>>();
    _handler = new UpdateCustomerCommandHandler(_dbContextMock.Object, _loggerMock.Object);

    existingCustomer = new Customer
    {
        Id = 1,
        Name = "John",
        PhoneNumber = 1234567890,
        UpdatedOn = DateTime.UtcNow
    };

    _dbContextMock.Setup(x => x.Customers.FindAsync(It.IsAny<int>())).ReturnsAsync(existingCustomer);
}

[Fact]
public async Task Handle_ShouldUpdateCustomer()
{
    // Arrange
    var updateCustomerCommand = new UpdateCustomerCommand
    {
        CustomerId = 1,
        CustomerName = "Jane",
        PhoneNumber = 0987654321
    };

    // Act
    var result = await _handler.Handle(updateCustomerCommand, default);

    // Assert
    Assert.Equal(updateCustomerCommand.CustomerId, result);
    Assert.Equal(updateCustomerCommand.CustomerName, existingCustomer.Name);
    Assert.Equal(updateCustomerCommand.PhoneNumber, existingCustomer.PhoneNumber);
}

[Fact]
public async Task Handle_ShouldThrowKeyNotFoundException_WhenCustomerDoesNotExist()
{
    // Arrange
    _dbContextMock.Setup(x => x.Customers.FindAsync(It.IsAny<int>())).ReturnsAsync((Customer)null);
    var updateCustomerCommand = new UpdateCustomerCommand
    {
        CustomerId = 2,
        CustomerName = "Jane",
        PhoneNumber = 0987654321
    };

    // Act
    Exception ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(updateCustomerCommand, default));

    // Assert
    Assert.IsType<KeyNotFoundException>(ex);
}
}