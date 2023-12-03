using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class LogWithGuidRepository(WebDbContext webDbContext, IUnitOfWork unitOfWork) : LogRepository<LogWithGuid>(webDbContext, unitOfWork), ILogWithGuidRepository
    {
    }
}
