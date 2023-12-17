using AutoMapper;
using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class PaymentRepository(WebDbContextShard1 webDbContextShard1,WebDbContextShard2 webDbContextShard2, IUnitOfWork unitOfWork) : Repository<Payment>(webDbContextShard1, webDbContextShard2, unitOfWork), 
        IPaymentRepository
    {
        public Payment? Update(Payment cPayment)
        {
            var uPayment = unitOfWork.Payment.GetById(cPayment.Id);
            if (uPayment == null) return null;

            var config = new MapperConfiguration(x => x.CreateMap<Payment, Payment>());
            var mapper = config.CreateMapper();
            mapper.Map(cPayment, uPayment);
            webDbContextShard1.Update(uPayment);
            return uPayment;
        }
    }
}
