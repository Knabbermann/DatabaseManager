using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseManager.Web.Pages.Generators.Product
{
    public class IndexModel : CustomPageModel<IndexModel>
    {
        public Models.Product Product;

        public void OnGet()
        {
        }
    }
}
