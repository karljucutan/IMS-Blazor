using IMS.CoreBusiness.Validations;
using System.ComponentModel.DataAnnotations;

namespace IMS.CoreBusiness
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be greater or equal to 0.")]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Price must be greater or equal to 0.")]
        public double Price { get; set; }

        [Product_EnsurePricesIsGreaterThanInventoriesCost]
        public List<ProductInventory> ProductInventories { get; set; } = new();

        public void AddInventory(Inventory inventory)
        {
            if (!ProductInventories.Any(
                productInventory => productInventory.Inventory is not null &&
                productInventory.Inventory.InventoryName.Equals(inventory.InventoryName)))
            {
                ProductInventories.Add(new()
                {
                    InventoryId = inventory.InventoryId,
                    Inventory = inventory,
                    InventoryQuantity = 1,
                    ProductId = ProductId,
                    Product = this
                });
            }
        }

        public void RemoveInventory(ProductInventory productInventory)
        {
            ProductInventories.Remove(productInventory);
        }
    }
}