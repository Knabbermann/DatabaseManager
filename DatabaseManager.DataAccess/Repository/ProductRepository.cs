using AutoMapper;
using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class ProductRepository(WebDbContextShard1 webDbContextShard1, WebDbContextShard2 webDbContextShard2, IUnitOfWork unitOfWork) : Repository<Product>(webDbContextShard1,webDbContextShard2, unitOfWork), 
        IProductRepository
    {
        public Product? Update(Product cProduct)
        {
            var uProduct = unitOfWork.Product.GetById(cProduct.Id);
            if (uProduct == null) return null;

            var config = new MapperConfiguration(x => x.CreateMap<Product, Product>());
            var mapper = config.CreateMapper();
            mapper.Map(cProduct, uProduct);
            webDbContextShard1.Update(uProduct);
            return uProduct;
        }
    }
}
