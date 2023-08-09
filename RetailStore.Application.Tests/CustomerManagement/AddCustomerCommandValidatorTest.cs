using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.CustomerManagement;
using Xunit;

namespace RetailStore.Tests.Requests.CustomerManagement;

public class AddCustomerCommandValidatorTests
{
    private readonly Mock<IRetailStoreDbContext> _dbContext;
    private readonly AddCustomerCommandValidator _validator;

    public AddCustomerCommandValidatorTests()
    {
        _dbContext = new Mock<IRetailStoreDbContext>();
        _validator = new AddCustomerCommandValidator(_dbContext.Object);
    }

    // Test for Customer Name requirements
    [Fact]
    public void Should_have_error_when_CustomerName_is_null_or_empty()
    {
        var model = new AddCustomerCommand { CustomerName = null };
        var result = _validator.Validate(model);
        Assert.Contains(result.Errors, x => x.PropertyName == "CustomerName");

        model = new AddCustomerCommand { CustomerName = string.Empty };
        result = _validator.Validate(model);
        Assert.Contains(result.Errors, x => x.PropertyName == "CustomerName");
    }

    // Tests for PhoneNumber requirements
    [Fact]
    public void Should_have_error_when_PhoneNumber_is_not_10_digits()
    {
        var model = new AddCustomerCommand { PhoneNumber = 1234567890 };
        var result = _validator.Validate(model);
        Assert.Contains(result.Errors, x => x.PropertyName == "PhoneNumber");
    }

    [Fact]
    public void Should_have_error_when_PhoneNumber_is_not_unique()
    {
        // Create a command with a PhoneNumber that already exists
        var existingCustomer = new Customer { PhoneNumber = 1234567890L };
        var model = new AddCustomerCommand { PhoneNumber = existingCustomer.PhoneNumber };

        // Setup dbContext to return the existing customer when queried
        var mockSet = CreateMockSet(new[] { existingCustomer });
        _dbContext.Setup(db => db.Customers).Returns(mockSet.Object);

        // Validate the model and assert that it contains an error for the PhoneNumber property
        var result = _validator.Validate(model);
        Assert.Contains(result.Errors, x => x.PropertyName == "PhoneNumber");
    }

    [Fact]
    public void Should_not_have_error_when_PhoneNumber_is_unique()
    {
        // Create a command with a PhoneNumber that is unique
        var model = new AddCustomerCommand { PhoneNumber = 5555555555L };

        // Setup dbContext to return an empty list when queried (no existing customers with this PhoneNumber)
        var mockSet = CreateMockSet(Enumerable.Empty<Customer>());
        _dbContext.Setup(db => db.Customers).Returns(mockSet.Object);

        // Validate the model and assert that it does not contain any errors for the PhoneNumber property
        var result = _validator.Validate(model);
        Assert.DoesNotContain(result.Errors, x => x.PropertyName == "PhoneNumber");
    }

    [Fact]
    public void Should_have_error_when_PhoneNumber_is_below_min_value()
    {
        var model = new AddCustomerCommand { PhoneNumber = 0 };
        var result = _validator.Validate(model);
        Assert.Contains(result.Errors, x => x.PropertyName == "PhoneNumber");
    }

    [Fact]
    public void Should_have_error_when_PhoneNumber_is_above_max_value()
    {
        var model = new AddCustomerCommand { PhoneNumber = long.MaxValue };
        var result = _validator.Validate(model);
        Assert.Contains(result.Errors, x => x.PropertyName == "PhoneNumber");
    }


    // Helper method to create a mocked DbSet
    private Mock<DbSet<T>> CreateMockSet<T>(IEnumerable<T> data) where T : class
    {
        var queryable = data.AsQueryable();

        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

        return mockSet;
    }
}