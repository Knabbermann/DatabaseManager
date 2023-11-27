using DatabaseManager.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace DatabaseManager.Web.Pages.Tables.Review
{
    public class AddModel(IUnitOfWork unitOfWork, IToastNotification toastNotification, Models.Review cReview) : PageModel
    {
        [BindProperty]
        public Models.Review CReview { get; set; } = cReview;

        public void OnGet()
        {
        }

        public IActionResult OnPost(Models.Review cReview)
        {
            ModelState.Remove("cReview.Customer");
            var cCustomer = unitOfWork.Customer.GetById(cReview.CustomerId);
            if (cCustomer == null) ModelState.AddModelError("CReview.CustomerId", $"Could not found customer with id {cReview.CustomerId}!");
            if (ModelState.IsValid)
            {
                unitOfWork.Review.Add(cReview);
                unitOfWork.SaveChanges();
                toastNotification.AddSuccessToastMessage("Successfully added Review.");
                return RedirectToPage("/Tables/Review/Index");
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
