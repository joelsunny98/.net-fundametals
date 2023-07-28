namespace RetailStore.Helpers
{
    /// <summary>
    /// Helper class for Twilio related operations.
    /// </summary>
    public static class TwilioHelper
    {
        /// <summary>
        /// Formats a phone number with the "+91" prefix for use in Twilio messages.
        /// </summary>
        /// <param name="phoneNumber">The phone number to be formatted.</param>
        /// <returns>The formatted phone number.</returns>
        public static string FormatPhoneNumber(long? phoneNumber)
        {
            if (phoneNumber.HasValue)
            {
                string phoneNum = "+91" + phoneNumber.Value.ToString();
                return phoneNum;
            }

            return string.Empty;
        }
    }
}
