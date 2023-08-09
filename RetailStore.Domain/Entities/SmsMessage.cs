namespace RetailStore.Model;

/// <summary>
/// Represents an SMS message.
/// </summary>
public class SmsMessage
{
    /// <summary>
    /// Gets or sets the recipient phone number.
    /// </summary>
    public string? To { get; set; }

    /// <summary>
    /// Gets or sets the sender phone number.
    /// </summary>
    public string? From { get; set; }

    /// <summary>
    /// Gets or sets the message content.
    /// </summary>
    public string? Message { get; set; }
}
