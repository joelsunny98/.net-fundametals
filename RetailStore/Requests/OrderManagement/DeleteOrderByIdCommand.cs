using MediatR;
using RetailStore.Constants;
using RetailStore.Model;
using RetailStore.Repository;

namespace RetailStore.Requests.OrderManagement;

/// <summary>
/// Command to Delete Order by Id
/// </summary>
public class DeleteOrderByIdCommand : IRequest<Order>
{
    /// <summary>
    /// Gets and sets Id
    /// </summary>
    public int Id { get; set; }
}

/// <summary>
/// Handler for Delete Order by Id command
/// </summary>
public class DeleteOrderByIdCommandHandler : IRequestHandler<DeleteOrderByIdCommand, Order>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly ILogger _logger;

    /// <summary>
    /// Injects IRpository class
    /// </summary>
    /// <param name="orderRepository"></param>
    public DeleteOrderByIdCommandHandler(IRepository<Order> orderRepository, ILogger<DeleteOrderByIdCommand> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    /// <summary>
    /// Deletes Order by Id
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Order</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<Order> Handle(DeleteOrderByIdCommand command, CancellationToken cancellationToken)
    {
        var deletedOrder = await _orderRepository.Delete(command.Id);

        if (deletedOrder == null)
        {
            _logger.LogError(LogMessage.SearchFail, command.Id);
            throw new KeyNotFoundException();
        }

        _logger.LogInformation(LogMessage.DeleteItem, command.Id);
        return deletedOrder;
    }
}
