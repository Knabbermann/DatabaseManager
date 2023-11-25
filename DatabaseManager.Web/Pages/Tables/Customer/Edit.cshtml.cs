using AutoMapper;
using DatabaseManager.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace DatabaseManager.Web.Pages.Tables.Customer
{
    public class EditModel(IUnitOfWork unitOfWork, IToastNotification toastNotification, Models.Customer? cCustomer) : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public Models.Customer? CCustomer { get; set; } = cCustomer;

        public IActionResult OnGet()
        {
            if(Id == 0)
            {
                toastNotification.AddErrorToastMessage("Id is null");
                return RedirectToPage("/Tables/Customer/Index");
            }

            CCustomer = unitOfWork.Customer.GetFirstOrDefault(x => x.Id == Id);
            if(CCustomer == null)
            {
                toastNotification.AddErrorToastMessage("Object is null");
                return RedirectToPage("/Tables/Customer/Index");
            }

            return Page();
        }

        public IActionResult OnPost(Models.Customer cCustomer)
        {
            ModelState.Remove("Id");
            ModelState.Remove("cCustomer.Orders");
            ModelState.Remove("cCustomer.Payments");
            ModelState.Remove("cCustomer.Reviews");
            ModelState.Remove("cCustomer.GcRecord");
            if (ModelState.IsValid)
            {
                var uCustomer = unitOfWork.Customer.GetFirstOrDefault(x => x.Id == cCustomer.Id);
                if (uCustomer == null)
                {
                    toastNotification.AddErrorToastMessage($"Could not found Customer with Id {cCustomer.Id}!");
                    return RedirectToPage("/Tables/Customer/Index");
                }
                var config = new MapperConfiguration(x => x.CreateMap<Models.Customer, Models.Customer>());
                var mapper = config.CreateMapper();
                mapper.Map(cCustomer, uCustomer);
                unitOfWork.Customer.Update(uCustomer);
                unitOfWork.SaveChanges();
                toastNotification.AddSuccessToastMessage("Successfully edited Customer.");
                return RedirectToPage("/Tables/Customer/Index");
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
