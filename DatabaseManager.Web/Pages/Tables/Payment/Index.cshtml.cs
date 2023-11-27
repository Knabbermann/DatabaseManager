using DatabaseManager.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace DatabaseManager.Web.Pages.Tables.Payment
{
    public class IndexModel(IUnitOfWork unitOfWork, IToastNotification toastNotification, IEnumerable<Models.Payment> payments) : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public IEnumerable<Models.Payment> Payments { get; set; } = payments;
        [BindProperty(SupportsGet = true)]
        public int RowCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int ColumnCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int UsedSpace { get; set; }
        public void OnGet()
        {
            Payments = unitOfWork.Payment.GetAll();
            RowCount = unitOfWork.Payment.GetRowCount();
            ColumnCount = unitOfWork.Payment.GetColumnCount();
            UsedSpace = unitOfWork.Payment.GetUsedSpace();
        }

        public IActionResult OnPostRemove(int id)
        {
            var payment = unitOfWork.Payment.GetFirstOrDefault(x => x.Id == id);
            if (payment == null)
            {
                toastNotification.AddErrorToastMessage($"Could not found Payment with Id {id}!");
                return RedirectToPage("/Tables/Payment/Index");
            }
            unitOfWork.Payment.Remove(payment);
            unitOfWork.SaveChanges();
            toastNotification.AddSuccessToastMessage($"Successfully removed Payment with Id {id}.");
            return RedirectToPage("/Tables/Payment/Index");
        }

        public override string ToString()
        {
            var fullName = GetType().ToString().Replace(".IndexModel", "");
            var index = fullName.LastIndexOf('.');
            return index < 0 ? fullName : fullName[(index + 1)..];
        }
    }
}
