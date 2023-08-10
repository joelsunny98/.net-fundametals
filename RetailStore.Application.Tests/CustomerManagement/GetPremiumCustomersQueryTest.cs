using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;

namespace RetailStore.Requests.CustomerManagement;

public class GetPremiumCustomersQueryTest
{
    private readonly Mock<IRetailStoreDbContext> _dbContextMock;
    private readonly Mock<ILogger<GetPremiumCustomersQuery>> _loggerMock;
    private readonly Mock<IPremiumCodeService> _premiumCodeServiceMock;
    private readonly GetPremiumCustomersQueryHandler _handler;

    public GetPremiumCustomersQueryTest()
    {
        _dbContextMock = new Mock<IRetailStoreDbContext>();
        _loggerMock = new Mock<ILogger<GetPremiumCustomersQuery>>();
        _premiumCodeServiceMock = new Mock<IPremiumCodeService>();

        _handler = new GetPremiumCustomersQueryHandler(
            _dbContextMock.Object, _loggerMock.Object, _premiumCodeServiceMock.Object);

        MockCustomerdata();
    }

    [Fact]
    public async Task Handle_Should_Return_PremiumCustomersDtoList()
    {
        // Act
        var result = await _handler.Handle(new GetPremiumCustomersQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once()
        );
    }

    private void MockCustomerdata()
    {
        _dbContextMock.Setup(x => x.Customers).Returns(new List<Customer>
        {
            new Customer 
            { 
                Id = 1,
                Name = "Austin", 
                PhoneNumber = 9947003224 
            }
        }.AsQueryable().BuildMockDbSet().Object);
    }
}
