using MediatR;
using RetailStore.Dtos;
using RetailStore.Helpers;
using RetailStore.Model;
using RetailStore.Persistence;

namespace RetailStore.Requests.OrderManagement;

/// <summary>
/// Command to Create a new Order
/// </summary>
public class CreateOrderCommand : OrderRequestDto, IRequest<string>
{
}

/// <summary>
/// Command Handler for Create Order Command
/// </summary>
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, string>
{
    private readonly RetailStoreDbContext _dbContext;

    /// <summary>
    /// Injects RetailStoreDbContext class
    /// </summary>
    /// <param name="dbContext"></param>
    public CreateOrderCommandHandler(RetailStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Adds a new order to the database
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Total Amount of Order</returns>
    public async Task<string> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var createdOrder = new Order
        {
            CustomerId = command.CustomerId,
            TotalAmount = 0,
            Discount = 0,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
        };
        _dbContext.Orders.Add(createdOrder);

        var details = command.Details.Select(d =>
        {
            var product = _dbContext.Products.FirstOrDefault(x => x.Id == d.ProductId);
            var orderDetail = new OrderDetail
            {
                ProductId = d.ProductId,
                Quantity = d.Quantity,
                Order = createdOrder
            };

            if (product != null)
            {
                var amountDto = AmountHelper.CalculateTotalValue(product.Price, d.Quantity);
                createdOrder.TotalAmount = amountDto.DiscountedAmount;
                createdOrder.Discount = amountDto.DiscountValue;
            }

            return orderDetail;
        }).ToList();

        _dbContext.OrderDetails.AddRange(details);

        await _dbContext.SaveChangesAsync();
        var result = createdOrder.TotalAmount.ConvertToCurrencyString();

        return result;
    }
}
