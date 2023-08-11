using FluentValidation.TestHelper;
using MockQueryable.Moq;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;

namespace RetailStore.Requests.CustomerManagement;

public class AddCustomerCommandValidatorTests
{
    private readonly Mock<IRetailStoreDbContext> _dbContextMock;
    private readonly AddCustomerCommandValidator _validator;

    public AddCustomerCommandValidatorTests()
    {
        _dbContextMock = new Mock<IRetailStoreDbContext>();
        _validator = new AddCustomerCommandValidator(_dbContextMock.Object);
        MockCustomerdata();
    }

    [Theory]
    [InlineData("")]
    public void Should_Have_Error_When_CustomerName_Is_Null(string customerName)
    {
        var command = new AddCustomerCommand { CustomerName = customerName };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.CustomerName);
    }

    [Theory]
    [InlineData(12345)]
    [InlineData(9947003224)]
    public void Should_Have_Error_When_PhoneNumber_Is_Invalid(long phoneNumber)
    {
        var command = new AddCustomerCommand { PhoneNumber = phoneNumber };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.PhoneNumber);
    }

    [Theory]
    [InlineData("John Doe", 1234567890)]
    public void Should_Not_Have_Errors_When_Command_Is_Valid(string customerName, long phoneNumber)
    {
        var command = new AddCustomerCommand
        {
            CustomerName = customerName,
            PhoneNumber = phoneNumber
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    #region DatabaseInitialization
    /// <summary>
    /// Initializes Mock database with mocked object
    /// </summary>
    private void MockCustomerdata()
    {
        var customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "Austin", PhoneNumber = 9947003224 }
        }.AsQueryable().BuildMockDbSet();

        _dbContextMock.Setup(x => x.Customers).Returns(customers.Object);
    }
    #endregion
}
