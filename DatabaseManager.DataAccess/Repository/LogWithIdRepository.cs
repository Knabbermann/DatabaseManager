using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class LogWithIdRepository(WebDbContext webDbContext, IUnitOfWork unitOfWork) : LogRepository<LogWithId>(webDbContext, unitOfWork), ILogWithIdRepository
    {
         
    }
}
