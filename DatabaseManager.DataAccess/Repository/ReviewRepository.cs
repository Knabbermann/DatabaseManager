using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        private readonly WebDbContext _webDbContext;
        public ReviewRepository(WebDbContext webDbContext) : base(webDbContext)
        {
            _webDbContext = webDbContext;
        }
        public void Update(Review review)
        {
            _webDbContext.Update(review);
            _webDbContext.SaveChanges();
        }
    }
}
