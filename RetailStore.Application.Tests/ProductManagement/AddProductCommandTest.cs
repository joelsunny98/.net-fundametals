using Moq;
using RetailStore.Contracts;
using RetailStore.Requests.ProductManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailStore.Tests.ProductManagement;

public class AddProductCommandTest
{
    [Fact]
    public async Task Handle_ValidData_ProductAddedToDatabase()
    {
        var mockDbContext = new Mock<IRetailStoreDbContext>();
        var command = new AddProductCommand
        {
            ProductName = "NewProduct",
            ProductPrice = 100
        };

        var handler = new AddProductCommandHandler(mockDbContext.Object);

        var productId = await handler.Handle(command, CancellationToken.None);

        mockDbContext.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);

        Assert.NotEqual(0, productId); 
    }
}