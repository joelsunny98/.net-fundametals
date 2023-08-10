using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.CustomerManagement;

namespace RetailStore.Tests.Requests.CustomerManagement;

public class UpdateCustomerByIdCommandTest
{
    private readonly Mock<IRetailStoreDbContext> _mockDbContext;
    private readonly Mock<ILogger<UpdateCustomerCommandHandler>> _logger;
    private readonly UpdateCustomerCommandHandler _handler;
    public UpdateCustomerByIdCommandTest()
    {
        _mockDbContext = new Mock<IRetailStoreDbContext>();
        _logger = new Mock<ILogger<UpdateCustomerCommandHandler>>();
        _handler = new UpdateCustomerCommandHandler(_mockDbContext.Object, _logger.Object);
        MockCustomerdata();
    }

    [Fact]
    public async Task Handle_CustomerExists_CustomerUpdated()
    {
        
        var updateCommand = new UpdateCustomerCommand
        {
            CustomerId = 1,
            CustomerName = "Josmy",
            PhoneNumber = 9876543210,
            
        };

        var response = await _handler.Handle(updateCommand, CancellationToken.None);

        _mockDbContext.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);

        Assert.Equal(1, response);
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
               CreatedOn= DateTime.Now,
               UpdatedOn = DateTime.Now,
               
            }
        }.AsQueryable().BuildMockDbSet().Object);
    }
    #endregion
}
