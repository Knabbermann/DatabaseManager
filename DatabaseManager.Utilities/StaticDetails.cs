using Microsoft.AspNetCore.Mvc.Rendering;

namespace DatabaseManager.Utilities
{
    public class StaticDetails
    {
        public static List<SelectListItem> GenderOptions = new()
        {
            new SelectListItem { Text = "male", Value = "male" },
            new SelectListItem { Text = "female", Value = "female" },
            new SelectListItem { Text = "non-binary", Value = "non-binary" }
        };

        public static List<SelectListItem> OrderStatuses = new()
        {
            new SelectListItem { Text = "pending", Value = "pending" },
            new SelectListItem { Text = "shipped", Value = "shipped" },
            new SelectListItem { Text = "delivered", Value = "delivered" }
        };

        public static List<SelectListItem> PaymentMethods = new()
        {
            new SelectListItem { Text = "bank transfer", Value = "bank transfer" },
            new SelectListItem { Text = "paypal", Value = "paypal" },
            new SelectListItem { Text = "credit card", Value = "credit card" }
        };

        public static List<SelectListItem> Categories = new()
        {
            new SelectListItem { Text = "electronics", Value = "electronics" },
            new SelectListItem { Text = "clothing", Value = "clothing" },
            new SelectListItem { Text = "books", Value = "books" },
            new SelectListItem { Text = "home", Value = "home" },
            new SelectListItem { Text = "health", Value = "health" }
        };
    }
}
