using DatabaseManager.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseManager.Web.Pages.Generators.Order
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
        public string TotalAmount { get; set; }
        [BindProperty]
        public string OrderDate { get; set; }
        [BindProperty]
        public string Status { get; set; }
        [BindProperty]
        public string GcRecord { get; set; }
        [BindProperty]
        public int GcRecordChance { get; set; }

        public void OnPostGenerate()
        {
            var random = new Random();
            var customerIds = unitOfWork.Customer.GetAllIds();
            for (var i = 0; i < Quantity; i++)
            {
                var cOrder = new Models.Order
                {
                    TotalAmount = GetRandomInt(),
                    OrderDate = OrderDate.Equals("Random") ? GetRandomDateTime() : DateTime.Now,
                    Status = Status.Equals("Random") ? GetRandomString() : GetRandomFromList("orderStatus"),
                    CustomerId = GetRandomFromIds(customerIds)
                };
                if (FillOptionalProperties)
                {
                    if (random.Next(0, 100) <= GcRecordChance)
                        cOrder.GcRecord = GcRecord.Equals("Random") ? GetRandomDateTime() : DateTime.Now;
                }
                unitOfWork.Order.Add(cOrder);
            }
        }
    }
}
