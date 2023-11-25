using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class ReviewRepository(WebDbContext webDbContext) : Repository<Review>(webDbContext), IReviewRepository
    {
        public void Update(Review review)
        {
            webDbContext.Update(review);
        }
    }
}
