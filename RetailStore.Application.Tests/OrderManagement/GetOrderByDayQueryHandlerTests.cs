using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.OrderManagement;

namespace RetailStore.Application.Tests.OrderManagement;

public class GetOrderByDayQueryHandlerTests
{
    private readonly Mock<IRetailStoreDbContext> _mockDbContext;
    private readonly Mock<ILogger<GetOrderByDayQuery>> _logger;

    public GetOrderByDayQueryHandlerTests()
    {
        _mockDbContext = new Mock<IRetailStoreDbContext>();
        _logger = new Mock<ILogger<GetOrderByDayQuery>>();
        MockOrderData();
    }
    
    [Fact]
    public async Task Handle_ReturnsOrdersForTheDay()
    {
        var handler = new GetOrderByDayQueryHandler(_mockDbContext.Object, _logger.Object);
        // Act
        var result = await handler.Handle(new GetOrderByDayQuery(), CancellationToken.None);
        // Assert
        Assert.NotNull(result);
    }

    private void MockOrderData()
    {
        _mockDbContext.Setup(x => x.Orders).Returns(new List<Order>
        {
            new Order
            {
                Id = 1,
                CustomerId = 1,
                TotalAmount = 100,
                Discount = 10,
                Customer = new Customer { Id = 1, Name = "Austin" },
                Details = new List<OrderDetail>
                {
                    new OrderDetail { Product = new Product { Id = 1, Name = "Product A",CreatedOn = DateTime.UtcNow }, Quantity = 2 },
                    new OrderDetail { Product = new Product { Id = 2, Name = "Product B", CreatedOn = DateTime.UtcNow.AddDays(-1) },  }
                }
            },
        }.AsQueryable().BuildMockDbSet().Object);
    }

}
