using DatabaseManager.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace DatabaseManager.Web.Pages.Tables.Review
{
    public class IndexModel(IUnitOfWork unitOfWork, IToastNotification toastNotification, IEnumerable<Models.Review> reviews) : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public IEnumerable<Models.Review> Reviews { get; set; } = reviews;
        [BindProperty(SupportsGet = true)]
        public int RowCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int ColumnCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int UsedSpace { get; set; }
        public void OnGet()
        {
            Reviews = unitOfWork.Review.GetAll();
            RowCount = unitOfWork.Review.GetRowCount();
            ColumnCount = unitOfWork.Review.GetColumnCount();
            UsedSpace = unitOfWork.Review.GetUsedSpace();
        }

        public IActionResult OnPostRemove(int id)
        {
            var review = unitOfWork.Review.GetFirstOrDefault(x => x.Id == id);
            if (review == null)
            {
                toastNotification.AddErrorToastMessage($"Could not found Review with Id {id}!");
                return RedirectToPage("/Tables/Review/Index");
            }
            unitOfWork.Review.Remove(review);
            unitOfWork.SaveChanges();
            toastNotification.AddSuccessToastMessage($"Successfully removed Review with Id {id}.");
            return RedirectToPage("/Tables/Review/Index");
        }

        public override string ToString()
        {
            var fullName = GetType().ToString().Replace(".IndexModel", "");
            var index = fullName.LastIndexOf('.');
            return index < 0 ? fullName : fullName[(index + 1)..];
        }
    }
}
