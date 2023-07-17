using FluentValidation;

namespace RetailStore.Requests.ProductManagement
{
    /// <summary>
    /// Validator for Add product command
    /// </summary>
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        /// <summary>
        /// Validation rules for specific properties
        /// </summary>
        public AddProductCommandValidator()
        {
            RuleFor(command => command.Data.ProductName).NotNull().NotEmpty()
                .WithMessage("Product name is required.");

            RuleFor(command => command.Data.ProductPrice).NotNull().NotEmpty()
                .WithMessage("Product price is required.");
        }
    }
}
