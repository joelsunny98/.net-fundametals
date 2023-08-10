using Microsoft.Extensions.Logging;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.CustomerManagement;

namespace RetailStore.Tests.Requests.CustomerManagement;

public class AddCustomerCommandHandlerTests
{
    private readonly Mock<IRetailStoreDbContext> _dbContext;
    private readonly Mock<ILogger<AddCustomerCommandHandler>> _logger;
    private readonly AddCustomerCommandHandler _handler;

    public AddCustomerCommandHandlerTests()
    {
        _dbContext = new Mock<IRetailStoreDbContext>();
        _logger = new Mock<ILogger<AddCustomerCommandHandler>>();
        _handler = new AddCustomerCommandHandler(_dbContext.Object, _logger.Object);
    }

    [Fact]
    public async Task Handle_SaveChangesAsyncReturnsZero_Failure()
    {
        // Arrange
        var command = new AddCustomerCommand
        {
            CustomerName = "Test3",
            PhoneNumber = 5555555555
        };

        // Mock dbContext so the SaveChangesAsync method returns 0
        _dbContext.Setup(d => d.Customers.Add(It.IsAny<Customer>()));
        var response = await _handler.Handle(command, CancellationToken.None);

        // Act and Assert
        _dbContext.Verify(x => x.Customers.Add(It.IsAny<Customer>()), Times.Once);
        _dbContext.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
}
