using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RetailStore.Contracts;
using RetailStore.Dtos;
using RetailStore.Model;
using RetailStore.Requests.CustomerManagement;
using Xunit;

namespace RetailStore.Tests
{
    public class GetCustomerByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Return_CustomerDto_When_Customer_Exists()
        {
            // Arrange
            var customerId = 1;
            var customerName = "John Doe";
            var phoneNumber = 1234567890;

            var customer = new Customer { Id = customerId, Name = customerName, PhoneNumber = phoneNumber };
            var dbContextMock = new Mock<IRetailStoreDbContext>();
            dbContextMock.Setup(db => db.Customers).ReturnsDbSet(new[] { customer });

            var loggerMock = new Mock<ILogger<GetCustomerByIdQueryHandler>>();
            var handler = new GetCustomerByIdQueryHandler(dbContextMock.Object, loggerMock.Object);
            var query = new GetCustomerByIdQuery { CustomerId = customerId };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(customerName, result.CustomerName);
            Assert.Equal(phoneNumber, result.PhoneNumber);

            loggerMock.Verify(x => x.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once());
        }

        [Fact]
        public async Task Handle_Should_Throw_KeyNotFoundException_When_Customer_Does_Not_Exist()
        {
            // Arrange
            var customerId = 1;

            var dbContextMock = new Mock<IRetailStoreDbContext>();
            dbContextMock.Setup(db => db.Customers).ReturnsDbSet(Array.Empty<Customer>());

            var loggerMock = new Mock<ILogger<GetCustomerByIdQueryHandler>>();
            var handler = new GetCustomerByIdQueryHandler(dbContextMock.Object, loggerMock.Object);
            var query = new GetCustomerByIdQuery { CustomerId = customerId };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(query, CancellationToken.None));

            loggerMock.Verify(x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once());
        }
    }
}
