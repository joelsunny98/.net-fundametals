using FluentValidation.TestHelper;
using MockQueryable.Moq;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.CustomerManagement;

namespace RetailStore.Tests.Requests.CustomerManagement;

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
    [Fact]
    public void Should_Have_Error_When_CustomerName_Is_Null()
    {
       
        var command = new AddCustomerCommand { CustomerName = "" };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.CustomerName);
    }

    [Fact]
    public void Should_Have_Error_When_PhoneNumber_Is_Invalid()
    {

        var command = new AddCustomerCommand { PhoneNumber = 12345 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.PhoneNumber);
    }

    [Fact]
    public void Should_Have_Error_When_PhoneNumber_Is_Not_Unique()
    {
        

        var command = new AddCustomerCommand { PhoneNumber = 9947003224 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.PhoneNumber);
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Command_Is_Valid()
    { 
        var command = new AddCustomerCommand
        {
            CustomerName = "John Doe",
            PhoneNumber = 1234567890
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    #region DatabaseInitilization
    /// <summary>
    /// Initializes Mock database with mocked object
    /// </summary>
    private void MockCustomerdata()
    {
        _dbContextMock.Setup(x => x.Customers).Returns(new List<Customer>{new Customer()
            {
               Id = 1,
               Name = "Austin",
               PhoneNumber = 9947003224
            }
        }.AsQueryable().BuildMockDbSet().Object);
    }
#endregion
}
