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
            .WithMessage(ValidationMessage.Required)
            .MaximumLength(50).WithMessage(command => string.Format(ValidationMessage.CharExceedsFifty, command.ProductName));

        RuleFor(command => command.ProductName).MustAsync(IsUniqueProduct);

        RuleFor(command => command.ProductPrice).NotNull().NotEmpty()
            .WithMessage(ValidationMessage.Required)
            .GreaterThan(0).WithMessage(ValidationMessage.GreaterThanZero);
    }

    private async Task<bool> IsUniqueProduct(string productName, CancellationToken cancellationToken) 
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(e => e.Name == productName);

        return product != null;
    }
}
