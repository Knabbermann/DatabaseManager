using DatabaseManager.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace DatabaseManager.Web.Pages.Tables.Customer
{
    public class IndexModel(IUnitOfWork unitOfWork, IToastNotification toastNotification, IEnumerable<Models.Customer> customers) : CustomPageModel<IndexModel>
    {
        [BindProperty(SupportsGet = true)]
        public IEnumerable<Models.Customer> Customers { get; set; } = customers;
        [BindProperty(SupportsGet = true)]
        public int RowCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int ColumnCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int UsedSpace { get; set; }
        public void OnGet()
        {
            Customers = unitOfWork.Customer.GetAll();
            RowCount = unitOfWork.Customer.GetRowCount();
            ColumnCount = unitOfWork.Customer.GetColumnCount();
            UsedSpace = unitOfWork.Customer.GetUsedSpace();
        }

        public IActionResult OnPostRemove(int id)
        {
            var customer = unitOfWork.Customer.GetFirstOrDefault(x => x.Id == id);
            if (customer == null)
            {
                toastNotification.AddErrorToastMessage($"Could not found Customer with Id {id}!");
                return RedirectToPage("/Tables/Customer/Index");
            }
            unitOfWork.Customer.Remove(customer);
            unitOfWork.SaveChanges();
            toastNotification.AddSuccessToastMessage($"Successfully removed Customer with Id {id}.");
            return RedirectToPage("/Tables/Customer/Index");
        }
    }
}
