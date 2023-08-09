using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.ProductManagement;

namespace RetailStore.Tests.ProductManagement;

public class GetAllProductQueryTest
{

    private readonly Mock<IRetailStoreDbContext> _mockDbContext;
    private readonly Mock<ILogger<GetAllProductsQuery>> _logger;
    public GetAllProductQueryTest()
    {
        _mockDbContext = new Mock<IRetailStoreDbContext>();
        _logger = new Mock<ILogger<GetAllProductsQuery>>();
        MockProductData();
    }
    [Fact]
    public async Task Handle_ReturnsListOfProducts()
    {
        // Arrange
        var handler = new GetAllProductsQueryHandler(_mockDbContext.Object, _logger.Object);

        var result = await handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal("Product 1", result[0].ProductName);
        Assert.Equal(10, result[0].ProductPrice);
    }

    private void MockProductData()
    {
        _mockDbContext.Setup(x => x.Products).Returns(new List<Product>
            {
                new Product() { Name = "Product 1", Price = 10 },
                new Product() { Name = "Product 2", Price = 20 }
            }.AsQueryable().BuildMockDbSet().Object);
    }
}