using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;
using RetailStore.Contracts;

namespace RetailStore.Requests.ProductManagement;

/// <summary>
/// Validator for Add product command
/// </summary>
public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
    private readonly IRetailStoreDbContext _dbContext;

    /// <summary>
    /// Validation rules for specific properties
    /// </summary>
    public AddProductCommandValidator(IRetailStoreDbContext dbcontext)
    {
        _dbContext = dbcontext;

        RuleFor(command => command.ProductName).NotNull().NotEmpty()
            .WithMessage(ValidationMessage.ProductNameRequired)
            .MaximumLength(50).WithMessage(ValidationMessage.ProductNameLength);

        RuleFor(command => command.ProductName).Must(IsUniqueProduct).WithMessage(ValidationMessage.ProductNameUnique);

        RuleFor(command => command.ProductPrice).NotNull().NotEmpty()
            .WithMessage(ValidationMessage.PriceRequired)
            .GreaterThan(0).WithMessage(ValidationMessage.PriceGreaterThanZero);
    }

    private bool IsUniqueProduct(string productName)
    {
        var product = _dbContext.Products.Any(e => e.Name == productName);
        return !product;
    }
}
