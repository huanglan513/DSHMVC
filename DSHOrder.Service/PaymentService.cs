using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Service.Interface;
using DSHOrder.Entity;
using DSHOrder.Repository;
using DSHOrder.Common;

namespace DSHOrder.Service
{
    public class PaymentService:IPaymentService
    {
        PaymentRepository repository = null;
        public PaymentService()
        {
            repository = new PaymentRepository();
        }

        #region IPaymentService 成员
        public int AddPayments(IList<Payment> paymentList)
        {
            foreach (Payment payment in paymentList)
            {
                repository.Add<Payment>(payment,true);
            }
            return repository.SaveChanges();
        }

        public Payment AddPayment(Payment entity)
        {
            return repository.Add<Payment>(entity);
        }

        public Payment GetPaymentById(int paymentId)
        {
            return repository.GetBy<Payment>(p => p.PaymentID == paymentId && p.DeleteInd == 0);
        }

        public IList<Payment> GetPaymentsByIds(int[] paymentIdArray)
        {
            return repository.GetAllBy<Payment>(p => paymentIdArray.Contains(p.PaymentID) && p.DeleteInd == 0);
        }

        public int UpdatePayments(IList<Payment> paymentList)
        {
            foreach (Payment payment in paymentList)
            {
                repository.Update<Payment>(payment, true);
            }
            return repository.SaveChanges();
        }

        public Payment UpdatePayment(Payment entity)
        {
            return repository.Update<Payment>(entity);
        }

        public IList<Payment> GetPaymentsByItemID(int groupByItemID)
        {
            return repository.GetAllBy<Payment>(p => p.GroupByItemID == groupByItemID && p.DeleteInd == 0).OrderBy(p=>p.PaymentType).ToList();
        }
        #endregion
    }
}
