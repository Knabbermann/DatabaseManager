using DatabaseManager.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace DatabaseManager.Web.Pages.Tables.Product
{
    public class IndexModel(IUnitOfWork unitOfWork, IToastNotification toastNotification, IEnumerable<Models.Product> products, IConfiguration configuration) : CustomPageModel<IndexModel>(configuration)
    {
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public IEnumerable<Models.Product> Products { get; set; } = products;
        [BindProperty(SupportsGet = true)]
        public int RowCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int ColumnCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public int UsedSpace { get; set; }
        public void OnGet(int pageNumber = 1)
        {
            CurrentPage = pageNumber;
            Products = unitOfWork.Product.GetPagedEntities(CurrentPage, 10);
            RowCount = unitOfWork.Product.GetRowCount();
            ColumnCount = unitOfWork.Product.GetColumnCount();
            UsedSpace = unitOfWork.Product.GetUsedSpace();
        }

        public IActionResult OnPostRemove(int id)
        {
            var product = unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                toastNotification.AddErrorToastMessage($"Could not found Product with Id {id}!");
                return RedirectToPage("/Tables/Product/Index");
            }
            unitOfWork.Product.Remove(product);
            unitOfWork.SaveChanges();
            toastNotification.AddSuccessToastMessage($"Successfully removed Product with Id {id}.");
            return RedirectToPage("/Tables/Product/Index");
        }
    }
}
