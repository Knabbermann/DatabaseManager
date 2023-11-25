using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class ProductRepository(WebDbContext webDbContext) : Repository<Product>(webDbContext), IProductRepository
    {
        public void Update(Product product)
        {
            webDbContext.Update(product);
        }
    }
}
