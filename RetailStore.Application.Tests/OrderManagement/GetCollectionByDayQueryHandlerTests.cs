using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.OrderManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailStore.Application.Tests.OrderManagement;

public class GetCollectionByDayQueryHandlerTests
{
    private readonly Mock<IRetailStoreDbContext> _mockDbContext;
    private readonly Mock<ILogger<GetCollectionByDayQuery>> _logger;

    public GetCollectionByDayQueryHandlerTests()
    {
        _mockDbContext = new Mock<IRetailStoreDbContext>();
        _logger = new Mock<ILogger<GetCollectionByDayQuery>>();
        MockOrderData();
    }

    [Fact]
    public async Task Handle_ReturnsCollectionForTheDay()
    {
        var handler = new GetCollectionByDayQueryHandler(_mockDbContext.Object, _logger.Object);
        // Act
        var result = await handler.Handle(new GetCollectionByDayQuery(), CancellationToken.None);
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
