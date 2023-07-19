﻿namespace RetailStore.Constants;

public class ValidationMessage
{
    public const string Required = "{PropertyName} is required";

    public const string UniquePhoneNumber = "{PropertyName} is already exists";

    public const string Length = "{PropertyName} is exceeds its limit";

    public const string PhoneNumberLength = "{PropertyName} should be 10 digits";

    public const string Invalid = "{PropertyName} is invalid";
}
