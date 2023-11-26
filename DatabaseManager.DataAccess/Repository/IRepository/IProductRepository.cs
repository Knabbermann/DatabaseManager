using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Product? Update(Product cProduct);
    }
}
