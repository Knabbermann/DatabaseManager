using DatabaseManager.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseManager.Web.Pages.Generators.Product
{
    public class IndexModel(IUnitOfWork unitOfWork) : CustomPageModel<IndexModel>
    {
        [BindProperty]
        public int Quantity { get; set; }
        [BindProperty]
        public bool FillOptionalProperties { get; set; }
        [BindProperty]
        public bool SetReferences { get; set; }
        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public int DescriptionChance { get; set; }
        [BindProperty]
        public string Category { get; set; }
        [BindProperty]
        public int CategoryChance { get; set; }
        [BindProperty]
        public string GcRecord { get; set; }
        [BindProperty]
        public int GcRecordChance { get; set; }

        public void OnPostGenerate()
        {
            var random = new Random();

            for (var i = 0; i < Quantity; i++)
            {
                var cProduct = new Models.Product
                {
                    Name = Name.Equals("Random") ? GetRandomString() : GetRandomFromList("productNames"),
                    Price = GetRandomDecimal(),
                    Stock = GetRandomInt()
                };
                if (FillOptionalProperties)
                {
                    if (random.Next(0, 100) <= DescriptionChance)
                        cProduct.Description = Description.Equals("Random") ? GetRandomString() : GetRandomFromList("productDescription");
                    if (random.Next(0,100) <= CategoryChance)
                        cProduct.Category = Category.Equals("Random") ? GetRandomString() : GetRandomFromList("productCategories");
                    if (random.Next(0, 100) <= GcRecordChance) 
                        cProduct.GcRecord = GcRecord.Equals("Random") ? GetRandomDateTime() : DateTime.Now;
                }
                unitOfWork.Product.Add(cProduct);
            }
            unitOfWork.SaveChanges();
        }
    }
}
