//using Microsoft.Extensions.Logging;
//using MockQueryable.Moq;
//using Moq;
//using RetailStore.Contracts;
//using RetailStore.Model;

//namespace RetailStore.Requests.CustomerManagement;

//public class GetPremiumCustomersQueryTest
//{
//    private readonly Mock<IRetailStoreDbContext> _mockDbContext;
//    private readonly Mock<ILogger<GetCustomerQueryHandler>> _logger;
//    public GetPremiumCustomersQueryTest()
//    {
//        _mockDbContext = new Mock<IRetailStoreDbContext>();
//        _logger = new Mock<ILogger<GetPremiumCustomersQuery>>();
//        _mockpremiumCodeService = new Mock<IPremiumCodeService>();
//        MockPremiumOrderdata();
//    }
//    [Fact]
//    public async Task Handle_ReturnsPremiumCustomers()
//    {




//    }

//    #region DatabaseInitilization
//    /// <summary>
//    /// Initializes Mock database with mocked object
//    /// </summary>
//    private void MockPremiumOrderdata()
//    {
//        _mockDbContext.Setup(x => x.Orders).Returns(new List<Order>{new Order()
//        {
//             Id = 1,
//             CustomerId = 2,
//             TotalAmount = 2000,
//             Customer = new Customer
//             {
//                 Name ="Vismaya",
//                 PhoneNumber =987678998,
//             }


//            }
//        }.AsQueryable().BuildMockDbSet().Object);
//    }
//    #endregion
//}
