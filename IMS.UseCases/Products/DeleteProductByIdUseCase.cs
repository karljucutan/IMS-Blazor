using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products.Interfaces;

namespace IMS.UseCases.Products
{
    public class DeleteProductByIdUseCase : IDeleteProductByIdUseCase
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductByIdUseCase(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task ExecuteAsync(int productId)
        {
            await _productRepository.DeleteProductByIdAsync(productId);
        }
    }
}
