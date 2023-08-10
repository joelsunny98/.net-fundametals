using Microsoft.Extensions.Logging;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.ProductManagement;

namespace RetailStore.Tests.ProductManagement
{
    public class UpdateProductCommandHandlerTest
    {
        private readonly Mock<IRetailStoreDbContext> _mockDbContext;
        private readonly Mock<ILogger<UpdateProductCommand>> _mockLogger;

        public UpdateProductCommandHandlerTest()
        {
            _mockDbContext = new Mock<IRetailStoreDbContext>();
            _mockLogger = new Mock<ILogger<UpdateProductCommand>>();
        }

        [Fact]
        public async Task Handle_ProductExists_ProductUpdated()
        {
            // Arrange
            var existingProduct = new Product { Id = 1, Name = "ExistingProduct", Price = 50 };
            _mockDbContext.Setup(x => x.Products.FindAsync(It.IsAny<int>())).ReturnsAsync(existingProduct);

            var updateCommand = new UpdateProductCommand
            {
                Id = 1,
                ProductName = "UpdatedProduct",
                ProductPrice = 75
            };

            var handler = new UpdateProductCommandHandler(_mockDbContext.Object, _mockLogger.Object);

            // Act
            var updatedProductId = await handler.Handle(updateCommand, CancellationToken.None);

            // Assert
            _mockDbContext.Verify(x => x.Products.FindAsync(updateCommand.Id), Times.Once);
            _mockDbContext.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);

            Assert.Equal(existingProduct.Id, updatedProductId);
            Assert.Equal(updateCommand.ProductName, existingProduct.Name);
            Assert.Equal(updateCommand.ProductPrice, existingProduct.Price);
            Assert.True(existingProduct.UpdatedOn > DateTime.UtcNow.AddMinutes(-1)); // Verify that UpdatedOn was updated
        }

        [Fact]
        public async Task Handle_ProductNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            _mockDbContext.Setup(x => x.Products.FindAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            var updateCommand = new UpdateProductCommand
            {
                Id = 1,
                ProductName = "UpdatedProduct",
                ProductPrice = 75
            };

            var handler = new UpdateProductCommandHandler(_mockDbContext.Object, _mockLogger.Object);

            // Act and Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await handler.Handle(updateCommand, CancellationToken.None);
            });

            // Assert
            _mockDbContext.Verify(x => x.Products.FindAsync(updateCommand.Id), Times.Once);

        }
    }
}