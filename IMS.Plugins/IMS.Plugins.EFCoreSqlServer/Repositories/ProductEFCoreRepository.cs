using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer.Repositories
{
    // TODO: for enhancements create a base repository and baseentity<Tkey>
    public class ProductEFCoreRepository : IProductRepository
    {
        private readonly IDbContextFactory<IMSDbContext> _dbContextFactory;

        public ProductEFCoreRepository(IDbContextFactory<IMSDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task AddProductAsync(Product product)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            await db.Products.AddAsync(product);
            FlagInventoryUnchanged(product, db);
            await db.SaveChangesAsync();
        }

        public async Task DeleteProductByIdAsync(int productId)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            var product = await db.Products.FindAsync(productId);
            //ArgumentNullException.ThrowIfNull(product);
            if (product is null) return;

            db.Products.Remove(product);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            var test = await db.Products
                .Where(product => product.ProductName.ToLower().Contains(name.ToLower()))
                .ToListAsync();

            return test;
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            var product = await db.Products
                .Include(x => x.ProductInventories)
                .ThenInclude(x => x.Inventory)
                .FirstOrDefaultAsync(x => x.ProductId == productId);

            return product;
        }

        public async Task UpdateProductAsync(Product product)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            var productToUpdate = await db.Products
                .Include(x => x.ProductInventories)
                .FirstOrDefaultAsync(x => x.ProductId == product.ProductId);
            //ArgumentNullException.ThrowIfNull(productToUpdate, nameof(productToUpdate));
            if (productToUpdate is null) return;

            productToUpdate.ProductName = product.ProductName;
            productToUpdate.Price = product.Price;
            productToUpdate.Quantity = product.Quantity;
            productToUpdate.ProductInventories = product.ProductInventories;
            FlagInventoryUnchanged(productToUpdate, db);

            await db.SaveChangesAsync();
        }

        // TODO check if needed or not. check if it will change the inventory record
        private void FlagInventoryUnchanged(Product product, IMSDbContext dbContext)
        {
            if (product?.ProductInventories is not null &&
                product.ProductInventories.Count > 0)
            {
                foreach (var prodInv in product.ProductInventories) 
                {
                    dbContext.Entry(prodInv.Inventory).State = EntityState.Unchanged;
                }
            }
        }
    }
}
