using IMS.CoreBusiness;
using IMS.WebApp.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModelsValidations
{
    public class Sell_EnsureEnoughProductQuantity : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var sellViewModel = validationContext.ObjectInstance as SellViewModel;
            if (sellViewModel is not null && sellViewModel.Product is not null)
            {
                if (sellViewModel.Product.Quantity < sellViewModel.QuantityToSell) // TODO: assign this to a variable with a meaningful name
                {
                    return new ValidationResult($"There is not enough product. There is only {sellViewModel.Product.Quantity} in the warehouse.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
