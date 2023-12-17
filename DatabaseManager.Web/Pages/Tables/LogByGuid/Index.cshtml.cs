using DatabaseManager.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace DatabaseManager.Web.Pages.Tables.LogByGuid
{
    public class IndexModel(IUnitOfWork unitOfWork, IToastNotification toastNotification, IEnumerable<Models.LogWithGuid> logWithGuids, IConfiguration configuration) : CustomPageModel<IndexModel>(configuration)
    {
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public IEnumerable<Models.LogWithGuid> LogWithGuids { get; set; } = logWithGuids;
        [BindProperty(SupportsGet = true)]
        public int RowCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int UsedSpace { get; set; }
        public void OnGet(int pageNumber = 1)
        {
            CurrentPage = pageNumber;
            LogWithGuids = unitOfWork.LogWithGuid.GetPagedEntities(x => x.Guid, pageNumber, 10);
            RowCount = unitOfWork.LogWithGuid.GetRowCount();
            UsedSpace = unitOfWork.LogWithGuid.GetUsedSpace();
        }
    }
}
