using FluentValidation;

namespace RetailStore.Requests.ProductManagement
{
    /// <summary>
    /// Valdiator for Update Product Command
    /// </summary>
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        /// <summary>
        /// Validator rules for Specific Rules
        /// </summary>
        public UpdateProductCommandValidator()
        {
            RuleFor(command => command.Id)
                .GreaterThan(0).WithMessage("Invalid product ID.");

            RuleFor(command => command.ProductName)
                .NotNull().NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(50).WithMessage("Product name cannot exceed 50 characters.");

            RuleFor(command => command.ProductPrice)
                .GreaterThan(0).WithMessage("Product price must be greater than 0.");
        }
    }
}
