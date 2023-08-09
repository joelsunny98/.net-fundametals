

namespace RetailStore.Contracts;

/// <summary>
/// Interface for generating premium codes for premium customers.
/// </summary>
public interface IPremiumCodeService
{
    /// <summary>
    /// Generates a unique premium code for premium customers.
    /// </summary>
    /// <returns>A string representing the premium code.</returns>
    string GeneratePremiumCode();
}
