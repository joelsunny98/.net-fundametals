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
using FluentAssertions;
using Xunit;

namespace RetailStore.Tests.Requests.CustomerManagement
{
    public class GetPremiumCustomersQueryTest
    {
        [Fact]
        public async Task Handle_Should_Return_PremiumCustomersDtoList()
        {
            // Arrange
            var customerId1 = 1;
            var customerName1 = "Customer 1";
            var phoneNumber1 = 1234567890;

            var customerId2 = 2;
            var customerName2 = "Customer 2";
            var phoneNumber2 = 9876543210;

            var customers = new List<Customer>
            {
                new Customer { Id = customerId1, Name = customerName1, PhoneNumber = phoneNumber1 },
                new Customer { Id = customerId2, Name = customerName2, PhoneNumber = phoneNumber2 }
            };

            var orders = new List<Order>
            {
                new Order { CustomerId = customerId1, TotalAmount = 100 },
                new Order { CustomerId = customerId2, TotalAmount = 200 },
                new Order { CustomerId = customerId1, TotalAmount = 50 }
            };

            var dbContextMock = new Mock<IRetailStoreDbContext>();
            dbContextMock.Setup(db => db.Customers).Returns(DbSetMock(customers.AsQueryable()));
            dbContextMock.Setup(db => db.Orders).Returns(DbSetMock(orders.AsQueryable()));

            var loggerMock = new Mock<ILogger<GetPremiumCustomersQuery>>();
            var premiumCodeServiceMock = new Mock<IPremiumCodeService>();

            var handler = new GetPremiumCustomersQueryHandler(dbContextMock.Object, loggerMock.Object, premiumCodeServiceMock.Object);
            var query = new GetPremiumCustomersQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once()
            );
        }

        private DbSet<T> DbSetMock<T>(IQueryable<T> data) where T : class
        {
            var dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return dbSetMock.Object;
        }
    }
}
