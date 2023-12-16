using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace DatabaseManager.Web.Pages.Tables.Order
{
    public class AddModel(IUnitOfWork unitOfWork, IToastNotification toastNotification, Models.Order cOrder) : CustomPageModel<AddModel>
    {
        [BindProperty]
        public Models.Order COrder { get; set; } = cOrder;

        public void OnGet()
        {
        }

        public IActionResult OnPost(Models.Order cOrder)
        {
            ModelState.Remove("cOrder.Customer");
            ModelState.Remove("cOrder.OrderItems");
            var cCustomer = unitOfWork.Customer.GetById(cOrder.CustomerId);
            if (cCustomer == null) ModelState.AddModelError("COrder.CustomerId", $"Could not found customer with id {cOrder.CustomerId}!");
            if (ModelState.IsValid)
            {
                unitOfWork.Order.Add(cOrder, Guid.NewGuid());
                toastNotification.AddSuccessToastMessage("Successfully added Order.");
                return RedirectToPage("/Tables/Order/Index");
            }

            return Page();
        }
    }
}
