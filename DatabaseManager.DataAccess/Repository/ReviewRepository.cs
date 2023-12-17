using AutoMapper;
using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class ReviewRepository(WebDbContextShard1 webDbContextShard1, WebDbContextShard2 webDbContextShard2, IUnitOfWork unitOfWork) : Repository<Review>(webDbContextShard1, webDbContextShard2, unitOfWork), 
        IReviewRepository
    {
        public Review? Update(Review cReview)
        {
            var uReview = unitOfWork.Review.GetById(cReview.Id);
            if (uReview == null) return null;

            var config = new MapperConfiguration(x => x.CreateMap<Review, Review>());
            var mapper = config.CreateMapper();
            mapper.Map(cReview, uReview);
            webDbContextShard1.Update(uReview);
            return uReview;
        }
    }
}
