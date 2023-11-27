using DatabaseManager.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace DatabaseManager.Web.Pages.Tables.Order
{
    public class IndexModel(IUnitOfWork unitOfWork, IToastNotification toastNotification, IEnumerable<Models.Order> orders) : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public IEnumerable<Models.Order> Orders { get; set; } = orders;
        [BindProperty(SupportsGet = true)]
        public int RowCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int ColumnCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int UsedSpace { get; set; }
        public void OnGet()
        {
            Orders = unitOfWork.Order.GetAll();
            RowCount = unitOfWork.Order.GetRowCount();
            ColumnCount = unitOfWork.Order.GetColumnCount();
            UsedSpace = unitOfWork.Order.GetUsedSpace();
        }

        public IActionResult OnPostRemove(int id)
        {
            var customer = unitOfWork.Order.GetFirstOrDefault(x => x.Id == id);
            if (customer == null)
            {
                toastNotification.AddErrorToastMessage($"Could not found Order with Id {id}!");
                return RedirectToPage("/Tables/Order/Index");
            }
            unitOfWork.Order.Remove(customer);
            unitOfWork.SaveChanges();
            toastNotification.AddSuccessToastMessage($"Successfully removed Order with Id {id}.");
            return RedirectToPage("/Tables/Order/Index");
        }

        public override string ToString()
        {
            var fullName = GetType().ToString().Replace(".IndexModel", "");
            var index = fullName.LastIndexOf('.');
            return index < 0 ? fullName : fullName[(index + 1)..];
        }
    }
}
