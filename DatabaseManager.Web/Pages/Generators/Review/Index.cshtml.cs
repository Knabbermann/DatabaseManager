using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseManager.Web.Pages.Generators.Review
{
    public class IndexModel(IUnitOfWork unitOfWork, IConfiguration configuration) : CustomPageModel<IndexModel>(configuration)
    {
        [BindProperty]
        public int Quantity { get; set; }
        [BindProperty]
        public bool FillOptionalProperties { get; set; }
        [BindProperty]
        public bool SetReferences { get; set; }
        [BindProperty]
        public string Rating { get; set; }
        [BindProperty]
        public string ReviewDate { get; set; }
        [BindProperty]
        public string Content { get; set; }
        [BindProperty]
        public int ContentChance { get; set; }
        [BindProperty]
        public string GcRecord { get; set; }
        [BindProperty]
        public int GcRecordChance { get; set; }
        [BindProperty]
        public int CustomerChance { get; set; }

        public void OnPostGenerate()
        {
            var random = new Random();
            var cSessionId = Guid.NewGuid();
            var customerIdsShard1 = new List<Guid>();
            var customerIdsShard2 = new List<Guid>();
            if (SetReferences)
            {
                customerIdsShard1 = unitOfWork.Customer.GetAllIds(1);
                customerIdsShard2 = unitOfWork.Customer.GetAllIds(2);
            }
            
            for (var i = 0; i < Quantity; i++)
            {
                var cReview = new Models.Review
                {
                    Rating = GetRandomInt(1, 5),
                    ReviewDate = ReviewDate.Equals("Random") ? GetRandomDateTime() : DateTime.Now,
                    Content = Content.Equals("Random") ? GetRandomString() : GetRandomFromList("reviewContent")
                };
                if (FillOptionalProperties)
                {
                    if (random.Next(0, 100) <= GcRecordChance)
                        cReview.GcRecord = GcRecord.Equals("Random") ? GetRandomDateTime() : DateTime.Now;
                }

                if (SetReferences)
                {
                    if (random.Next(0, 100) < CustomerChance) 
                        cReview.CustomerId = GetRandomFromIds(cReview.HasGcRecord ? customerIdsShard2 : customerIdsShard1);
                }
                var shardId = cReview.HasGcRecord ? 2 : 1;
                unitOfWork.Review.Add(cReview, cSessionId, shardId);
            }
        }
    }
}
