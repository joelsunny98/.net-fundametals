namespace RetailStore.Constants;

/// <summary>
/// Constansts for Log Message
/// </summary>
public class LogMessage
{
    /// <summary>
    /// "New item added with id {itemId}"
    /// </summary>
    public const string NewItem = "New item added with id {itemId}";

    /// <summary>
    /// "Retreived {itemCount} items";
    /// </summary>
    public const string GetAllItems = "Retreived {itemCount} items";

    /// <summary>
    /// "Retreived one item with id {itemId}"
    /// </summary>
    public const string GetItemById = "Retreived one item with id {itemId}";

    /// <summary>
    /// "Deleted item with id {itemId}"
    /// </summary>
    public const string DeleteItem = "Deleted item with id {itemId}";

    /// <summary>
    /// "Updated item with Id {itemId}"
    /// </summary>
    public const string UpdatedItem = "Updated item with Id {itemId}";

    /// <summary>
    /// "No such item found with Id {ItemId}"
    /// </summary>
    public const string SearchFail = "No such item found with Id {ItemId}";

    /// <summary>
    /// "Failed to retrieve all items"
    /// </summary>
    public const string FailedToGetAllItems = "Failed to retrieve all items";

    /// <summary>
    /// "Retreived Total Collection for the day"
    /// </summary>
    public const string DayCollection = "Retreived Total Collection for the day";

    /// <summary>
    /// "Retrieved {OrderCount} for the day"
    /// </summary>
    public const string OrderForTheDay = "Retrieved {OrderCount} for the day";

    /// <summary>
    /// "Generated a barcode for Product with Id: {ProductId}"
    /// </summary>
    public const string GenerateBarCode = "Generated a barcode for Product with Id: {ProductId}";

    /// <summary>
    /// "Barcode for Product {ProductId} generated"
    /// </summary>
    public const string BarcodeGenerated = "Barcode for Product {ProductId} generated";

    /// <summary>
    /// "Failed to retrieve premium customer"
    /// </summary>
    public const string GetPremiumCustomersFailed = "Failed to retrieve premium customer";

    /// <summary>
    /// "Generated Premium Customers"
    /// </summary>
    public const string GeneratePremiumCustomer = "Generated Premium Customers";
}
