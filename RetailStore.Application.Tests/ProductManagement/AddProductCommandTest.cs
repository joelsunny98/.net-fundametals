using Microsoft.Extensions.Logging;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.ProductManagement;
using Twilio.Http;

namespace RetailStore.Tests.ProductManagement;

public class AddProductCommandTest
{

    [Fact]
    public async Task Handle_ValidData_ProductAddedToDatabase()
    {
        var mockDbContext = new Mock<IRetailStoreDbContext>();
        var command = new AddProductCommand
        {
           ProductName ="Soap",
           ProductPrice= 50
        };

        mockDbContext.Setup(x => x.Products.Add(It.IsAny<Product>()));
        var handler = new AddProductCommandHandler(mockDbContext.Object);

        // Act
        var result = await handler.Handle(command,CancellationToken.None);

        // Assert
        mockDbContext.Verify(db => db.Products.Add(It.IsAny<Product>()), Times.Once);
        mockDbContext.Verify(db => db.SaveChangesAsync(CancellationToken.None), Times.Once);
        Assert.IsType<int>(result);
    }
}
//var product = new Product
//{
//    Name = request.ProductName,
//    Price = request.ProductPrice,
//    CreatedOn = DateTime.UtcNow,
//    UpdatedOn = DateTime.UtcNow
//};

//_dbContext.Products.Add(product);
//        await _dbContext.SaveChangesAsync(cancellationToken);

//        return product.Id;