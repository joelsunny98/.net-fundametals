
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RetailStore.Constants;
using RetailStore.Contracts;
using RetailStore.Dtos;
using RetailStore.Model;
using RetailStore.Requests.CustomerManagement;
using Xunit;

namespace RetailStore.Tests.Requests.CustomerManagement;

public class GetCustomerQueryHandlerTests
{
private readonly GetCustomerQueryHandler _handler;
private readonly Mock<IRetailStoreDbContext> _dbContextMock;
private readonly Mock<ILogger<GetCustomerQueryHandler>> _loggerMock;

public GetCustomerQueryHandlerTests()
{
    _dbContextMock = new Mock<IRetailStoreDbContext>();
    _loggerMock = new Mock<ILogger<GetCustomerQueryHandler>>();
    _handler = new GetCustomerQueryHandler(_dbContextMock.Object, _loggerMock.Object);
}

[Fact]
public async Task Handle_Should_Return_All_Customers()
{
    // Arrange
    var customers = new List<Customer>
    {
        new Customer { Id = 1, Name = "John Doe", PhoneNumber = 1234567890 },
        new Customer { Id = 2, Name = "Jane Smith", PhoneNumber = 9876543210 }
    };
    _dbContextMock.Setup(x => x.Customers).Returns(MockDbSet(customers));

    // Act
    var result = await _handler.Handle(new GetCustomersQuery(), CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(customers.Count, result.Count);
    Assert.Equal(customers[0].Name, result[0].CustomerName);
    Assert.Equal(customers[0].PhoneNumber, result[0].PhoneNumber);
    Assert.Equal(customers[1].Name, result[1].CustomerName);
    Assert.Equal(customers[1].PhoneNumber, result[1].PhoneNumber);
    _loggerMock.Verify(x => x.LogInformation(It.IsAny<string>(), customers.Count), Times.Once);
}

[Fact]
public async Task Handle_Should_Throw_Exception_When_Failed_To_Get_Customers()
{
    // Arrange
    _dbContextMock.Setup(x => x.Customers).Throws(new Exception("Failed to get customers"));

    // Act & Assert
    await Assert.ThrowsAsync<Exception>(() => _handler.Handle(new GetCustomersQuery(), CancellationToken.None));
    _loggerMock.Verify(x => x.LogError(It.IsAny<Exception>(), LogMessage.FailedToGetAllItems), Times.Once);
}

private static DbSet<T> MockDbSet<T>(List<T> data) where T : class
{
    var queryableData = data.AsQueryable();
    var mockSet = new Mock<DbSet<T>>();
    mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
    mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
    mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
    mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryableData.GetEnumerator());
    return mockSet.Object;
}
}