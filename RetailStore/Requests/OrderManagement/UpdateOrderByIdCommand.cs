﻿using MediatR;
using RetailStore.Dtos;
using RetailStore.Model;
using RetailStore.Persistence;
using RetailStore.Helpers;

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
    private readonly RetailStoreDbContext _dbContext;
    private readonly ILogger _logger;

    /// <summary>
    /// Injects RetailDbContext Class
    /// </summary>
    /// <param name="dbContext"></param>
    public UpdateOrderByIdCommandHandler(RetailStoreDbContext dbContext, ILogger<UpdateOrderByIdCommand> logger)
    {
        _dbContext = dbContext;
        _logger = logger;

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
        var order = _dbContext.Orders.FirstOrDefault(e => e.Id == command.Id);

        if (order == null)
        {
            _logger.LogError("No order with Id: {OrderId}", command.Id);
            throw new KeyNotFoundException();
        }

        order.CustomerId = command.OrderRequest.CustomerId;
        order.UpdatedOn = DateTime.UtcNow;

        var details = command.OrderRequest.Details.Select(d =>
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == d.ProductId);
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

        _logger.LogInformation("Updated Order with Id: {OrderId", command.Id);
        return order.Id;
    }
}
