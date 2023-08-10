using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.CustomerManagement;

namespace RetailStore.Tests.Requests.CustomerManagement;

public class GetCustomerByIdQueryTest
{
    private readonly Mock<IRetailStoreDbContext> _mockDbContext;
    private readonly Mock<ILogger<GetCustomerByIdQueryHandler>> _logger;
    public GetCustomerByIdQueryTest()
    {
        _mockDbContext = new Mock<IRetailStoreDbContext>();
        _logger = new Mock<ILogger<GetCustomerByIdQueryHandler>>();
        MockCustomerdata();
    }

    [Fact]
    public async Task GetCustomerByIdQueryTests_ShouldReturn_CorrectDataBasedOnTheId()
    {
        var handler = new GetCustomerByIdQueryHandler(_mockDbContext.Object,_logger.Object);
        var result = await handler.Handle(new GetCustomerByIdQuery { CustomerId = 1 }, CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal("Test", result.CustomerName);
        Assert.Equal(1234567890, result.PhoneNumber);
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
