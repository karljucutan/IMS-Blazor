using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System.Linq;

namespace IMS.Plugins.InMemory
{
    public class InventoryTransactionRepository : IInventoryTransactionRepository
    {
        private readonly IInventoryRepository _inventoryRepository;
        public List<InventoryTransaction> _inventoryTransactions = new();

        public InventoryTransactionRepository(IInventoryRepository inventoryRepository)
        {
            // TODO: injecting another repository in this repository is a violation of SRP
            _inventoryRepository = inventoryRepository;
        }

        public async Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo, InventoryTransactionType? activityType)
        {
            var inventories = (await _inventoryRepository.GetInventoriesByNameAsync(string.Empty)).ToList();

            //var inventTransc = _inventoryTransactions.FirstOrDefault();
            //var invent = inventories.FirstOrDefault(x => x.InventoryId == inventTransc?.InventoryId);
            //var test1 = (string.IsNullOrWhiteSpace(inventoryName) || (invent is not null && invent.InventoryName.Contains(inventoryName)));
            //var test2 = (!dateFrom.HasValue || (inventTransc.TransactionDate >= dateFrom.Value.Date && inventTransc.TransactionDate <= dateTo.Value));
            //var test3 = (!activityType.HasValue || inventTransc.ActivityType == activityType);

            var query = from it in _inventoryTransactions
                        join inv in inventories
                        on it.InventoryId equals inv.InventoryId
                        where
                            (string.IsNullOrWhiteSpace(inventoryName) || inv.InventoryName.Contains(inventoryName)) &&
                            (!dateFrom.HasValue || it.TransactionDate >= dateFrom.Value.Date) &&
                            (!dateTo.HasValue || it.TransactionDate <= dateTo.Value.Date) &&
                            (!activityType.HasValue || it.ActivityType == activityType)
                        select new InventoryTransaction
                        {
                            Inventory = inv,
                            InventoryTransactionId = it.InventoryTransactionId,
                            PurchaseOrderNumber = it.PurchaseOrderNumber,
                            InventoryId = it.InventoryId,
                            QuantityBefore = it.QuantityBefore,
                            ActivityType = it.ActivityType,
                            QuantityAfter = it.QuantityAfter,
                            TransactionDate = it.TransactionDate,
                            DoneBy = it.DoneBy,
                            UnitPrice = it.UnitPrice,
                        };

            return query;
        }

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

