using DatabaseManager.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace DatabaseManager.Web.Pages.Tables.Customer
{
    public class IndexModel(IUnitOfWork unitOfWork, IToastNotification toastNotification, IEnumerable<Models.Customer> customers) : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public IEnumerable<Models.Customer> Customers { get; set; } = customers;

        public void OnGet()
        {
            Customers = unitOfWork.Customer.GetAll();
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

        public override string ToString()
        {
            var fullName = GetType().ToString().Replace(".IndexModel","");
            var index = fullName.LastIndexOf('.');
            return index < 0 ? fullName : fullName[(index + 1)..];
        }
    }
}
