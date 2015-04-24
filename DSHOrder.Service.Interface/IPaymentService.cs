using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;

namespace DSHOrder.Service.Interface
{
    public interface IPaymentService
    {
        int AddPayments(IList<Payment> paymentList);

        Payment AddPayment(Payment entity);

        Payment GetPaymentById(int paymentId);

        IList<Payment> GetPaymentsByIds(int[] paymentIdArray);

        int UpdatePayments(IList<Payment> paymentList);

        Payment UpdatePayment(Payment entity);

        IList<Payment> GetPaymentsByItemID(int groupByItemID);

    }
}
