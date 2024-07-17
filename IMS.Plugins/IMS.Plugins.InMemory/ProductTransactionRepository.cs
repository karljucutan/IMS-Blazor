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

        public async Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy)
        {
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
    }
}
