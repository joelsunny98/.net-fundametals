using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using RetailStore.Constants;
using RetailStore.Model;
using RetailStore.Repository;
using RetailStore.Requests.CustomerManagement;
using Xunit;

namespace RetailStore.Tests.Requests.CustomerManagement
{
    public class DeleteCustomerCommandHandlerTests
    {
        [Fact]
        public async Task Handle_DeletesExistingCustomer_ReturnsSuccessMessage()
        {
            // Arrange
            var customerId = 1;
            var customerToDelete = new Customer { Id = customerId, Name = "Test Customer" };

            var customerRepositoryMock = new Mock<IRepository<Customer>>();
            customerRepositoryMock.Setup(repo => repo.GetById(customerId)).ReturnsAsync(customerToDelete);
            customerRepositoryMock.Setup(repo => repo.Delete(customerId)).ReturnsAsync(customerToDelete);

            var loggerMock = new Mock<ILogger<DeleteCustomerCommandHandler>>();

            var command = new DeleteCustomerCommand { CustomerId = customerId };
            var handler = new DeleteCustomerCommandHandler(customerRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(string.Format(ValidationMessage.CustomerDeletedSuccessfully, customerId), result);
            customerRepositoryMock.Verify(repo => repo.GetById(customerId), Times.Once);
            customerRepositoryMock.Verify(repo => repo.Delete(customerId), Times.Once);
            loggerMock.Verify(logger => logger.LogInformation(LogMessage.DeleteItem, customerId), Times.Once);
        }

        [Fact]
        public async Task Handle_CustomerDoesNotExist_ReturnsErrorMessage()
        {
            // Arrange
            var customerId = 1;

            var customerRepositoryMock = new Mock<IRepository<Customer>>();
            customerRepositoryMock.Setup(repo => repo.GetById(customerId)).ReturnsAsync((Customer)null);

            var loggerMock = new Mock<ILogger<DeleteCustomerCommandHandler>>();

            var command = new DeleteCustomerCommand { CustomerId = customerId };
            var handler = new DeleteCustomerCommandHandler(customerRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(string.Format(ValidationMessage.CustomerDoesNotExist, customerId), result);
            customerRepositoryMock.Verify(repo => repo.GetById(customerId), Times.Once);
            customerRepositoryMock.Verify(repo => repo.Delete(It.IsAny<int>()), Times.Never);
            loggerMock.Verify(logger => logger.LogInformation(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }
    }
}
