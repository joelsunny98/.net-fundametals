using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using RetailStore.Contracts;
using RetailStore.Helpers;
using RetailStore.Model;
using SixLabors.ImageSharp.Metadata;

namespace RetailStore.Requests.CustomerManagement;

public class GetPremiumCustomersQueryTest
{
    private readonly Mock<IRetailStoreDbContext> _context;
    private readonly Mock<IPremiumCodeService> _service;

    public GetPremiumCustomersQueryTest()
    {
        _context = new Mock<IRetailStoreDbContext>();
        _service = new Mock<IPremiumCodeService>();
        MockPremiumOrderdata();
    }

    [Fact]
    public async Task Handle_ReturnsPremiumCustomersWithPremiumCodes()
    {
        // Arrange
        var handler = new GetPremiumCustomersQueryHandler(_context.Object, _service.Object);

        _service.Setup(service => service.GeneratePremiumCode())
                .Returns("MockedPremiumCode");

        // Act
        var result = await handler.Handle(new GetPremiumCustomersQuery(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);

        foreach (var premiumCustomer in result)
        {
            Assert.NotNull(premiumCustomer.PremiumCode);
        }

        _context.Verify(db => db.Orders, Times.Once);
        _service.Verify(service => service.GeneratePremiumCode(), Times.Exactly(result.Count));
    }



    #region DatabaseInitilization
    /// <summary>
    /// Initializes Mock database with mocked object
    /// </summary>
    private void MockPremiumOrderdata()
    {
        _context.Setup(x => x.Orders).Returns(new List<Order>{new Order()
    {
         Id = 1,
         CustomerId = 2,
         TotalAmount = 2000,
         Customer = new Customer
         {
             Name ="Vismaya",
             PhoneNumber =987678998,
         }
        }
    }.AsQueryable().BuildMockDbSet().Object);
    }
    #endregion
}