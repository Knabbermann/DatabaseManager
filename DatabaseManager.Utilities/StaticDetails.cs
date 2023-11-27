﻿using Microsoft.AspNetCore.Mvc.Rendering;

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
    }
}
