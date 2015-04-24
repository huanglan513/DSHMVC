using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DSHOrder.Entity
{
    [MetadataType(typeof(OrderDetailMetaData))]
    public partial class OrderDetail
    {
    }

    public class OrderDetailMetaData
    {
        //[DisplayName("处理日期")]
        //[Required(ErrorMessage = "处理日期不能为空")]
        //[DataType(DataType.Date, ErrorMessage = "处理日期的格式不正确，应为yyyy-MM-dd")]
        //public DateTime? DealTime { get; set; }

        //[DisplayName("退款金额")]
        //[Required(ErrorMessage = "退款金额不能为空")]
        //public decimal? RefundFee { get; set; }
    }

    public enum OrderDetailSearchType
    {
        All = 0,
        Pending = 1,
        Done = 2
    }

    public enum OfflineReturnStatus
    {
        All = 0,
        Pending = 1,
        Done = 2
    }
}
