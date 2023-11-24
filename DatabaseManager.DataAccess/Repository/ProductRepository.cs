using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly WebDbContext _webDbContext;
        public ProductRepository(WebDbContext webDbContext) : base(webDbContext)
        {
            _webDbContext = webDbContext;
        }
        public void Update(Product product)
        {
            _webDbContext.Update(product);
            _webDbContext.SaveChanges();
        }
    }
}
