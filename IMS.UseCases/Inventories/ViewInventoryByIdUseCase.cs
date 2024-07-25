using AutoMapper;
using IMS.CoreBusiness;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.ViewModels;

namespace IMS.UseCases.Inventories
{
    public class ViewInventoryByIdUseCase : IViewInventoryByIdUseCase
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;

        public ViewInventoryByIdUseCase(IInventoryRepository inventoryRepository, IMapper mapper)
        {
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
        }

        public async Task<InventoryViewModel> ExecuteAsync(int inventoryId)
        {
            return _mapper.Map<InventoryViewModel>(await _inventoryRepository.GetInventoryByIdAsync(inventoryId));
        }
    }
}
