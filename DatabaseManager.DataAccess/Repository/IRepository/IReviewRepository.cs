using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository.IRepository
{
    public interface IReviewRepository : IRepository<Review>
    {
        Review? Update(Review review);
    }
}
