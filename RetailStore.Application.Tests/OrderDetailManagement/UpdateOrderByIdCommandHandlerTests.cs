using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using RetailStore.Contracts;
using RetailStore.Dtos;
using RetailStore.Helpers;
using RetailStore.Model;
using RetailStore.Requests.OrderManagement;
using RetailStore.Requests.ProductManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailStore.Application.Tests.OrderDetailManagement;
public class UpdateOrderByIdCommandHandlerTests
{
    private readonly Mock<IRetailStoreDbContext> _mockDbContext;

    public UpdateOrderByIdCommandHandlerTests()
    {
        _mockDbContext = new Mock<IRetailStoreDbContext>();
        MockOrderData();
    }

    [Fact]
    public async Task Handle_OrderExists_OrderUpdated()
    {
        // Arrange
        var updateCommand = new UpdateOrderByIdCommand
        {
            Id = 1,
            OrderRequest = new OrderRequestDto
            {
                CustomerId = 1,
                Details = new List<OrderDetailRequestDto>
                {
                    new OrderDetailRequestDto {ProductId = 1, Quantity = 100}
                }
            }
        };

        var handler = new UpdateOrderByIdCommandHandler(_mockDbContext.Object);

        // Act
        var result = await handler.Handle(updateCommand, CancellationToken.None);

        // Assert
        _mockDbContext.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        Assert.Equal(1, result);

        var updatedOrder = _mockDbContext.Object.Orders.FirstOrDefault(o => o.Id == updateCommand.Id);
        Assert.NotNull(updatedOrder);
        Assert.Equal(updateCommand.OrderRequest.CustomerId, updatedOrder.CustomerId);
    }



    private void MockOrderData()
    {
        _mockDbContext.Setup(x => x.Products).Returns(
            new List<Product>
            {
            new Product() {
                Id = 1,
                Name = "Test",
                Price= 100,
                CreatedOn= DateTime.Now,
                UpdatedOn = DateTime.Now
                }
            }.AsQueryable().BuildMockDbSet().Object);

        _mockDbContext.Setup(x => x.Orders).Returns(
        new List<Order>
        {
            new Order()
            {
            Id = 1,
            CustomerId = 1,
            TotalAmount = 100,
            Discount = 10,
            Customer = new Customer
            {
                Id = 1,
                Name = "Test",
                PhoneNumber = 786687687697,
                CreatedOn= DateTime.Now,
                UpdatedOn= DateTime.Now
            },
            Details = new List<OrderDetail>
                {
                    new OrderDetail { ProductId = 1,OrderId = 1,Product = new Product { Id = 1, Name = "Product A",CreatedOn = DateTime.UtcNow }, Quantity = 2 },
                }
            }
        }.AsQueryable().BuildMockDbSet().Object);


        _mockDbContext.Setup(x => x.OrderDetails).Returns(
        new List<OrderDetail>
        {

        new OrderDetail { ProductId = 1,OrderId = 1,Product = new Product { Id = 1, Name = "Product A",CreatedOn = DateTime.UtcNow }, Quantity = 2 }

        }.AsQueryable().BuildMockDbSet().Object);
    }
    }

