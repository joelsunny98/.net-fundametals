using MediatR;
using RetailStore.Dtos;
using RetailStore.Extensions;
using RetailStore.Model;
using RetailStore.Persistence;

namespace RetailStore.Features.OrderManagement.Commands;

/// <summary>
/// Command to Create a new Order
/// </summary>
public class CreateOrderCommand : IRequest<string>
{
    /// <summary>
    /// Gets and sets Data
    /// </summary>
    public OrderRequestDto Data { get; set; }
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
    /// Adds new order to the database
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Total Amount of Order</returns>
    public async Task<string> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var createdOrder = new Order
        {
            CustomerId = command.Data.CustomerId,
            TotalAmount = 0,
            Discount = 0,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
        };
        _dbContext.Orders.Add(createdOrder);

        var details = command.Data.Details.Select(d =>
        {
            var product = _dbContext.Products.Where(x => x.Id == d.ProductId).FirstOrDefault();
            var orderDetail = new OrderDetail
            {
                ProductId = d.ProductId,
                Quantity = d.Quantity,
                Order = createdOrder
            };

            if (product != null)
            {
                var Amount = createdOrder.TotalAmount.TotalValue(product.Price, d.Quantity);
                createdOrder.TotalAmount = Amount.DiscountedAmount;
                createdOrder.Discount = Amount.DiscountValue;
            }
            return orderDetail;
        }).ToList();
        _dbContext.OrderDetails.AddRange(details);

        await _dbContext.SaveChangesAsync();
        var result = createdOrder.TotalAmount.ConvertToCurrencyString();

        return result;
    }
}
