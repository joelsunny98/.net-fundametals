﻿using FluentValidation;

namespace RetailStore.Requests.ProductManagement
{
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductCommandValidator()
        {
            RuleFor(command => command.Data.ProductName).NotNull().NotEmpty()
                .WithMessage("Product name is required.");

            RuleFor(command => command.Data.ProductPrice).NotNull().NotEmpty()
                .WithMessage("Product price is required.");
        }
    }
}
