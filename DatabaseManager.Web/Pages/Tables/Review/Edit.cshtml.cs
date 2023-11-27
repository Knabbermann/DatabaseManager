using DatabaseManager.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace DatabaseManager.Web.Pages.Tables.Review
{
    public class EditModel(IUnitOfWork unitOfWork, IToastNotification toastNotification, Models.Review? cReview) : CustomPageModel<EditModel>
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public Models.Review? CReview { get; set; } = cReview;

        public IActionResult OnGet()
        {
            if (Id == 0)
            {
                toastNotification.AddErrorToastMessage("Id is null");
                return RedirectToPage("/Tables/Review/Index");
            }

            CReview = unitOfWork.Review.GetFirstOrDefault(x => x.Id == Id);
            if (CReview == null)
            {
                toastNotification.AddErrorToastMessage("Object is null");
                return RedirectToPage("/Tables/Review/Index");
            }

            return Page();
        }

        public IActionResult OnPost(Models.Review cReview)
        {
            ModelState.Remove("Id");
            ModelState.Remove("cReview.Customer");
            if (ModelState.IsValid)
            {
                var uReview = unitOfWork.Review.Update(cReview);
                if (uReview == null)
                {
                    toastNotification.AddErrorToastMessage($"Could not found review with Id {cReview.Id}!");
                    return RedirectToPage("/Tables/Review/Index");
                }

                unitOfWork.SaveChanges();
                toastNotification.AddSuccessToastMessage("Successfully edited review.");
                return RedirectToPage("/Tables/Review/Index");
            }

            return Page();
        }
    }
}
