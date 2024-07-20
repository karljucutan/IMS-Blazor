namespace IMS.CoreBusiness.Extensions
{
    public static class InventoryTransactionTypeExtensions
    {
        public static string GetEnumName(this InventoryTransactionType type)
        {
            string name = type switch
            {
                InventoryTransactionType.PurchaseInventory => "Purchase Inventory",
                InventoryTransactionType.ProduceProduct => "Produce Product",
                _ => "Unknown"  // The default case
            };

            return name;
        }
    }
}
