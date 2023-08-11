using RetailStore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailStore.Application.Tests.Helpers;

public class AmountHelperTests
{
    [Theory]
    [InlineData(100, 5, 0, 500)] // No discount
    [InlineData(190, 10, 190, 1710)] // 10% discount
    [InlineData(290, 10, 145, 2755)] // 20% discount
    [InlineData(390, 10, 130, 3770)] // 30% discount
    public void CalculateTotalValue_ShouldCalculateCorrectly(decimal price, int quantity, decimal expectedDiscountValue, decimal expectedDiscountedAmount)
    {
        // Act
        var amountDto = AmountHelper.CalculateTotalValue(price, quantity);

        // Assert
        Assert.Equal(expectedDiscountValue, amountDto.DiscountValue);
        Assert.Equal(expectedDiscountedAmount, amountDto.DiscountedAmount);
    }
}
