using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.OrderManagement;

namespace RetailStore.Application.Tests.OrderManagement;

public class GetOrdersQueryTest
{
    private readonly Mock<IRetailStoreDbContext> _mockDbContext;
    private readonly Mock<ILogger<GetOrdersQuery>> _logger;

    public GetOrdersQueryTest()
    {
        _mockDbContext = new Mock<IRetailStoreDbContext>();
        _logger = new Mock<ILogger<GetOrdersQuery>>();
        MockOrderData();
    }

    [Fact]
    public async Task Handle_ReturnsListOfOrders()
    {
        var handler = new GetOrdersQueryHandler(_mockDbContext.Object, _logger.Object);
        var result = await handler.Handle(new GetOrdersQuery(), CancellationToken.None);

        Assert.NotEmpty(result);
    }

    private void MockOrderData()
    {
        _mockDbContext.Setup(x => x.Orders).Returns(new List<Order>
        {
            new Order
            {
                Id = 1,
                CustomerId = 1,
                TotalAmount = 100.0m,
                Discount = 10.0m,
                Customer = new Customer { Id = 1, Name = "Austin" },
                Details = new List<OrderDetail>
                {
                    new OrderDetail { Product = new Product { Id = 1, Name = "Product A" }, Quantity = 2 },
                    new OrderDetail { Product = new Product { Id = 2, Name = "Product B" }, Quantity = 3 }
                }
            },
        }.AsQueryable().BuildMockDbSet().Object);
    }

}
