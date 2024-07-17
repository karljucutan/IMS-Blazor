using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IInventoryTransactionRepository
    {
        Task ProduceProductAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy, int price);
        Task PurchaseInventoryAsync(string purchaseNumber, Inventory inventory, int quantity, string doneBy, double price);
    }
}