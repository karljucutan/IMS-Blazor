using IMS.CoreBusiness;
using IMS.Plugins.EFCoreSqlServer;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.InMemory
{
    public class ProductTransactionEFCoreRepository : IProductTransactionRepository
    {
        private readonly IDbContextFactory<IMSDbContext> _dbContextFactory;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public ProductTransactionEFCoreRepository(IDbContextFactory<IMSDbContext> dbContextFactory,
            IProductRepository productRepository,
            IInventoryTransactionRepository inventoryTransactionRepository,
            IInventoryRepository inventoryRepository)
        {
            _dbContextFactory = dbContextFactory;
            _productRepository = productRepository;
            _inventoryTransactionRepository = inventoryTransactionRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? activityType)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();

            var query = (from pt in db.ProductTransactions
                        join inv in db.Products
                        on pt.ProductId equals inv.ProductId
                        where
                            (string.IsNullOrWhiteSpace(productName) || inv.ProductName.Contains(productName)) &&
                            (!dateFrom.HasValue || pt.TransactionDate >= dateFrom.Value.Date) &&
                            (!dateTo.HasValue || pt.TransactionDate <= dateTo.Value.Date) &&
                            (!activityType.HasValue || pt.ActivityType == activityType)
                        select pt)
                        .Include(x => x.Product);

            return await query.ToListAsync();
        }

        public async Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();

            // TODO: this part can be move to the application layer since this is not related to ProductTransaction.
            var productToUpdate = await _productRepository.GetProductByIdAsync(product.ProductId);
            if (productToUpdate is not null)
            {
                foreach (var productInventory in productToUpdate.ProductInventories)
                {
                    if (productInventory is not null)
                    {
                        // add inventory transaction as produce product
                        await _inventoryTransactionRepository.ProduceProductAsync(productionNumber,
                        productInventory.Inventory,
                        productInventory.InventoryQuantity * quantity,
                        doneBy,
                        -1);

                        // decrease inventories
                        var inventoryToUpdate = await _inventoryRepository.GetInventoryByIdAsync(productInventory.InventoryId);
                        inventoryToUpdate.Quantity -= productInventory.InventoryQuantity * quantity;
                        await _inventoryRepository.UpdateInventoryAsync(inventoryToUpdate);
                    }
                }
            }
            // TODO: end

            // add product transaction
            await db.ProductTransactions.AddAsync(new ProductTransaction
            {
                ProductionNumber = productionNumber,
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                ActivityType = ProductTransactionType.ProduceProduct,
                QuantityAfter = product.Quantity + quantity,
                TransactionDate = DateTime.UtcNow,
                DoneBy = doneBy
            });

            await db.SaveChangesAsync();
        }

        public async Task SellProductAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();

            await db.ProductTransactions.AddAsync(new ProductTransaction
            {
                ActivityType = ProductTransactionType.SellProduct,
                SalesOrderNumber = salesOrderNumber,
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                QuantityAfter = product.Quantity - quantity,
                DoneBy = doneBy,
                UnitPrice = unitPrice,
            });

            await db.SaveChangesAsync();
        }
    }
}
