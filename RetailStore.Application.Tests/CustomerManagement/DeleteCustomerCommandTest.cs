using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using RetailStore.Model;
using RetailStore.Repository;
using RetailStore.Requests.CustomerManagement;
using Xunit;

namespace RetailStore.Tests
{
    public class DeleteCustomerCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Delete_Customer_And_Return_Success_Message()
        {
            // Arrange
            var customerId = 1;
            var deletedCustomer = new Customer { Id = customerId, Name = "John Doe", PhoneNumber = 1234567890 };

            var customerRepositoryMock = new Mock<IRepository<Customer>>();
            customerRepositoryMock.Setup(repo => repo.GetById(customerId)).ReturnsAsync(deletedCustomer);
            customerRepositoryMock.Setup(repo => repo.Delete(customerId)).ReturnsAsync(deletedCustomer);

            var loggerMock = new Mock<ILogger<DeleteCustomerCommandHandler>>();

            var handler = new DeleteCustomerCommandHandler(customerRepositoryMock.Object, loggerMock.Object);
            var command = new DeleteCustomerCommand { CustomerId = customerId };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(string.Format(ValidationMessage.CustomerDeletedSuccessfully, customerId), result);

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                ),
                Times.Once()
            );
        }

        [Fact]
        public async Task Handle_Should_Return_Not_Found_Message_When_Customer_Does_Not_Exist()
        {
            // Arrange
            var customerRepositoryMock = new Mock<IRepository<Customer>>();
            customerRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Customer)null);

            var loggerMock = new Mock<ILogger<DeleteCustomerCommandHandler>>();

            var handler = new DeleteCustomerCommandHandler(customerRepositoryMock.Object, loggerMock.Object);
            var command = new DeleteCustomerCommand { CustomerId = 1 };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(string.Format(ValidationMessage.CustomerDoesNotExist, command.CustomerId), result);

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                ),
                Times.Never()
            );
        }
    }
}
