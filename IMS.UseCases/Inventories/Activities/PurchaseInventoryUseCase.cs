using IMS.UseCases.Inventories.Activities.Interfaces;
using IMS.UseCases.Inventories.DTOs;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Inventories.Activities
{
    public class PurchaseInventoryUseCase : IPurchaseInventoryUseCase
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IInventoryTransactionRepository _inventoryTransactionRepository;

        public PurchaseInventoryUseCase(IInventoryRepository inventoryRepository,
            IInventoryTransactionRepository inventoryTransactionRepository)
        {
            _inventoryRepository = inventoryRepository;
            _inventoryTransactionRepository = inventoryTransactionRepository;
        }

        public async Task ExecuteAsync(PurchaseOrderDTO purchaseOrderDTO)
        {
            await _inventoryTransactionRepository.PurchaseInventoryAsync(
                purchaseOrderDTO.PurchaseNumber,
                purchaseOrderDTO.Inventory,
                purchaseOrderDTO.Quantity,
                purchaseOrderDTO.DoneBy,
                purchaseOrderDTO.Inventory.Price
                );
        }
    }
}
