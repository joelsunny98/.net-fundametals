using NanoidDotNet;
using RetailStore.Constants;
using RetailStore.Contracts;


namespace RetailStore.Services;

/// <summary>
/// Service to generate Premium codes for premium customers.
/// </summary>
public class PremiumCodeService : IPremiumCodeService
{
    private static readonly HashSet<string> _generatedCodes = new HashSet<string>();
    private static readonly object _lockObject = new object();

    /// <summary>
    /// Method to generate a unique PremiumCode for premium customers.
    /// </summary>
    /// <returns>A string representing the generated premium code.</returns>
    public string GeneratePremiumCode()
    {
        string premiumCode;
        lock (_lockObject)
        {
            do
            {
                premiumCode = Nanoid.Generate(PremiumCustomer.AllowedChars, PremiumCustomer.Size);
            } while (_generatedCodes.Contains(premiumCode));

            _generatedCodes.Add(premiumCode);
        }
        return premiumCode;
    }
}
