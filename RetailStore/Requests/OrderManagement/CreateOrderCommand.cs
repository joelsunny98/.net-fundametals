using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;
using RetailStore.Contracts;
using RetailStore.Dtos;
using RetailStore.Helpers;
using RetailStore.Model;

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
    private readonly IRetailStoreDbContext _dbContext;
    private readonly ILogger _logger;

    /// <summary>
    /// Injects RetailStoreDbContext class
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public CreateOrderCommandHandler(IRetailStoreDbContext dbContext, ILogger<CreateOrderCommand> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
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

        var productIds = command.Details.Select(e => e.ProductId).ToList();
        var products = await _dbContext.Products.Where(e => productIds.Contains(e.Id)).ToListAsync(cancellationToken);

        var details = command.Details.Select(d =>
        {
            var product = products.FirstOrDefault(e => e.Id == d.ProductId);
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

        await _dbContext.SaveChangesAsync(cancellationToken);
        var result = createdOrder.TotalAmount.ConvertToCurrencyString();

        _logger.LogInformation(LogMessage.NewItem, createdOrder.Id);

        return result;
    }
}
