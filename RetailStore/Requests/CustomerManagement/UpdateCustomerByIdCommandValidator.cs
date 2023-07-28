using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;
using RetailStore.Contracts;

namespace RetailStore.Requests.CustomerManagement;

/// <summary>
/// Validator for Update Customer Command
/// </summary>
public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    private readonly IRetailStoreDbContext _dbContext;

    /// <summary>
    /// Validator for defining specific rules for properties
    /// </summary>
    public UpdateCustomerCommandValidator(IRetailStoreDbContext dbContext)
    {
        _dbContext = dbContext;
        RuleFor(command => command.CustomerId)
            .GreaterThan(0).WithMessage(ValidationMessage.CustomerIDGreaterThanZero);

        RuleFor(command => command.CustomerName)
            .NotNull().NotEmpty().WithMessage(ValidationMessage.CustomerNameRequired)
            .MaximumLength(25).WithMessage(ValidationMessage.CustomerNameLength);

        RuleFor(command => command.PhoneNumber)
              .NotNull().NotEmpty().WithMessage(ValidationMessage.PhoneNumberRequired)
              .Must(BeValidPhoneNumber).WithMessage(ValidationMessage.PhoneNumberValid)
              .Must(BeUniquePhoneNumber).WithMessage(ValidationMessage.PhoneNumverUnique);
    }

    private bool BeValidPhoneNumber(long phoneNumber)
    {
        var phoneNumberString = phoneNumber.ToString();
        return phoneNumberString.Length == 10;
    }

    private bool BeUniquePhoneNumber(long phoneNumber)
    {
        var exists = _dbContext.Customers.Any(c => c.PhoneNumber == phoneNumber);
        return !exists;
    }
}
