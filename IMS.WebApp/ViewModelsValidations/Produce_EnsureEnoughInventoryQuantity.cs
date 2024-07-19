using IMS.WebApp.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModelsValidations
{
    public class Produce_EnsureEnoughInventoryQuantity : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var produceViewModel = validationContext.ObjectInstance as ProduceViewModel;
            if (produceViewModel is not null)
            {
                if (produceViewModel.Product is not null)
                {
                    foreach (var productInventory in produceViewModel.Product.ProductInventories)
                    {
                        if (productInventory.Inventory is not null &&
                            productInventory.InventoryQuantity * produceViewModel.QuantityToProduce > productInventory.Inventory.Quantity)
                        {
                            return new ValidationResult($"The inventory ({productInventory.Inventory.InventoryName}) is not enough to produce {produceViewModel.QuantityToProduce} products.",
                                new[] { validationContext.MemberName });
                        }
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
