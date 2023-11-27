using DatabaseManager.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace DatabaseManager.Web.Pages.Tables.Product
{
    public class EditModel(IUnitOfWork unitOfWork, IToastNotification toastNotification, Models.Product? cProduct) : CustomPageModel<EditModel>
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public Models.Product? CProduct { get; set; } = cProduct;

        public IActionResult OnGet()
        {
            if (Id == 0)
            {
                toastNotification.AddErrorToastMessage("Id is null");
                return RedirectToPage("/Tables/Product/Index");
            }

            CProduct = unitOfWork.Product.GetFirstOrDefault(x => x.Id == Id);
            if (CProduct == null)
            {
                toastNotification.AddErrorToastMessage("Object is null");
                return RedirectToPage("/Tables/Product/Index");
            }

            return Page();
        }

        public IActionResult OnPost(Models.Product cProduct)
        {
            ModelState.Remove("Id");
            ModelState.Remove("cProduct.OrderItems");
            if (ModelState.IsValid)
            {
                var uProduct = unitOfWork.Product.Update(cProduct);
                if (uProduct == null)
                {
                    toastNotification.AddErrorToastMessage($"Could not found product with Id {cProduct.Id}!");
                    return RedirectToPage("/Tables/Product/Index");
                }

                unitOfWork.SaveChanges();
                toastNotification.AddSuccessToastMessage("Successfully edited product.");
                return RedirectToPage("/Tables/Product/Index");
            }

            return Page();
        }
    }
}
