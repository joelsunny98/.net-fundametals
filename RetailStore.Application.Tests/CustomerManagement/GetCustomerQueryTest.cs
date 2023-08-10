using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;

namespace RetailStore.Requests.CustomerManagement;

public class GetCustomerQueryTest
{
    private readonly Mock<IRetailStoreDbContext> _mockDbContext;
    private readonly Mock<ILogger<GetCustomerQueryHandler>> _logger;
    public GetCustomerQueryTest()
    {
        _mockDbContext = new Mock<IRetailStoreDbContext>();
        _logger = new Mock<ILogger<GetCustomerQueryHandler>>();
        MockCustomerdata();
    }

    [Fact]
    public async Task Handle_Should_Return_List_Of_CustomerDtos()
    {
       var handler = new GetCustomerQueryHandler(_mockDbContext.Object,_logger.Object);
       var result = await handler.Handle(new GetCustomersQuery(),CancellationToken.None);
       var customer = result.First();
       Assert.NotNull(customer);
       Assert.Equal("Test", customer.CustomerName);
       Assert.Equal(1234567890, customer.PhoneNumber);
    }


    #region DatabaseInitilization
    /// <summary>
    /// Initializes Mock database with mocked object
    /// </summary>
    private void MockCustomerdata()
    {
        _mockDbContext.Setup(x => x.Customers).Returns(new List<Customer>{new Customer()
            {
               Id = 1,
               Name = "Test",
               PhoneNumber = 1234567890,
            }
        }.AsQueryable().BuildMockDbSet().Object);
    }
    #endregion
}
