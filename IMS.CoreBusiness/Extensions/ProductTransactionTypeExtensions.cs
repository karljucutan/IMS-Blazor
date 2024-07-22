namespace IMS.CoreBusiness.Extensions
{
    public static class ProductTransactionTypeExtensions
    {
        public static string GetEnumName(this ProductTransactionType type)
        {
            string name = type switch
            {
                ProductTransactionType.SellProduct => "Sell Product",
                ProductTransactionType.ProduceProduct => "Produce Product",
                _ => "Unknown"  // The default case
            };

            return name;
        }
    }
}
