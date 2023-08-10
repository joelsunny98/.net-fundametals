using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.OrderManagement;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Xunit;

public class SendSmsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ValidRequest_ReturnsTotalPurchaseAmount()
    {
        // Arrange
        var dbContextMock = new Mock<IRetailStoreDbContext>();

        var customer = new Customer
        {
            Id = 1,
            Name = "John",
            PhoneNumber = 1234567890
        };

        var orders = new List<Order>
        {
            new Order { CustomerId = 1, TotalAmount = 50 },
            new Order { CustomerId = 1, TotalAmount = 75 }
        }.AsQueryable();

        dbContextMock.Setup(db => db.Customers).Returns(MockDbSet(new List<Customer> { customer }.AsQueryable()));
        dbContextMock.Setup(db => db.Orders).Returns(MockDbSet(orders));
    }

    private static DbSet<T> MockDbSet<T>(IQueryable<T> data) where T : class
    {
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        return mockSet.Object;
    }
}
