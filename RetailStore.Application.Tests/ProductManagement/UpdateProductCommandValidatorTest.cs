using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.ProductManagement;

namespace RetailStore.Tests.ProductManagement
{
    public class UpdateProductCommandValidatorTest
    {
        private readonly Mock<IRetailStoreDbContext> _mockDbContext;

        public UpdateProductCommandValidatorTest()
        {
            _mockDbContext = new Mock<IRetailStoreDbContext>();
        }

        [Fact]
        public void Validate_ValidCommand_ShouldNotHaveAnyErrors()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "ExistingProduct", Price = 50 }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Product>>();
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

            _mockDbContext.Setup(x => x.Products).Returns(mockDbSet.Object);

            var validator = new UpdateProductCommandValidator(_mockDbContext.Object);
            var validCommand = new UpdateProductCommand
            {
                Id = 1,
                ProductName = "UpdatedProduct",
                ProductPrice = 75
            };

            // Act
            var result = validator.TestValidate(validCommand);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
            result.ShouldNotHaveValidationErrorFor(x => x.ProductName);
            result.ShouldNotHaveValidationErrorFor(x => x.ProductPrice);
        }

        [Fact]
        public void Validate_ProductNameMaxLengthExceeded_ShouldHaveError()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "ExistingProduct", Price = 50 }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Product>>();
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

            _mockDbContext.Setup(x => x.Products).Returns(mockDbSet.Object);

            var validator = new UpdateProductCommandValidator(_mockDbContext.Object);
            var invalidCommand = new UpdateProductCommand
            {
                Id = 1,
                ProductName = "A very long product name that exceeds the maximum length allowed",
                ProductPrice = 75
            };

            // Act
            var result = validator.TestValidate(invalidCommand);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ProductName);
        }

    }
}