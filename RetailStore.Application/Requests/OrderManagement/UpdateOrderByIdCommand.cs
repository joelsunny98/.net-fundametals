using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;
using RetailStore.Contracts;
using RetailStore.Dtos;
using RetailStore.Helpers;
using RetailStore.Model;
using Microsoft.Extensions.Logging;

namespace RetailStore.Requests.OrderManagement;

/// <summary>
/// Update Command for Order
/// </summary>
public class UpdateOrderByIdCommand : IRequest<int>
{
    /// <summary>
    /// Gets and sets Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets and sets OrderRequest
    /// </summary>
    public OrderRequestDto OrderRequest { get; set; }
}

/// <summary>
/// Handler for Update Order Command
/// </summary>
public class UpdateOrderByIdCommandHandler : IRequestHandler<UpdateOrderByIdCommand, int>
{
    private readonly IRetailStoreDbContext _dbContext;

    /// <summary>
    /// Injects RetailDbContext Class
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public UpdateOrderByIdCommandHandler(IRetailStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Updates Order by Id
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Order id</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<int> Handle(UpdateOrderByIdCommand command, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders.FirstOrDefaultAsync(e => e.Id == command.Id);

     
        order.CustomerId = command.OrderRequest.CustomerId;
        order.UpdatedOn = DateTime.UtcNow;

        var productIds = command.OrderRequest.Details.Select(e => e.ProductId).ToList();
        var products = await _dbContext.Products.Where(e => productIds.Contains(e.Id)).ToListAsync(cancellationToken);

        var details = command.OrderRequest.Details.Select(d =>
        {
            var product = products.FirstOrDefault(e => e.Id == d.ProductId);

            var orderDetail = new OrderDetail
            {
                ProductId = d.ProductId,
                Quantity = d.Quantity,
                Order = order
            };

            if (product != null)
            {
                var amountDto = AmountHelper.CalculateTotalValue(product.Price, d.Quantity);
                order.TotalAmount = amountDto.DiscountedAmount;
                order.Discount = amountDto.DiscountValue;
            }

            return orderDetail;
        }).ToList();

        _dbContext.OrderDetails.AddRange(details);
        await _dbContext.SaveChangesAsync();

        return order.Id;
    }
}
