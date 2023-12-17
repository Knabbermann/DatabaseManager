using DatabaseManager.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace DatabaseManager.Web.Pages.Tables.Payment
{
    public class EditModel(IUnitOfWork unitOfWork, IToastNotification toastNotification, Models.Payment? cPayment, IConfiguration configuration) : CustomPageModel<EditModel>(configuration)
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public Models.Payment? CPayment { get; set; } = cPayment;

        public IActionResult OnGet()
        {
            if (Id == 0)
            {
                toastNotification.AddErrorToastMessage("Id is null");
                return RedirectToPage("/Tables/Payment/Index");
            }

            CPayment = unitOfWork.Payment.GetFirstOrDefault(x => x.Id == Id);
            if (CPayment == null)
            {
                toastNotification.AddErrorToastMessage("Object is null");
                return RedirectToPage("/Tables/Payment/Index");
            }

            return Page();
        }

        public IActionResult OnPost(Models.Payment cPayment)
        {
            ModelState.Remove("Id");
            ModelState.Remove("cPayment.Customer");
            if (ModelState.IsValid)
            {
                var uOrder = unitOfWork.Payment.Update(cPayment);
                if (uOrder == null)
                {
                    toastNotification.AddErrorToastMessage($"Could not found payment with Id {cPayment.Id}!");
                    return RedirectToPage("/Tables/Payment/Index");
                }

                unitOfWork.SaveChanges();
                toastNotification.AddSuccessToastMessage("Successfully edited payment.");
                return RedirectToPage("/Tables/Payment/Index");
            }

            return Page();
        }
    }
}
