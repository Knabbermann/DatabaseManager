using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class PerformanceRepository(WebDbContext webDbContext, IUnitOfWork unitOfWork) : LogRepository<Performance>(webDbContext, unitOfWork), IPerformanceRepository
    {
    }
}
