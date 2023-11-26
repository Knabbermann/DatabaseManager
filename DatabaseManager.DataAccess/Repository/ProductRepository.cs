using AutoMapper;
using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class ProductRepository(WebDbContext webDbContext, IUnitOfWork unitOfWork) : Repository<Product>(webDbContext, unitOfWork), 
        IProductRepository
    {
        public Product? Update(Product cProduct)
        {
            var uProduct = unitOfWork.Product.GetById(cProduct.Id);
            if (uProduct == null) return null;

            var config = new MapperConfiguration(x => x.CreateMap<Product, Product>());
            var mapper = config.CreateMapper();
            mapper.Map(cProduct, uProduct);
            webDbContext.Update(uProduct);
            return uProduct;
        }
    }
}
