using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseManager.Web.Pages.Generators.Payment
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
        public string Amount { get; set; }
        [BindProperty]
        public string PaymentDate { get; set; }
        [BindProperty]
        public string PaymentMethod { get; set; }
        [BindProperty]
        public string GcRecord { get; set; }
        [BindProperty]
        public int GcRecordChance { get; set; }

        public void OnPostGenerate()
        {
            var random = new Random();
            var cSessionId = Guid.NewGuid();
            var customerIdsShard1 = unitOfWork.Customer.GetAllIds(1);
            var customerIdsShard2 = unitOfWork.Customer.GetAllIds(2);
            for (var i = 0; i < Quantity; i++)
            {
                var cPayment = new Models.Payment
                {
                    Amount = GetRandomInt(),
                    PaymentDate = PaymentDate.Equals("Random") ? GetRandomDateTime() : DateTime.Now,
                    PaymentMethod = PaymentMethod.Equals("Random") ? GetRandomString() : GetRandomFromList("preferredPaymentMethods"),
                };
                if (FillOptionalProperties)
                {
                    if (random.Next(0, 100) <= GcRecordChance)
                        cPayment.GcRecord = GcRecord.Equals("Random") ? GetRandomDateTime() : DateTime.Now;
                }
                cPayment.CustomerId = GetRandomFromIds(cPayment.HasGcRecord ? customerIdsShard2 : customerIdsShard1);
                var shardId = cPayment.HasGcRecord ? 2 : 1;
                unitOfWork.Payment.Add(cPayment, cSessionId, shardId);
            }
        }
    }
}
