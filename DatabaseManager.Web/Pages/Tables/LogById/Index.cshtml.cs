using DatabaseManager.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace DatabaseManager.Web.Pages.Tables.LogById
{
    public class IndexModel(IUnitOfWork unitOfWork, IToastNotification toastNotification, IEnumerable<Models.LogWithId> logWithIds) : CustomPageModel<IndexModel>
    {
        [BindProperty(SupportsGet = true)]
        public IEnumerable<Models.LogWithId> LogWithIds { get; set; } = logWithIds;
        [BindProperty(SupportsGet = true)]
        public int RowCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int UsedSpace { get; set; }
        public void OnGet(int pageNumber = 1)
        {
            LogWithIds = unitOfWork.LogWithId.GetPagedEntities(x => x.Id,pageNumber, 10);
            RowCount = unitOfWork.LogWithId.GetRowCount();
            UsedSpace = unitOfWork.LogWithId.GetUsedSpace();
        }
    }
}
