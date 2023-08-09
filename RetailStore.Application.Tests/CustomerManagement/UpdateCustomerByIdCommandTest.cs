using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using RetailStore.Contracts;
using RetailStore.Dtos;
using RetailStore.Model;
using RetailStore.Requests.CustomerManagement;
using Xunit;

namespace RetailStore.Tests
{
    public class UpdateCustomerCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_ShouldUpdateCustomer()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                Name = "Old Name",
                PhoneNumber = 1234567890,
                UpdatedOn = DateTime.UtcNow
            };

            var dbContextMock = new Mock<IRetailStoreDbContext>();
            dbContextMock.Setup(db => db.Customers.FindAsync(1)).ReturnsAsync(customer);
            dbContextMock.Setup(db => db.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(1); // Number of changes saved

            var loggerMock = new Mock<ILogger<UpdateCustomerCommandHandler>>();

            var handler = new UpdateCustomerCommandHandler(dbContextMock.Object, loggerMock.Object);
            var command = new UpdateCustomerCommand
            {
                CustomerId = 1,
                CustomerName = "New Name",
                PhoneNumber = 9876543210
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(1, result); // Customer Id returned
            Assert.Equal("New Name", customer.Name);
            Assert.Equal(9876543210, customer.PhoneNumber);
            Assert.True(DateTime.UtcNow - customer.UpdatedOn < TimeSpan.FromSeconds(1)); // Verify updated timestamp

            loggerMock.Verify(
                x => x.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once()
            );
        }

        [Fact]
        public async Task Handle_InvalidCustomerId_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var dbContextMock = new Mock<IRetailStoreDbContext>();
            dbContextMock.Setup(db => db.Customers.FindAsync(1)).ReturnsAsync((Customer)null);

            var loggerMock = new Mock<ILogger<UpdateCustomerCommandHandler>>();

            var handler = new UpdateCustomerCommandHandler(dbContextMock.Object, loggerMock.Object);
            var command = new UpdateCustomerCommand
            {
                CustomerId = 1,
                CustomerName = "New Name",
                PhoneNumber = 9876543210
            };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(command, CancellationToken.None));

            loggerMock.Verify(
                x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once()
            );
        }

    }
}
