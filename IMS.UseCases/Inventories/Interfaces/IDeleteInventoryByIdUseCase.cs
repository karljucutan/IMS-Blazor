namespace IMS.UseCases.Inventories.Interfaces
{
    public interface IDeleteInventoryByIdUseCase
    {
        Task ExecuteAsync(int inventoryId);
    }
}