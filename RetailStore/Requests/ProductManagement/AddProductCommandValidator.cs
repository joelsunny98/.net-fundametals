using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;
using RetailStore.Persistence;

namespace RetailStore.Requests.ProductManagement;

/// <summary>
/// Validator for Add product command
/// </summary>
public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
    private readonly RetailStoreDbContext _dbContext;

    /// <summary>
    /// Validation rules for specific properties
    /// </summary>
    public AddProductCommandValidator(RetailStoreDbContext dbcontext)
    {
        _dbContext = dbcontext;

        RuleFor(command => command.ProductName).NotNull().NotEmpty()
            .WithMessage(command => string.Format(ValidationMessage.Required, "Product Name"))
            .MaximumLength(50).WithMessage(command => string.Format(ValidationMessage.CharExceedsFifty, command.ProductName));

        RuleFor(command => command.ProductName).Must(IsUniqueProduct).WithMessage(command => string.Format(ValidationMessage.Unique, command.ProductName));

        RuleFor(command => command.ProductPrice).NotNull().NotEmpty()
            .WithMessage(command => string.Format(ValidationMessage.Required, "Price"))
            .GreaterThan(0).WithMessage(ValidationMessage.GreaterThanZero);
    }

    private bool IsUniqueProduct(string productName)
    {
        var product = _dbContext.Products.Any(e => e.Name == productName);  
        return !product;
    }
}
