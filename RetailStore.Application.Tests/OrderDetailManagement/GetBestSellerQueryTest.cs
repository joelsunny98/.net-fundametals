using MockQueryable.Moq;
using Moq;
using RetailStore.Contracts;
using RetailStore.Dtos;
using RetailStore.Model;
using RetailStore.Requests.OrderDetailManagement;

namespace RetailStore.Tests.Requests.OrderDetailManagement;

public class GetBestSellerQueryTest
{
    private readonly Mock<IRetailStoreDbContext> _dbContextMock;

    public GetBestSellerQueryTest()
    {
        _dbContextMock = new Mock<IRetailStoreDbContext>();
        BestSellerMock();
    }

    [Fact]
    public async Task Handle_Should_Return_BestSeller()
    {
        var handler = new GetBestSellerQueryHandler(_dbContextMock.Object);

        var query = new GetBestSellerQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);  

    }

    #region DatabaseInitilization
    /// <summary>
    /// Initializes Mock database with mocked object
    /// </summary>
    private void BestSellerMock()
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