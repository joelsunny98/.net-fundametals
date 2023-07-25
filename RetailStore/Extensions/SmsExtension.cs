using System.Globalization;
using System.Numerics;
using Twilio.Types;

namespace RetailStore.Extensions;

public static class SmsExtension
{

    public static string ConvertPhoneNumToString(this long? PhoneNumber)
    {
        string phoneNum = "+91" + PhoneNumber.ToString();
        return phoneNum;
    }
}
