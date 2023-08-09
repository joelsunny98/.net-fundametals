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
    public class GetPremiumCustomersQueryHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Return_List_Of_PremiumCustomerDtos()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { CustomerId = 1, TotalAmount = 100 },
                new Order { CustomerId = 1, TotalAmount = 200 },
                new Order { CustomerId = 2, TotalAmount = 150 },
            };

            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "John Doe", PhoneNumber = 1234567890 },
                new Customer { Id = 2, Name = "Jane Smith", PhoneNumber = 9876543210 }
            };

            var dbContextMock = new Mock<IRetailStoreDbContext>();
            var ordersDbSetMock = new Mock<DbSet<Order>>();
            var customersDbSetMock = new Mock<DbSet<Customer>>();

            ordersDbSetMock.As<IQueryable<Order>>().Setup(m => m.Provider).Returns(orders.AsQueryable().Provider);
            ordersDbSetMock.As<IQueryable<Order>>().Setup(m => m.Expression).Returns(orders.AsQueryable().Expression);
            ordersDbSetMock.As<IQueryable<Order>>().Setup(m => m.ElementType).Returns(orders.AsQueryable().ElementType);
            ordersDbSetMock.As<IQueryable<Order>>().Setup(m => m.GetEnumerator()).Returns(() => orders.GetEnumerator());

            customersDbSetMock.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(customers.AsQueryable().Provider);
            customersDbSetMock.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(customers.AsQueryable().Expression);
            customersDbSetMock.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(customers.AsQueryable().ElementType);
            customersDbSetMock.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(() => customers.GetEnumerator());

            dbContextMock.Setup(db => db.Orders).Returns(ordersDbSetMock.Object);
            dbContextMock.Setup(db => db.Customers).Returns(customersDbSetMock.Object);

            var loggerMock = new Mock<ILogger<GetPremiumCustomersQueryHandler>>();
            var premiumCodeServiceMock = new Mock<IPremiumCodeService>();
            premiumCodeServiceMock.Setup(service => service.GeneratePremiumCode()).Returns("PREM123");

            var handler = new GetPremiumCustomersQueryHandler(dbContextMock.Object, loggerMock.Object, premiumCodeServiceMock.Object);
            var query = new GetPremiumCustomersQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            var johnDoe = result.FirstOrDefault(c => c.CustomerId == 1);
            Assert.NotNull(johnDoe);
            Assert.Equal("John Doe", johnDoe.CustomerName);
            Assert.Equal(300, johnDoe.TotalPurchaseAmount);
            Assert.Equal("PREM123", johnDoe.PremiumCode);

            var janeSmith = result.FirstOrDefault(c => c.CustomerId == 2);
            Assert.NotNull(janeSmith);
            Assert.Equal("Jane Smith", janeSmith.CustomerName);
            Assert.Equal(150, janeSmith.TotalPurchaseAmount);
            Assert.Equal("PREM123", janeSmith.PremiumCode);

            loggerMock.Verify(
                x => x.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once()
            );
        }
    }
}
