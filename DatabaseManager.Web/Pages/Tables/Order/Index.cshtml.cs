using DatabaseManager.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace DatabaseManager.Web.Pages.Tables.Order
{
    public class IndexModel(IUnitOfWork unitOfWork, IToastNotification toastNotification, IEnumerable<Models.Order> orders) : CustomPageModel<IndexModel>
    {
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public IEnumerable<Models.Order> Orders { get; set; } = orders;
        [BindProperty(SupportsGet = true)]
        public int RowCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int ColumnCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int UsedSpace { get; set; }
        public void OnGet(int pageNumber = 1)
        {
            CurrentPage = pageNumber;
            Orders = unitOfWork.Order.GetPagedEntities(pageNumber, 10);
            RowCount = unitOfWork.Order.GetRowCount();
            ColumnCount = unitOfWork.Order.GetColumnCount();
            UsedSpace = unitOfWork.Order.GetUsedSpace();
        }

        public IActionResult OnPostRemove(int id)
        {
            var order = unitOfWork.Order.GetFirstOrDefault(x => x.Id == id);
            if (order == null)
            {
                toastNotification.AddErrorToastMessage($"Could not found Order with Id {id}!");
                return RedirectToPage("/Tables/Order/Index");
            }
            unitOfWork.Order.Remove(order);
            unitOfWork.SaveChanges();
            toastNotification.AddSuccessToastMessage($"Successfully removed Order with Id {id}.");
            return RedirectToPage("/Tables/Order/Index");
        }
    }
}
