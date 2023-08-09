using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RetailStore.Contracts;
using RetailStore.Dtos;
using RetailStore.Model;
using RetailStore.Requests.CustomerManagement;
using Xunit;

namespace RetailStore.Tests.Requests.CustomerManagement;

public class GetCustomerByIdQueryHandlerTests
{
private readonly GetCustomerByIdQueryHandler _handler;
private readonly Mock<IRetailStoreDbContext> _dbContextMock;
private readonly Mock<ILogger<GetCustomerByIdQueryHandler>> _loggerMock;

public GetCustomerByIdQueryHandlerTests()
{
    _dbContextMock = new Mock<IRetailStoreDbContext>();
    _loggerMock = new Mock<ILogger<GetCustomerByIdQueryHandler>>();
    _handler = new GetCustomerByIdQueryHandler(_dbContextMock.Object, _loggerMock.Object);
}

[Fact]
public async Task Handle_Should_Return_CustomerDto_When_Customer_Exists()
{
    // Arrange
    var customerId = 1;
    var query = new GetCustomerByIdQuery { CustomerId = customerId };
    var customer = new Customer { Id = customerId, Name = "John Doe", PhoneNumber = 1234567890 };
    _dbContextMock.Setup(x => x.Customers.FirstOrDefaultAsync(It.IsAny<Expression<Func<Customer, bool>>>(), default)).ReturnsAsync(customer);

    // Act
    var result = await _handler.Handle(query, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(customer.Name, result.CustomerName);
    Assert.Equal(customer.PhoneNumber, result.PhoneNumber);
    _loggerMock.Verify(x => x.LogInformation(It.IsAny<string>(), customerId), Times.Once);
}

[Fact]
public async Task Handle_Should_Throw_KeyNotFoundException_When_Customer_Does_Not_Exist()
{
    // Arrange
    var customerId = 1;
    var query = new GetCustomerByIdQuery { CustomerId = customerId };
    _dbContextMock.Setup(x => x.Customers.FirstOrDefaultAsync(It.IsAny<Expression<Func<Customer, bool>>>(), default)).ReturnsAsync((Customer)null);

    // Act & Assert
    await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    _loggerMock.Verify(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>(), customerId), Times.Once);
}
}