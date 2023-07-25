using FluentValidation;
using RetailStore.Constants;
using RetailStore.Contracts;

namespace RetailStore.Requests.CustomerManagement;

/// <summary>
/// Validator for Add Customer Command
/// </summary>
public class AddCustomerCommandValidator : AbstractValidator<AddCustomerCommand>
{
    private readonly IRetailStoreDbContext _dbContext;

    /// <summary>
    /// Validator for defining specific rules for properties
    /// </summary>
    public AddCustomerCommandValidator(IRetailStoreDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(command => command.CustomerName)
            .NotNull().NotEmpty().WithMessage(ValidationMessage.CustomerNameRequired)
            .MaximumLength(25).WithMessage(ValidationMessage.CustomerNameLength);

        RuleFor(command => command.PhoneNumber)
            .NotNull().NotEmpty().WithMessage(ValidationMessage.PhoneNumberRequired)
            .Must(BeValidPhoneNumber).WithMessage(ValidationMessage.PhoneNumberValid)
            .Must(BeUniquePhoneNumber).WithMessage(ValidationMessage.PhoneNumverUnique);
    }

    /// <summary>
    /// Method to check valid Phonenumber
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    private bool BeValidPhoneNumber(long phoneNumber)
    {
        var phoneNumberString = phoneNumber.ToString();
        return phoneNumberString.Length == 10;
    }

    /// <summary>
    /// Method to check unique Phonenumber
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    private bool BeUniquePhoneNumber(long phoneNumber)
    {
        var exists = _dbContext.Customers.Any(c => c.PhoneNumber == phoneNumber);
        return !exists;
    }
}
