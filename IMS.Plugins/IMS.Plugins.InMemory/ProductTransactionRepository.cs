using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class ProductTransactionRepository : IProductTransactionRepository
    {
        private readonly IProductRepository _productRepository;
        private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private List<ProductTransaction> _productTransactions = new();

        public ProductTransactionRepository(IProductRepository productRepository,
            IInventoryTransactionRepository inventoryTransactionRepository,
            IInventoryRepository inventoryRepository)
        {
            _productRepository = productRepository;
            _inventoryTransactionRepository = inventoryTransactionRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? activityType)
        {
            var products = (await _productRepository.GetProductsByNameAsync(string.Empty)).ToList();

            var query = from pt in _productTransactions
                        join inv in products
                        on pt.ProductId equals inv.ProductId
                        where
                            (string.IsNullOrWhiteSpace(productName) || inv.ProductName.Contains(productName)) &&
                            (!dateFrom.HasValue || pt.TransactionDate >= dateFrom.Value.Date) &&
                            (!dateTo.HasValue || pt.TransactionDate <= dateTo.Value.Date) &&
                            (!activityType.HasValue || pt.ActivityType == activityType)
                        select new ProductTransaction
                        {
                            Product = inv,
                            ProductTransactionId = pt.ProductTransactionId,
                            SalesOrderNumber = pt.SalesOrderNumber,
                            ProductId = pt.ProductId,
                            QuantityBefore = pt.QuantityBefore,
                            ActivityType = pt.ActivityType,
                            QuantityAfter = pt.QuantityAfter,
                            TransactionDate = pt.TransactionDate,
                            DoneBy = pt.DoneBy,
                            UnitPrice = pt.UnitPrice,
                        };

            return query;
        }

        // TODO: change to AddProduceProductTransactionAsync
        public async Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy)
        {
            // TODO: transfer to the application layer ProduceProductUseCase. this method should be just adding ProductTransaction to follow SRP principle
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

            // add product transaction
            _productTransactions.Add(new ProductTransaction
            {
                ProductionNumber = productionNumber,
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                ActivityType = ProductTransactionType.ProduceProduct,
                QuantityAfter = product.Quantity + quantity,
                TransactionDate = DateTime.UtcNow,
                DoneBy = doneBy
            });
        }

        // TODO: change to AddSellProductTransactionAsync
        public Task SellProductAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy)
        {
            _productTransactions.Add(new ProductTransaction
            {
                ActivityType = ProductTransactionType.SellProduct,
                SalesOrderNumber = salesOrderNumber,
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                QuantityAfter = product.Quantity - quantity,
                DoneBy = doneBy,
                UnitPrice = unitPrice,
            });

            return Task.CompletedTask;
        }
    }
}
