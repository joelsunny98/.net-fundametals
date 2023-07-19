namespace RetailStore.Constants;

public class LogMessage
{
    public const string NewItem = "New item added with id {itemId}";

    public const string GetAllItems = "Retreived {itemCount} items";

    public const string GetItemById = "Retreived one item with id {itemId}";

    public const string DeleteItem = "Deleted item with id {itemId}";

    public const string UpdatedItem = "Updated item with Id {itemId}";

    public const string SearchFail = "No such item found with Id {ItemId}";

    public const string FailedToGetAllItems = "Failed to retrieve all items";

    public const string DayCollection = "Retreived Total Collection for the day";

    public const string OrderForTheDay = "Retrieved {OrderCount} for the day";

    public const string GenerateBarCode = "Generated a barcode for Product with Id: {ProductId}";
}
