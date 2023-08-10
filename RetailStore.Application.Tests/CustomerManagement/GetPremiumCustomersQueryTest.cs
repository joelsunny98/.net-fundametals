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
using MockQueryable.Moq;

namespace RetailStore.Tests.Requests.CustomerManagement
{
    public class GetPremiumCustomersQueryTest
    {
        private readonly Mock<IRetailStoreDbContext> _dbContextMock;
        private readonly Mock<ILogger<GetPremiumCustomersQuery>> _loggerMock;
        private readonly Mock<IPremiumCodeService> _premiumCodeServiceMock;
        private readonly GetPremiumCustomersQueryHandler _handler;

        public GetPremiumCustomersQueryTest()
        {
            _dbContextMock = new Mock<IRetailStoreDbContext>();
            _loggerMock = new Mock<ILogger<GetPremiumCustomersQuery>>();
            _premiumCodeServiceMock = new Mock<IPremiumCodeService>();

            _handler = new GetPremiumCustomersQueryHandler(
                _dbContextMock.Object, _loggerMock.Object, _premiumCodeServiceMock.Object);

            MockCustomerdata();
        }

        [Theory]
        [InlineData(1, "Customer 1", 1234567890, 2, "Customer 2", 9876543210)]
        public async Task Handle_Should_Return_PremiumCustomersDtoList(
            int customerId1, string customerName1, int phoneNumber1,
            int customerId2, string customerName2, int phoneNumber2)
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

            _dbContextMock.Setup(db => db.Customers).Returns(DbSetMock(customers.AsQueryable()));
            _dbContextMock.Setup(db => db.Orders).Returns(DbSetMock(orders.AsQueryable()));

            // Act
            var result = await _handler.Handle(new GetPremiumCustomersQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();

            _loggerMock.Verify(
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

        private void MockCustomerdata()
        {
            _dbContextMock.Setup(x => x.Customers).Returns(new List<Customer>
            {
                new Customer { Id = 1, Name = "Austin", PhoneNumber = 9947003224 }
            }.AsQueryable().BuildMockDbSet().Object);
        }
    }
}
