using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace DatabaseManager.Web.Pages.Tables.Customer
{
    public class AddModel(IUnitOfWork unitOfWork, IToastNotification toastNotification, Models.Customer cCustomer) : CustomPageModel<AddModel>
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
                unitOfWork.Customer.Add(cCustomer, Guid.NewGuid());
                toastNotification.AddSuccessToastMessage("Successfully added Customer.");
                return RedirectToPage("/Tables/Customer/Index");
            }

            return Page();
        }
    }
}
