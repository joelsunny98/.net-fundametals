namespace RetailStore.Constants;

/// <summary>
/// Constants for Validation Message
/// </summary>
public class ValidationMessage
{
    /// <summary>
    /// Is required
    /// </summary>
    public const string Required = "{0} is required";

    /// <summary>
    /// Is Unique
    /// </summary>
    public const string Unique = "{0} is already exists";

    /// <summary>
    /// Length Limit
    /// </summary>
    public const string Length = "{0} exceeds its limit";

    /// <summary>
    /// PHone number must have 10 digits
    /// </summary>
    public const string PhoneNumberLength = "{0} should be 10 digits";

    /// <summary>
    /// Is Invalid
    /// </summary>
    public const string Invalid = "{0} is invalid";

    /// <summary>
    /// Exceeds 50 characters
    /// </summary>
    public const string CharExceedsFifty = "{0} exceeds 50 characters";

    /// <summary>
    /// Amount must be greater that 0
    /// </summary>
    public const string GreaterThanZero = "Amount must be greater than 0";
}
