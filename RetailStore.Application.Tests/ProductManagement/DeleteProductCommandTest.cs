using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.ProductManagement;
using Xunit;

namespace RetailStore.Tests.ProductManagement;

public class DeleteProductCommandTest
{
    [Fact]
    public async Task Handle_ValidRequest_ReturnsTrue()
    {
        // Arrange
        var dbContextMock = new Mock<IRetailStoreDbContext>();
        var loggerMock = new Mock<ILogger<DeleteProductCommand>>();

        DeleteProductCommandHandler sut = new DeleteProductCommandHandler(dbContextMock.Object, loggerMock.Object);

        var request = new DeleteProductCommand { ProductId = 1 };

        dbContextMock.Setup(d => d.Products.FindAsync(request.ProductId))
            .ReturnsAsync(new Product());

        // Act
        var result = await sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Handle_ProductNotFound_ReturnsFalse()
    {
        // Arrange
        var dbContextMock = new Mock<IRetailStoreDbContext>();
        var loggerMock = new Mock<ILogger<DeleteProductCommand>>();

        DeleteProductCommandHandler sut = new DeleteProductCommandHandler(dbContextMock.Object, loggerMock.Object);

        var request = new DeleteProductCommand { ProductId = 1 };

        dbContextMock.Setup(d => d.Products.FindAsync(request.ProductId))
            .ReturnsAsync((Product)null); // Simulate product not found

        // Act
        var result = await sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
    }
}