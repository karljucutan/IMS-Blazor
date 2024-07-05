using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Inventories
{
    public class DeleteInventoryByIdUseCase : IDeleteInventoryByIdUseCase
    {
        private readonly IInventoryRepository _inventoryRepository;

        public DeleteInventoryByIdUseCase(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task ExecuteAsync(int inventoryId)
        {
            await _inventoryRepository.DeleteInventoryByIdAsync(inventoryId);
        }
    }
}
