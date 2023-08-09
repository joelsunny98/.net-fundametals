using Microsoft.Extensions.Configuration;
using Twilio.Clients;
using Twilio.Http;

namespace RetailStore.Services;

/// <summary>
/// Represents a custom Twilio REST client for RetailStore.
/// </summary>
public class TwilioClient : ITwilioRestClient
{
    private readonly ITwilioRestClient _innerClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="TwilioClient"/> class.
    /// </summary>
    /// <param name="config">The IConfiguration instance to access configuration settings.</param>
    /// <param name="httpClient">The HttpClient to be used by the inner TwilioRestClient.</param>
    public TwilioClient(IConfiguration config, System.Net.Http.HttpClient httpClient)
    {
        httpClient.DefaultRequestHeaders.Add("X-Custom-Header", "CustomTwilioRestClient-Demo");
        _innerClient = new TwilioRestClient(
            config["Twilio:AccountSid"],
            config["Twilio:AuthToken"],
            httpClient: new SystemNetHttpClient(httpClient));
    }

    /// <inheritdoc/>
    public Response Request(Request request) => _innerClient.Request(request);

    /// <inheritdoc/>
    public Task<Response> RequestAsync(Request request) => _innerClient.RequestAsync(request);

    /// <inheritdoc/>
    public string AccountSid => _innerClient.AccountSid;

    /// <inheritdoc/>
    public string Region => _innerClient.Region;

    /// <inheritdoc/>
    public Twilio.Http.HttpClient HttpClient => _innerClient.HttpClient;
}
