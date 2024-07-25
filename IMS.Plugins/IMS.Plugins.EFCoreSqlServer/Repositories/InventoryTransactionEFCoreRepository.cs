using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer.Repositories
{
    public class InventoryTransactionEFCoreRepository : IInventoryTransactionRepository
    {
        private readonly IDbContextFactory<IMSDbContext> _dbContextFactory;

        public InventoryTransactionEFCoreRepository(IDbContextFactory<IMSDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo, InventoryTransactionType? activityType)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();

            var query = (from it in db.InventoryTransactions
                        join inv in db.Inventories
                        on it.InventoryId equals inv.InventoryId
                        where
                            (string.IsNullOrWhiteSpace(inventoryName) || inv.InventoryName.Contains(inventoryName)) &&
                            (!dateFrom.HasValue || it.TransactionDate >= dateFrom.Value.Date) &&
                            (!dateTo.HasValue || it.TransactionDate <= dateTo.Value.Date) &&
                            (!activityType.HasValue || it.ActivityType == activityType)
                        select it)
                        .Include(x => x.Inventory);

            return await query.ToListAsync();
        }

        // TODO: change to AddProduceProductAsync
        public async Task ProduceProductAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy, int price)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();

            await db.InventoryTransactions.AddAsync(
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

            await db.SaveChangesAsync();
        }

        // TODO: change to AddProduceProductAsync
        public async Task PurchaseInventoryAsync(string purchaseNumber, Inventory inventory, int quantity, string doneBy, double price)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();

            await db.InventoryTransactions.AddAsync(
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

            await db.SaveChangesAsync();
        }
    }
}

