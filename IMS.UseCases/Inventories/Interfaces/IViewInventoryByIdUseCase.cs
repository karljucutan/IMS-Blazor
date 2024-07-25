using IMS.CoreBusiness;
using IMS.UseCases.ViewModels;

namespace IMS.UseCases.Inventories.Interfaces
{
    public interface IViewInventoryByIdUseCase
    {
        Task<InventoryViewModel> ExecuteAsync(int inventoryId);
    }
}