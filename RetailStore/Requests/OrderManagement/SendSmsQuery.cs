using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Extensions;
using RetailStore.Persistence;
using Twilio.Clients;
using Twilio.Types;

namespace RetailStore.Requests.OrderManagement
{
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
        private readonly RetailStoreDbContext _retailStoreDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendSmsQueryHandler"/> class.
        /// </summary>
        /// <param name="retailStoreDbContext">The RetailStoreDbContext.</param>
        public SendSmsQueryHandler(RetailStoreDbContext retailStoreDbContext)
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
                to: new PhoneNumber(customer.PhoneNumber.ConvertPhoneNumToString()),
                from: new PhoneNumber(Constants.SmsServiceConstants.FromPhoneNumber),
                body: "Hai " + customer.Name + " your total order amount is " + totalPurchase,
                client: twilioClient
            );

            return totalPurchase;
        }
    }
}
