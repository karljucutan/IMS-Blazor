using IMS.UseCases.Inventories.DTOs;

namespace IMS.UseCases.Inventories.Activities.Interfaces
{
    public interface IPurchaseInventoryUseCase
    {
        Task ExecuteAsync(PurchaseOrderDTO purchaseOrderDTO);
    }
}