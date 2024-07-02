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
                new Inventory { InventoryId = 3, InventoryName = "Bike Wheels", Quantity = 10, Price = 2 },
                new Inventory { InventoryId = 4, InventoryName = "Bike Pedals", Quantity = 10, Price = 2 },
            };
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return await Task.FromResult(_inventories);

            return _inventories.Where(inventory =>  inventory.InventoryName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
