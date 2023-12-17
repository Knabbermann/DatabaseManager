using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace DatabaseManager.Web.Pages.Tables.Payment
{
    public class AddModel(IUnitOfWork unitOfWork, IToastNotification toastNotification, Models.Payment cPayment, IConfiguration configuration) : CustomPageModel<AddModel>(configuration)
    {
        [BindProperty]
        public Models.Payment CPayment { get; set; } = cPayment;

        public void OnGet()
        {
        }

        public IActionResult OnPost(Models.Payment cPayment)
        {
            ModelState.Remove("cPayment.Customer");
            var cCustomer = unitOfWork.Customer.GetById(cPayment.CustomerId);
            if (cCustomer == null) ModelState.AddModelError("CPayment.CustomerId", $"Could not found customer with id {cPayment.CustomerId}!");
            if (ModelState.IsValid)
            {
                unitOfWork.Payment.Add(cPayment, Guid.NewGuid());
                toastNotification.AddSuccessToastMessage("Successfully added Payment.");
                return RedirectToPage("/Tables/Payment/Index");
            }

            return Page();
        }
    }
}
