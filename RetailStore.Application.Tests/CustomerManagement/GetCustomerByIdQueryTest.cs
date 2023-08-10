using System;
using System.Collections.Generic;
using System.Linq;
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
        [Theory]
        [InlineData(1, "John Doe", 1234567890)]
        public async Task Handle_Should_Return_CustomerDto(int customerId, string customerName, int phoneNumber)
        {
            // Arrange
            var customer = new Customer
            {
                Id = customerId,
                Name = customerName,
                PhoneNumber = phoneNumber
            };

            var customers = new List<Customer> { customer };

            var customersDbSetMock = new Mock<DbSet<Customer>>();
            customersDbSetMock.As<IQueryable<Customer>>().Setup(d => d.Provider).Returns(customers.AsQueryable().Provider);
            customersDbSetMock.As<IQueryable<Customer>>().Setup(d => d.Expression).Returns(customers.AsQueryable().Expression);
            customersDbSetMock.As<IQueryable<Customer>>().Setup(d => d.ElementType).Returns(customers.AsQueryable().ElementType);
            customersDbSetMock.As<IQueryable<Customer>>().Setup(d => d.GetEnumerator()).Returns(customers.GetEnumerator());

            var dbContextMock = new Mock<IRetailStoreDbContext>();
            dbContextMock.Setup(db => db.Customers).Returns(customersDbSetMock.Object);

            var loggerMock = new Mock<ILogger<GetCustomerByIdQueryHandler>>();
            var handler = new GetCustomerByIdQueryHandler(dbContextMock.Object, loggerMock.Object);
            var query = new GetCustomerByIdQuery { CustomerId = customerId };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer.Name, result.CustomerName);
            Assert.Equal(customer.PhoneNumber, result.PhoneNumber);

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

    }
}
