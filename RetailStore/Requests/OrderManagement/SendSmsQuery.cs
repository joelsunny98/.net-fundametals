using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Contracts;
using RetailStore.Extensions;
using RetailStore.Helpers;
using Twilio.Clients;
using Twilio.Types;

namespace RetailStore.Requests.OrderManagement;

/// <summary>
/// Represents a query to send an SMS with the total purchase amount to a customer.
/// </summary>
public class SendSmsQuery : IRequest<decimal>
{
    /// <summary>
    /// Gets or sets the customer ID.
    /// </summary>
    public long Id { get; set; }
}

/// <summary>
/// Represents the handler for the <see cref="SendSmsQuery"/> query.
/// </summary>
public class SendSmsQueryHandler : IRequestHandler<SendSmsQuery, decimal>
{
    private readonly IRetailStoreDbContext _retailStoreDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="SendSmsQueryHandler"/> class.
    /// </summary>
    /// <param name="retailStoreDbContext">The RetailStoreDbContext.</param>
    public SendSmsQueryHandler(IRetailStoreDbContext retailStoreDbContext)
    {
        _retailStoreDbContext = retailStoreDbContext;
    }

    /// <summary>
    /// Handles the <see cref="SendSmsQuery"/> query to send an SMS with the total purchase amount to a customer.
    /// </summary>
    /// <param name="request">The SendSmsQuery request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the SMS sending operation.</returns>
    public async Task<decimal> Handle(SendSmsQuery request, CancellationToken cancellationToken)
    {
        var accountSid = Constants.SmsServiceConstants.AccountSid;
        var authToken = Constants.SmsServiceConstants.AuthToken;

        var customer = await _retailStoreDbContext.Customers
            .FirstAsync(x => x.Id == request.Id, cancellationToken);

        var totalPurchase = await _retailStoreDbContext.Orders
            .Where(x => x.CustomerId == request.Id)
            .SumAsync(x => x.TotalAmount, cancellationToken);

        var twilioClient = new TwilioRestClient(accountSid, authToken);

        var twilioMessage = Twilio.Rest.Api.V2010.Account.MessageResource.Create(
            to: new PhoneNumber(TwilioHelper.FormatPhoneNumber(customer.PhoneNumber)),
            from: new PhoneNumber(Constants.SmsServiceConstants.FromPhoneNumber),
            body: "Hai " + customer.Name + " your total order amount is " + totalPurchase,
            client: twilioClient
        );

        return totalPurchase;
    }
}
