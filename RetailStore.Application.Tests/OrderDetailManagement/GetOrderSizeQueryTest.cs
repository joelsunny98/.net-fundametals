using MockQueryable.Moq;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.OrderDetailManagement;

namespace RetailStore.Tests.Requests.OrderDetailManagement;

public class GetOrderSizeQueryTest
{
    private readonly Mock<IRetailStoreDbContext> _dbContextMock;
    public GetOrderSizeQueryTest()
    {
        _dbContextMock = new Mock<IRetailStoreDbContext>();
        OrderDetailsMock();
    }

    [Fact]
    public async Task Handle_Should_Return_Correct_Order_Size()
    {     
        var handler = new GetOrderSizeQueryHandler(_dbContextMock.Object);
        // Act
        var result = await handler.Handle(new GetOrderSizeQuery { OrderId = 1 }, CancellationToken.None);
        // Assert
        Assert.Equal("Small", result);
    }

    #region DatabaseInitilization
    /// <summary>
    /// Initializes Mock database with mocked object
    /// </summary>
    private void OrderDetailsMock()
    {
        _dbContextMock.Setup(x => x.OrderDetails).Returns(new List<OrderDetail>{
            new OrderDetail()
            {
               Id = 1,
               ProductId = 2,
               OrderId = 1,
               Quantity =20,
               Order = new Order
               {
                   Id = 1,
                   TotalAmount = 200
               },
               Product = new Product
               {
                 Id = 2,
                 Name ="shoe"
               }     
            },
              new OrderDetail(){
               Id = 5,
               ProductId = 1,
               OrderId = 1,
               Quantity =25,
               Order = new Order
               {
                   Id = 1,
                   TotalAmount = 200
               },
               Product = new Product
               {
                 Id = 3,
                 Name ="pen"
               }
            }

        }.AsQueryable().BuildMockDbSet().Object);
    }
    #endregion
}
