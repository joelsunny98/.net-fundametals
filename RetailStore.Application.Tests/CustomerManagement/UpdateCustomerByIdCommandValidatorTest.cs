using FluentValidation.TestHelper;
using Moq;
using RetailStore.Contracts;
using RetailStore.Requests.CustomerManagement;
using Xunit;

namespace RetailStore.Tests.Requests.CustomerManagement;

public class UpdateCustomerCommandValidatorTests
{
private readonly UpdateCustomerCommandValidator _validator;
private readonly Mock<IRetailStoreDbContext> _dbContextMock;

public UpdateCustomerCommandValidatorTests()
{
    _dbContextMock = new Mock<IRetailStoreDbContext>();
    _validator = new UpdateCustomerCommandValidator(_dbContextMock.Object);
}

[Fact]
public void Should_have_error_when_CustomerId_is_not_greater_than_zero()
{
    // Arrange
    var command = new UpdateCustomerCommand { CustomerId = 0 };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(c => c.CustomerId);
}

[Fact]
public void Should_have_error_when_CustomerName_is_null_or_empty()
{
    // Arrange
    var command = new UpdateCustomerCommand { CustomerName = null };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(c => c.CustomerName);
}

[Fact]
public void Should_have_error_when_CustomerName_is_longer_than_25_characters()
{
    // Arrange
    var command = new UpdateCustomerCommand { CustomerName = "This is a very long customer name that exceeds the maximum length of 25 characters" };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(c => c.CustomerName);
}

[Fact]
public void Should_have_error_when_PhoneNumber_is_null_or_empty()
{
    // Arrange
    var command = new UpdateCustomerCommand { PhoneNumber = 0 };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(c => c.PhoneNumber);
}

[Fact]
public void Should_have_error_when_PhoneNumber_is_not_valid()
{
    // Arrange
    var command = new UpdateCustomerCommand { PhoneNumber = 12345 };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(c => c.PhoneNumber);
}

[Fact]
public void Should_have_error_when_PhoneNumber_is_not_unique()
{
    // Arrange
    var command = new UpdateCustomerCommand { PhoneNumber = 1234567890 };
    _dbContextMock.Setup(x => x.Customers.Any(c => c.PhoneNumber == command.PhoneNumber)).Returns(true);

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(c => c.PhoneNumber);
}
}