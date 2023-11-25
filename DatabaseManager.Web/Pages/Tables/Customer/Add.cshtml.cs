using DatabaseManager.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace DatabaseManager.Web.Pages.Tables.Customer
{
    public class AddModel(IUnitOfWork unitOfWork, IToastNotification toastNotification, Models.Customer cCustomer) : PageModel
    {
        [BindProperty]
        public Models.Customer CCustomer { get; set; } = cCustomer;

        public void OnGet()
        {
        }

        public IActionResult OnPost(Models.Customer cCustomer)
        {
            ModelState.Remove("cCustomer.Orders");
            ModelState.Remove("cCustomer.Payments");
            ModelState.Remove("cCustomer.Reviews");
            ModelState.Remove("cCustomer.GcRecord");
            if(ModelState.IsValid)
            {
                unitOfWork.Customer.Add(cCustomer);
                unitOfWork.SaveChanges();
                toastNotification.AddSuccessToastMessage("Successfully added Customer.");
                return RedirectToPage("/Tables/Customer/Index");
            }

            return Page();
        }

        public override string ToString()
        {
            var fullName = GetType().ToString().Replace(".IndexModel", "");
            var index = fullName.LastIndexOf('.');
            return index < 0 ? fullName : fullName[(index + 1)..];
        }
    }
}
