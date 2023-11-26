using AutoMapper;
using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;

namespace DatabaseManager.DataAccess.Repository
{
    public class PaymentRepository(WebDbContext webDbContext, IUnitOfWork unitOfWork) : Repository<Payment>(webDbContext, unitOfWork), 
        IPaymentRepository
    {
        public Payment? Update(Payment cPayment)
        {
            var uPayment = unitOfWork.Payment.GetById(cPayment.Id);
            if (uPayment == null) return null;

            var config = new MapperConfiguration(x => x.CreateMap<Payment, Payment>());
            var mapper = config.CreateMapper();
            mapper.Map(cPayment, uPayment);
            webDbContext.Update(uPayment);
            return uPayment;
        }
    }
}
