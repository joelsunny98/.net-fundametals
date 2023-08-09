using FluentValidation.TestHelper;
using Moq;
using RetailStore.Contracts;
using RetailStore.Requests.ProductManagement;

namespace RetailStore.Tests.ProductManagement;

public class AddProductCommandValidatorTests
{
    private readonly Mock<IRetailStoreDbContext> _mockDbContext;
    private readonly AddProductCommandValidator _validator;

    public AddProductCommandValidatorTests()
    {
        _mockDbContext = new Mock<IRetailStoreDbContext>();
        _validator = new AddProductCommandValidator(_mockDbContext.Object);
    }

    [Fact]
    public void Validate_ValidData_ShouldNotHaveErrors()
    {
        // Arrange
        var command = new AddProductCommand
        {
            ProductName = "Test Product",
            ProductPrice = 100
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ProductName);
        result.ShouldNotHaveValidationErrorFor(x => x.ProductPrice);
    }

    [Fact]
    public void ProductName_NullOrEmpty_ShouldFailValidation()
    {
        // Arrange
        var command = new AddProductCommand
        {
            ProductName = "",
            ProductPrice = 100
        };

        // Act
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ProductName).WithErrorMessage("Product Name is required");
    }

    [Fact]
    public void ProductPrice_NullOrEmpty_ShouldFailValidation()
    {
        // Arrange
        var command = new AddProductCommand
        {
            ProductName = "Product",
            ProductPrice = default(decimal)
        };

        // Act
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ProductPrice).WithErrorMessage("Price is required");
    }

    [Fact]
    public void ProductPrice_LessThanOne_ShouldFailValidation()
    {
        // Arrange
        var command = new AddProductCommand
        {
            ProductName = "Product",
            ProductPrice = 0
        };

        // Act
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ProductPrice).WithErrorMessage("Price must be greater than 0");
    }
}