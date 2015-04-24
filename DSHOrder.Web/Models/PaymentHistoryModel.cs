using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSHOrder.Web.Models
{
    public class PaymentHistoryModel
    {
        public int PaymentType { get; set; }

        public DateTime PaymentDeadline { get; set; }

        public decimal? PaymentProportion { get; set; }

        public decimal? PaymentPrice { get; set; }

        public DateTime? ActualPaymentDeadline { get; set; }

        public decimal? ActualPaymentProportion { get; set; }

        public decimal? ActualPaymentPrice { get; set; }

        public string Action { get; set; }

        public string Result { get; set; }

        public string User { get; set; }

        public DateTime ActionDate { get; set; }

        public string Comment { get; set; }

        public string CertificateImage { get; set; }
        
    }
}