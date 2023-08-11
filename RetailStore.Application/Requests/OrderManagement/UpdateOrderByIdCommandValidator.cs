﻿using FluentValidation;
using RetailStore.Constants;
using RetailStore.Contracts;

namespace RetailStore.Requests.OrderManagement;

/// <summary>
/// Validator for Update Order Command
/// </summary>
public class UpdateOrderByIdCommandValidator : AbstractValidator<UpdateOrderByIdCommand>
{
    /// <summary>
    /// Validator for defining specific rules for properties
    /// </summary>
    public UpdateOrderByIdCommandValidator(IRetailStoreDbContext dbContext)
    {
        //Rules for required fields
        RuleFor(x => x.OrderRequest.CustomerId).NotNull().NotEmpty().WithMessage(ValidationMessage.CustomerIdRequried);
        RuleForEach(x => x.OrderRequest.Details).ChildRules(p =>
        {
            p.RuleFor(x => x.ProductId).NotNull().NotEmpty().WithMessage(ValidationMessage.ProductIdRequired);
            p.RuleFor(x => x.Quantity).NotNull().NotEmpty().WithMessage(ValidationMessage.QuantityRequired);
        });
    }
}
