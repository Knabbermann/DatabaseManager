using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class LogWithIdRepository(WebDbContextShard1 webDbContext, IUnitOfWork unitOfWork) : LogRepository<LogWithId>(webDbContext, unitOfWork), ILogWithIdRepository
    {
         
    }
}
