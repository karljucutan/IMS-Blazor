using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IInventoryTransactionRepository
    {
        Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo, InventoryTransactionType? activityType);
        Task ProduceProductAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy, int price);
        Task PurchaseInventoryAsync(string purchaseNumber, Inventory inventory, int quantity, string doneBy, double price);
    }
}