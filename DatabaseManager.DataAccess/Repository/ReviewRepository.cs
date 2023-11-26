using AutoMapper;
using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class ReviewRepository(WebDbContext webDbContext, IUnitOfWork unitOfWork) : Repository<Review>(webDbContext, unitOfWork), 
        IReviewRepository
    {
        public Review? Update(Review cReview)
        {
            var uReview = unitOfWork.Review.GetById(cReview.Id);
            if (uReview == null) return null;

            var config = new MapperConfiguration(x => x.CreateMap<Customer, Customer>());
            var mapper = config.CreateMapper();
            mapper.Map(cReview, uReview);
            webDbContext.Update(uReview);
            return uReview;
        }
    }
}
