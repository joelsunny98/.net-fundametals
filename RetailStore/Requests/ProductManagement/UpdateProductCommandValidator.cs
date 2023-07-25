using FluentValidation;
using RetailStore.Constants;
using RetailStore.Contracts;

namespace RetailStore.Requests.ProductManagement
{
    /// <summary>
    /// Valdiator for Update Product Command
    /// </summary>
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        private readonly IRetailStoreDbContext _dbContext;

        /// <summary>
        /// Validator rules for Specific Rules
        /// </summary>
        public UpdateProductCommandValidator(IRetailStoreDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(command => command.Id)
                .GreaterThan(0).WithMessage(ValidationMessage.ProductIdGreaterThanZero);

            RuleFor(command => command.ProductName)
                .NotNull().NotEmpty().WithMessage(ValidationMessage.ProductNameRequired)
                .MaximumLength(50).WithMessage(ValidationMessage.ProductNameLength)
                .Must(IsUniqueProduct).WithMessage(ValidationMessage.ProductNameUnique);

            RuleFor(command => command.ProductPrice)
                .NotEqual(0).WithMessage(ValidationMessage.PriceGreaterThanZero);
        }

        private bool IsUniqueProduct(string productName)
        {
            var product = _dbContext.Products.Any(e => e.Name == productName);
            return !product;
        }
    }
}
