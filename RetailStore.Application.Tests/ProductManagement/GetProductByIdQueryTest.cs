using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.ProductManagement;

namespace RetailStore.Tests.ProductManagement;

public class GetProductByIdQueryTest
{
    private readonly Mock<IRetailStoreDbContext> _mockDbContext;
    private readonly Mock<ILogger<GetProductByIdQuery>> _logger;
    public GetProductByIdQueryTest()
    {
        _mockDbContext = new Mock<IRetailStoreDbContext>();
        _logger = new Mock<ILogger<GetProductByIdQuery>>();
        MockProductData();
    }

    [Fact]
    public async Task GetProductIdQueryShouldReturnValidData()
    {
        var handler = new GetProductByIdQueryHandler(_mockDbContext.Object, _logger.Object);
        var result = await handler.Handle(new GetProductByIdQuery { Id = 1 }, CancellationToken.None);
        var product = result.First();
        Assert.NotNull(result);
        Assert.Equal("Product 1", product.ProductName);
    }

    [Fact]
    public async Task GetProductIdQuery_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var handler = new GetProductByIdQueryHandler(_mockDbContext.Object, _logger.Object);

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
        {
            await handler.Handle(new GetProductByIdQuery { Id = 999 }, CancellationToken.None);
        });
    }

    private void MockProductData()
    {
        _mockDbContext.Setup(x => x.Products).Returns(new List<Product>
            {
                new Product() {Id=1, Name = "Product 1", Price = 10 },
                new Product() {Id=2, Name = "Product 2", Price = 20 }
            }.AsQueryable().BuildMockDbSet().Object);
    }
}