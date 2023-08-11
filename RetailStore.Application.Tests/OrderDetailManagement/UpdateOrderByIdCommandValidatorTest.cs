using FluentValidation.TestHelper;
using MockQueryable.Moq;
using Moq;
using RetailStore.Contracts;
using RetailStore.Dtos;
using RetailStore.Model;
using RetailStore.Requests.CustomerManagement;
using RetailStore.Requests.OrderManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailStore.Application.Tests.OrderDetailManagement;

public class UpdateOrderByIdCommandValidatorTest
{
    private readonly Mock<IRetailStoreDbContext> _dbContextMock;
    private readonly UpdateOrderByIdCommandValidator _validator;

    public UpdateOrderByIdCommandValidatorTest()
    {
        _dbContextMock = new Mock<IRetailStoreDbContext>();
        _validator = new UpdateOrderByIdCommandValidator(_dbContextMock.Object);
        MockOrderData();
    }

    [Fact]
    public void PhoneNumberNull_ShouldHaveValidationError()
    {
        var updateCommand = new UpdateOrderByIdCommand
        {
            Id = 1,
            OrderRequest = new OrderRequestDto
            {
                CustomerId = default(int),
                Details = new List<OrderDetailRequestDto>
                {
                    new OrderDetailRequestDto {ProductId = 1, Quantity = 100}
                }
            }
        };

        var result = _validator.TestValidate(updateCommand);

        result.ShouldHaveValidationErrorFor(c =>c.OrderRequest.CustomerId);
    }

    private void MockOrderData()
    {
        _dbContextMock.Setup(x => x.Products).Returns(
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

        _dbContextMock.Setup(x => x.Orders).Returns(
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


        _dbContextMock.Setup(x => x.OrderDetails).Returns(
        new List<OrderDetail>
        {

        new OrderDetail { ProductId = 1,OrderId = 1,Product = new Product { Id = 1, Name = "Product A",CreatedOn = DateTime.UtcNow }, Quantity = 2 }

        }.AsQueryable().BuildMockDbSet().Object);
    }
}
