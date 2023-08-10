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

namespace RetailStore.Application.Tests.CustomerManagement
{
    public class GetPremiumCustomersQueryTest
    {
        [Theory]
        [InlineData(1, "Customer 1", 1234567890, 2, "Customer 2", 9876543210)]
        public async Task Handle_Should_Return_PremiumCustomersDtoList(int customerId1, string customerName1, int phoneNumber1, int customerId2, string customerName2, int phoneNumber2)
        {
            // Arrange
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
            dbContextMock.Setup(db => db.Customers).Returns(DbSetMock(customers));
            dbContextMock.Setup(db => db.Orders).Returns(DbSetMock(orders));

            var loggerMock = new Mock<ILogger<GetPremiumCustomersQuery>>();
            var premiumCodeServiceMock = new Mock<IPremiumCodeService>();

            var handler = new GetPremiumCustomersQueryHandler(dbContextMock.Object, loggerMock.Object, premiumCodeServiceMock.Object);
            var query = new GetPremiumCustomersQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);

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

        private DbSet<T> DbSetMock<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            return dbSetMock.Object;
        }
    }
}
