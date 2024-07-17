using IMS.CoreBusiness;

namespace IMS.UseCases.Activities.Interfaces
{
    public interface IProduceProductUseCase
    {
        Task ExecuteAsync(string productionNumber, Product product, int quanity, string doneBy);
    }
}