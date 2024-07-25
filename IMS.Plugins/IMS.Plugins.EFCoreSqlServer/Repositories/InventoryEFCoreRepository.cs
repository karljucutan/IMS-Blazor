using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer.Repositories
{
    // TODO: for enhancements create a base repository and baseentity<Tkey>
    public class InventoryEFCoreRepository : IInventoryRepository
    {
        private readonly IDbContextFactory<IMSDbContext> _dbContextFactory;

        public InventoryEFCoreRepository(IDbContextFactory<IMSDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task AddInventoryAsync(Inventory inventory)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            await db.Inventories.AddAsync(inventory);
            await db.SaveChangesAsync();
        }

        public async Task DeleteInventoryByIdAsync(int inventoryId)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            var inventory = await db.Inventories.FindAsync(inventoryId);
            //ArgumentNullException.ThrowIfNull(inventory);
            if (inventory is null) return;

            db.Inventories.Remove(inventory);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            var test = await db.Inventories
                .Where(inventory => inventory.InventoryName.ToLower().Contains(name.ToLower()))
                .ToListAsync();

            return test;
        }

        public async Task<Inventory> GetInventoryByIdAsync(int inventoryId)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            var inventory = await db.Inventories.FindAsync(inventoryId);

            return inventory;
        }

        public async Task UpdateInventoryAsync(Inventory inventory)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            var inventoryToUpdate = await db.Inventories.FindAsync(inventory.InventoryId);
            //ArgumentNullException.ThrowIfNull(inventoryToUpdate, nameof(inventoryToUpdate));
            if (inventoryToUpdate is null) return;

            inventoryToUpdate.InventoryName = inventory.InventoryName;
            inventoryToUpdate.Price = inventory.Price;
            inventoryToUpdate.Quantity = inventory.Quantity;

            await db.SaveChangesAsync();
        }
    }
}
