using Twilio.Types;

namespace RetailStore.Helpers;

public static class TwilioHelper
{
    public static string FormatPhoneNumber(long? phoneNumber)
    {
        string phoneNum = "+91" + phoneNumber.ToString();
        return phoneNum;
    }
}
