using IMS.UseCases.Inventories.DTOs;

namespace IMS.UseCases.Activities.Interfaces
{
    public interface IPurchaseInventoryUseCase
    {
        Task ExecuteAsync(PurchaseOrderDTO purchaseOrderDTO);
    }
}