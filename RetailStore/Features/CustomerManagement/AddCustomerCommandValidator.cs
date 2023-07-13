using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RetailStore.Dtos;
using RetailStore.Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RetailStore.Features.CustomerManagement;
public class AddCustomerCommandValidator : AbstractValidator<AddCustomerCommand>
{
    private readonly RetailStoreDbContext _dbContext;

    public AddCustomerCommandValidator(RetailStoreDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(command => command.Data.CustomerName)
            .NotEmpty().WithMessage("Customer name is required.");

        RuleFor(command => command.Data.PhoneNumber)
            //.NotEmpty().WithMessage("Phone number is required.")
            .Must(BeValidPhoneNumber).WithMessage("Phone number must be 10 digits.")
            .MustAsync(BeUniquePhoneNumber).WithMessage("Phone number must be unique.");
    }

    private bool BeValidPhoneNumber(long phoneNumber)
    {
        string phoneNumberString = phoneNumber.ToString();
        return phoneNumberString.Length == 10;
    }

    private async Task<bool> BeUniquePhoneNumber(AddCustomerCommand command, long phoneNumber, CancellationToken cancellationToken)
    {
        var duplicateCustomer = await _dbContext.Customers.AnyAsync(x => x.PhoneNumber == command.Data.PhoneNumber, cancellationToken);
        return !duplicateCustomer;
    }
}
