using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseManager.Web.Pages
{
    public class CustomPageModel<T> : PageModel
    {
        public override string ToString()
        {
            var fullName = typeof(T).ToString().Replace($".{typeof(T).Name}", "");
            var index = fullName.LastIndexOf('.');
            return index < 0 ? fullName : fullName[(index + 1)..];
        }
    }
}
