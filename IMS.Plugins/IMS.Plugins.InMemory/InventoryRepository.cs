using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class InventoryRepository : IInventoryRepository
    {
        private List<Inventory> _inventories;

        public InventoryRepository()
        {
            _inventories = new()
            {
                new Inventory { InventoryId = 1, InventoryName = "Bike Seat", Quantity = 10, Price = 2 },
                new Inventory { InventoryId = 2, InventoryName = "Bike Body", Quantity = 10, Price = 2 },
                new Inventory { InventoryId = 3, InventoryName = "Bike Wheels", Quantity = 20, Price = 2 },
                new Inventory { InventoryId = 4, InventoryName = "Bike Pedals", Quantity = 20, Price = 2 },
            };
        }

        public Task AddInventoryAsync(Inventory inventory)
        {
            if (_inventories.Any(inventories_inv => string.Equals(inventories_inv.InventoryName, inventory.InventoryName, StringComparison.OrdinalIgnoreCase)))
                return Task.CompletedTask;

            var maxId = _inventories.Max(inventory => inventory.InventoryId);
            inventory.InventoryId = maxId + 1;

            _inventories.Add(inventory);

            return Task.CompletedTask;
        }

        public Task DeleteInventoryByIdAsync(int inventoryId)
        {
            var inventoryToDelete = _inventories.FirstOrDefault(inventory => inventory.InventoryId == inventoryId);

            if (inventoryToDelete is not null)
                _inventories.Remove(inventoryToDelete);

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return await Task.FromResult(_inventories);

            return _inventories.Where(inventory => inventory.InventoryName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<Inventory> GetInventoryByIdAsync(int inventoryId)
        {
            var inventory = _inventories.First(inventory => inventory.InventoryId == inventoryId);

            var newInventory = new Inventory
            {
                InventoryId = inventory.InventoryId,
                InventoryName = inventory.InventoryName,
                Price = inventory.Price,
                Quantity = inventory.Quantity
            };

            return await Task.FromResult(newInventory);
        }

        public Task UpdateInventoryAsync(Inventory inventory)
        {
            if (_inventories.Any(x => x.InventoryId != inventory.InventoryId &&
                x.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase)))
                return Task.CompletedTask;

            var inventoryToUpdate = _inventories.FirstOrDefault(x => x.InventoryId == inventory.InventoryId);  
            
            if (inventoryToUpdate is not null)
            {
                inventoryToUpdate.InventoryName = inventory.InventoryName;
                inventoryToUpdate.Quantity = inventory.Quantity;
                inventoryToUpdate.Price = inventory.Price;
            }

            return Task.CompletedTask;
        }
    }
}
