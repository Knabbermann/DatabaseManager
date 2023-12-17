using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace DatabaseManager.Web.Pages.Tables.Product
{
    public class AddModel(IUnitOfWork unitOfWork, IToastNotification toastNotification, Models.Product cProduct, IConfiguration configuration) : CustomPageModel<AddModel>(configuration)
    {
        [BindProperty]
        public Models.Product CProduct { get; set; } = cProduct;

        public void OnGet()
        {
        }

        public IActionResult OnPost(Models.Product cProduct)
        {
            ModelState.Remove("cProduct.OrderItems");
            if (ModelState.IsValid)
            {
                unitOfWork.Product.Add(cProduct, Guid.NewGuid());
                toastNotification.AddSuccessToastMessage("Successfully added Product.");
                return RedirectToPage("/Tables/Product/Index");
            }

            return Page();
        }
    }
}
