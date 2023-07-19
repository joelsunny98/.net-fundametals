using FluentValidation;
using RetailStore.Constants;
using RetailStore.Persistence;

namespace RetailStore.Requests.ProductManagement
{
    /// <summary>
    /// Valdiator for Update Product Command
    /// </summary>
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        private readonly RetailStoreDbContext _dbContext;

        /// <summary>
        /// Validator rules for Specific Rules
        /// </summary>
        public UpdateProductCommandValidator(RetailStoreDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(command => command.Id)
                .GreaterThan(0).WithMessage(ValidationMessage.Invalid);

            RuleFor(command => command.ProductName)
                .NotNull().NotEmpty().WithMessage(command => string.Format(ValidationMessage.Required, "Product Name"))
                .MaximumLength(50).WithMessage(command => string.Format(ValidationMessage.Length, command.ProductName))
                .Must(IsUniqueProduct).WithMessage(command => string.Format(ValidationMessage.Unique, command.ProductName));

            RuleFor(command => command.ProductPrice)
                .NotEqual(0).WithMessage(ValidationMessage.GreaterThanZero);
        }

        private bool IsUniqueProduct(string productName)
        {
            var product = _dbContext.Products.Any(e => e.Name == productName);
            return !product;
        }
    }
}
