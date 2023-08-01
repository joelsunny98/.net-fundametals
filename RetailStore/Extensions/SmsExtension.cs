using RetailStore.Constants;

namespace RetailStore.Extensions;

/// <summary>
/// Extension methods for SMS related operations.
/// </summary>
public static class SmsExtension
{
    /// <summary>
    /// Converts a phone number to a string format with the "+91" prefix.
    /// </summary>
    /// <param name="phoneNumber">The phone number to be converted.</param>
    /// <returns>The formatted phone number string.</returns>
    public static string ConvertPhoneNumToString(this long? phoneNumber)
    {
        if (phoneNumber.HasValue)
        {
            string phoneNum = Constant.IndiaPhoneCode + phoneNumber.Value.ToString();
            return phoneNum;
        }
        return string.Empty;
    }
}
