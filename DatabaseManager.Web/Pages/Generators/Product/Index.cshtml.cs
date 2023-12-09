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
            
            var quantity = Quantity;
            var fillOptionalProperties = FillOptionalProperties;
            var name = Name;
            var description = Description;
            var descriptionChance = DescriptionChance;
            var category = Category;
            var categoryChance = CategoryChance;
            var gcRecord = GcRecord;
            var gcRecordChance = GcRecordChance;

            for (var i = 0; i < quantity; i++)
            {
                var cProduct = new Models.Product
                {
                    Name = name.Equals("Random") ? GetRandomString() : GetRandomFromList("productNames"),
                    Price = GetRandomDecimal(),
                    Stock = GetRandomInt()
                };
                if (fillOptionalProperties)
                {
                    if (random.Next(0, 100) <= descriptionChance)
                        cProduct.Description = description.Equals("Random") ? GetRandomString() : GetRandomFromList("productDescription");
                    if (random.Next(0,100) <= categoryChance)
                        cProduct.Category = category.Equals("Random") ? GetRandomString() : GetRandomFromList("productCategories");
                    if (random.Next(0, 100) <= gcRecordChance) 
                        cProduct.GcRecord = gcRecord.Equals("Random") ? GetRandomDateTime() : DateTime.Now;
                }
                unitOfWork.Product.Add(cProduct);
            }
            unitOfWork.SaveChanges();
        }

        private static int GetRandomInt()
        {
            return new Random().Next(1, 9999);
        }

        private static decimal GetRandomDecimal()
        {
            var random = new Random();
            var next = random.NextDouble() * (9999.99 - 0.01) + 0.01;
            return Math.Round((decimal)next, 2);
        }

        private static DateTime GetRandomDateTime()
        {
            var random = new Random();
            var minDate = DateTime.MinValue;
            var range = DateTime.Now - minDate;
            var randomTimeSpan = new TimeSpan((long)(random.NextDouble() * range.Ticks));
            return minDate + randomTimeSpan;
        }

        private static string GetRandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var stringChars = new char[new Random().Next(5, 11)];
            var random = new Random();
            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new string(stringChars);
        }

        private static string GetRandomFromList(string fileName)
        {
            var lines = System.IO.File.ReadAllLines(@$"C:\Users\jiris\source\repos\DatabaseManager\DatabaseManager.Utilities\{fileName}.txt");
            var random = new Random();
            var randomLine = lines[random.Next(lines.Length)];
            return randomLine;
        }
    }
}
