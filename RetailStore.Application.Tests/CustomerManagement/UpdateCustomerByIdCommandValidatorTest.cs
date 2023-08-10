using FluentValidation.TestHelper;
using MockQueryable.Moq;
using Moq;
using RetailStore.Contracts;
using RetailStore.Model;
using RetailStore.Requests.CustomerManagement;

namespace RetailStore.Tests.Requests.CustomerManagement
{
    public class UpdateCustomerCommandValidatorTests
    {
        private readonly Mock<IRetailStoreDbContext> _dbContextMock;
        private readonly UpdateCustomerCommandValidator _validator;

        public UpdateCustomerCommandValidatorTests()
        {
            _dbContextMock = new Mock<IRetailStoreDbContext>();
            _validator = new UpdateCustomerCommandValidator(_dbContextMock.Object);
        }

        [Theory]
        [InlineData(1, "John Doe", 1234567890)]
        public void DuplicatePhoneNumber_ShouldHaveValidationError(int customerId, string customerName, long phoneNumber)
        {
            // Arrange
            var command = new UpdateCustomerCommand
            {
                CustomerId = customerId,
                CustomerName = customerName,
                PhoneNumber = phoneNumber
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.PhoneNumber);
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
               Name = "Test",
               PhoneNumber = 1234567890,
            }
        }.AsQueryable().BuildMockDbSet().Object);
        }
        #endregion
    }
}
