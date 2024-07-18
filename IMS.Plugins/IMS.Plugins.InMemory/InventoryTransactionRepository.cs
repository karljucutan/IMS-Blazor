using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class InventoryTransactionRepository : IInventoryTransactionRepository
    {
        public List<InventoryTransaction> _inventoryTransactions = new();

        // TODO: change to AddProduceProductAsync
        public Task ProduceProductAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy, int price)
        {
            _inventoryTransactions.Add(
                new InventoryTransaction
                {
                    ProductionNumber = productionNumber,
                    InventoryId = inventory.InventoryId,
                    QuantityBefore = inventory.Quantity,
                    ActivityType = InventoryTransactionType.ProduceProduct,
                    QuantityAfter = inventory.Quantity - quantityToConsume,
                    TransactionDate = DateTime.UtcNow,
                    DoneBy = doneBy,
                    UnitPrice = price
                });

            return Task.CompletedTask;
        }

        // TODO: change to AddProduceProductAsync
        public Task PurchaseInventoryAsync(string purchaseNumber, Inventory inventory, int quantity, string doneBy, double price)
        {
            _inventoryTransactions.Add(
                new InventoryTransaction
                {
                    PurchaseOrderNumber = purchaseNumber,
                    InventoryId = inventory.InventoryId,
                    QuantityBefore = inventory.Quantity,
                    ActivityType = InventoryTransactionType.PurchaseInventory,
                    QuantityAfter = inventory.Quantity + quantity,
                    TransactionDate = DateTime.UtcNow,
                    DoneBy = doneBy,
                    UnitPrice = price
                });

            return Task.CompletedTask;
        }
    }
}

