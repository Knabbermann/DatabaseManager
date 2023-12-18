using DatabaseManager.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace DatabaseManager.Web.Pages.Tables.Order
{
    public class EditModel(IUnitOfWork unitOfWork, IToastNotification toastNotification, Models.Order? cOrder, IConfiguration configuration) : CustomPageModel<EditModel>(configuration)
    {
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public Models.Order? COrder { get; set; } = cOrder;

        public IActionResult OnGet()
        {
            if (Id == Guid.Empty)
            {
                toastNotification.AddErrorToastMessage("Id is null");
                return RedirectToPage("/Tables/Order/Index");
            }

            COrder = unitOfWork.Order.GetFirstOrDefault(x => x.Id == Id);
            if (COrder == null)
            {
                toastNotification.AddErrorToastMessage("Object is null");
                return RedirectToPage("/Tables/Order/Index");
            }

            return Page();
        }

        public IActionResult OnPost(Models.Order cOrder)
        {
            ModelState.Remove("Id"); 
            ModelState.Remove("cOrder.Customer");
            ModelState.Remove("cOrder.OrderItems");
            if (ModelState.IsValid)
            {
                var uOrder = unitOfWork.Order.Update(cOrder);
                if (uOrder == null)
                {
                    toastNotification.AddErrorToastMessage($"Could not found order with Id {cOrder.Id}!");
                    return RedirectToPage("/Tables/Order/Index");
                }

                unitOfWork.SaveChanges();
                toastNotification.AddSuccessToastMessage("Successfully edited order.");
                return RedirectToPage("/Tables/Order/Index");
            }

            return Page();
        }
    }
}
