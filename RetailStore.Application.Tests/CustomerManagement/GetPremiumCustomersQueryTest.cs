using Microsoft.Extensions.Logging;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.CustomerManagement;

namespace RetailStore.Tests.Requests.CustomerManagement;

public class GetPremiumCustomersQueryHandlerTests
{
    private readonly GetPremiumCustomersQueryHandler _handler;
    private readonly Mock<IRetailStoreDbContext> _dbContextMock;
    private readonly Mock<ILogger<GetPremiumCustomersQuery>> _loggerMock;
    private readonly Mock<IPremiumCodeService> _premiumCodeServiceMock;

    public GetPremiumCustomersQueryHandlerTests()
    {
        _dbContextMock = new Mock<IRetailStoreDbContext>();
        _loggerMock = new Mock<ILogger<GetPremiumCustomersQuery>>();
        _premiumCodeServiceMock = new Mock<IPremiumCodeService>();

        _handler = new GetPremiumCustomersQueryHandler(
            _dbContextMock.Object,
            _loggerMock.Object,
            _premiumCodeServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Return_Premium_Customers_With_Premium_Codes()
    {
        // Arrange
        var orders = new List<Order>
        {
            new Order { CustomerId = 1, TotalAmount = 100 },
            new Order { CustomerId = 2, TotalAmount = 200 },
            new Order { CustomerId = 1, TotalAmount = 300 },
            new Order { CustomerId = 3, TotalAmount = 400 }
        };
        var customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "John Doe", PhoneNumber = 1234567890 },
            new Customer { Id = 2, Name = "Jane Smith", PhoneNumber = 9876543210 },
            new Customer { Id = 3, Name = "Alice Johnson", PhoneNumber = 5555555555 }
        };
        _dbContextMock.Setup(x => x.Orders).Returns((Delegate)orders.AsQueryable());
        _dbContextMock.Setup(x => x.Customers).Returns((Delegate)customers.AsQueryable());

        _premiumCodeServiceMock.Setup(x => x.GeneratePremiumCode()).Returns("PREM123");

        // Act
        var result = await _handler.Handle(new GetPremiumCustomersQuery(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(1, result[0].CustomerId);
        Assert.Equal("John Doe", result[0].CustomerName);
        Assert.Equal(1234567890, result[0].PhoneNumber);
        Assert.Equal(700, result[0].TotalPurchaseAmount);
        Assert.Equal("PREM123", result[0].PremiumCode);
        Assert.Equal(2, result[1].CustomerId);
        Assert.Equal("Jane Smith", result[1].CustomerName);
        Assert.Equal(9876543210, result[1].PhoneNumber);
        Assert.Equal(200, result[1].TotalPurchaseAmount);
        Assert.Equal("PREM123", result[1].PremiumCode);
        _loggerMock.Verify(x => x.LogInformation(It.IsAny<string>()), Times.Once);
        _premiumCodeServiceMock.Verify(x => x.GeneratePremiumCode(), Times.Exactly(2));
    }

    [Fact]
    public async Task Handle_MultiplePremiumCodeGenerations_Should_AssignDifferentCodes()
    {
        // Arrange
        var orders = new List<Order>
    {
        new Order { CustomerId = 1, TotalAmount = 100 },
        new Order { CustomerId = 2, TotalAmount = 200 }
    };
        var customers = new List<Customer>
    {
        new Customer { Id = 1, Name = "John Doe", PhoneNumber = 1234567890 },
        new Customer { Id = 2, Name = "Jane Smith", PhoneNumber = 9876543210 }
    };
        _dbContextMock.Setup(x => x.Orders).Returns((Delegate)orders.AsQueryable());
        _dbContextMock.Setup(x => x.Customers).Returns((Delegate)customers.AsQueryable());

        _premiumCodeServiceMock.SetupSequence(x => x.GeneratePremiumCode())
            .Returns("PREM123")
            .Returns("PREM456");

        // Act
        var result = await _handler.Handle(new GetPremiumCustomersQuery(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("PREM123", result[0].PremiumCode);
        Assert.Equal("PREM456", result[1].PremiumCode);
        _premiumCodeServiceMock.Verify(x => x.GeneratePremiumCode(), Times.Exactly(2));
    }

    [Fact]
    public async Task Handle_EmptyOrdersAndCustomers_Should_ReturnEmptyResult()
    {
        // Arrange
        _dbContextMock.Setup(x => x.Orders).Returns((Delegate)Enumerable.Empty<Order>().AsQueryable());
        _dbContextMock.Setup(x => x.Customers).Returns((Delegate)Enumerable.Empty<Customer>().AsQueryable());

        // Act
        var result = await _handler.Handle(new GetPremiumCustomersQuery(), CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_PremiumCodeGenerationFails_Should_NotGeneratePremiumCodes()
    {
        // Arrange
        var orders = new List<Order>
    {
        new Order { CustomerId = 1, TotalAmount = 100 },
        new Order { CustomerId = 2, TotalAmount = 200 }
    };
        var customers = new List<Customer>
    {
        new Customer { Id = 1, Name = "John Doe", PhoneNumber = 1234567890 },
        new Customer { Id = 2, Name = "Jane Smith", PhoneNumber = 9876543210 }
    };
        _dbContextMock.Setup(x => x.Orders).Returns((Delegate)orders.AsQueryable());
        _dbContextMock.Setup(x => x.Customers).Returns((Delegate)customers.AsQueryable());

        _premiumCodeServiceMock.Setup(x => x.GeneratePremiumCode()).Returns((string)null);

        // Act
        var result = await _handler.Handle(new GetPremiumCustomersQuery(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _premiumCodeServiceMock.Verify(x => x.GeneratePremiumCode(), Times.Exactly(1));
    }

}
