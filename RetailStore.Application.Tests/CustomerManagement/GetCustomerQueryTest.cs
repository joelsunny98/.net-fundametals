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

namespace RetailStore.Tests.CustomerManagement;

public class GetCustomerQueryHandlerTests
{

    [Fact]
    public async Task Handle_Should_Return_List_Of_CustomerDtos()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "John Doe", PhoneNumber = 1234567890 },
            new Customer { Id = 2, Name = "Jane Smith", PhoneNumber = 9876543210 }
        };

        var dbContextMock = new Mock<IRetailStoreDbContext>();
        var customersDbSetMock = new Mock<DbSet<Customer>>();
        customersDbSetMock.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(customers.AsQueryable().Provider);
        customersDbSetMock.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(customers.AsQueryable().Expression);
        customersDbSetMock.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(customers.AsQueryable().ElementType);
        customersDbSetMock.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(() => customers.GetEnumerator());
        dbContextMock.Setup(db => db.Customers).Returns(customersDbSetMock.Object);

        var loggerMock = new Mock<ILogger<GetCustomerQueryHandler>>();
        var handler = new GetCustomerQueryHandler(dbContextMock.Object, loggerMock.Object);

        // Act
        var result = await handler.Handle(new GetCustomersQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(customers.Count, result.Count);
        Assert.Equal(customers[0].Name, result[0].CustomerName);
        Assert.Equal(customers[1].PhoneNumber, result[1].PhoneNumber);

        loggerMock.Verify(
            x => x.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once()
        );
    }
}
