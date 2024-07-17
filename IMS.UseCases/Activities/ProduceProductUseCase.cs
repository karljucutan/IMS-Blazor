using IMS.CoreBusiness;
using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Activities
{
    public class ProduceProductUseCase : IProduceProductUseCase
    {
        private readonly IProductTransactionRepository _productTransactionRepository;
        private readonly IProductRepository _productRepository;

        public ProduceProductUseCase(IProductTransactionRepository productTransactionRepository,
            IProductRepository productRepository)
        {
            _productTransactionRepository = productTransactionRepository;
            _productRepository = productRepository;
        }

        public async Task ExecuteAsync(string productionNumber, Product product, int quanity, string doneBy)
        {
            // add product transaction record
            await _productTransactionRepository.ProduceAsync(productionNumber, product, quanity, doneBy);

            //update the quantity of product
            product.Quantity += quanity;
            await _productRepository.UpdateProductAsync(product);
        }
    }
}
