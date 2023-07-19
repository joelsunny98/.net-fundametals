using FluentValidation;
using RetailStore.Constants;


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
                .GreaterThan(0).WithMessage(ValidationMessage.Invalid);

            RuleFor(command => command.ProductName)
                .NotNull().NotEmpty().WithMessage(ValidationMessage.Required)
                .MaximumLength(50).WithMessage(ValidationMessage.Length) ;

            RuleFor(command => command.ProductPrice)
                .NotEqual(0).WithMessage(ValidationMessage.GreaterThanZero);
        }
    }
}
