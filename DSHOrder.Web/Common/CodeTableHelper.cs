using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Service.Interface;
using DSHOrder.Service;
using DSHOrder.Entity;

namespace DSHOrder.Web.Common
{
    public class CodeTableHelper
    {
        private static IList<CodeTable> CodeTableAll = null;

        public static IList<CodeTable> CodeTableRefundType = null;
        public static IList<CodeTable> CodeTablePaymentType = null;
        public static IList<CodeTable> CodeTableRefundStatus = null;
        public static IList<CodeTable> CodeTableIssueType = null;
        public static IList<CodeTable> CodeTableOrderType = null;
        public static IList<CodeTable> CodeTableGoodType = null;
        public static IList<CodeTable> CodeTableUploadFileType = null;
        public static IList<CodeTable> CodeTableJoinJWY = null;
        public static IList<CodeTable> CodeTableXSZLTJTZSP = null;
        public static IList<CodeTable> CodeTableTZSPJG = null;
        public static IList<CodeTable> CodeTableSJZT = null;
        public static IList<CodeTable> CodeTableSJSFWC = null;
        public static IList<CodeTable> CodeTableTLSFQR = null;


        public static IList<CodeTable> CodeTableSYLB = null;
        public static IList<CodeTable> CodeTableSFZSJJM = null;
        public static IList<CodeTable> CodeTableSFBHMP = null;
        public static IList<CodeTable> CodeTableSFJGWD = null;
        public static IList<CodeTable> CodeTableJTGJ = null;

        static CodeTableHelper()
        {
            ICodeTableService service = new CodeTableService();
            CodeTableAll = service.GetAll();

            CodeTableRefundType = GetCodeTablesByType(ComUtil.CodeType.RefundType);
            CodeTablePaymentType = GetCodeTablesByType(ComUtil.CodeType.PaymentType);
            CodeTableRefundStatus = GetCodeTablesByType(ComUtil.CodeType.RefundStatus);
            CodeTableIssueType = GetCodeTablesByType(ComUtil.CodeType.IssueType);
            CodeTableOrderType = GetCodeTablesByType(ComUtil.CodeType.OrderType);
            CodeTableGoodType = GetCodeTablesByType(ComUtil.CodeType.GoodType);
            CodeTableUploadFileType = GetCodeTablesByType(ComUtil.CodeType.UploadFileType);
            CodeTableJoinJWY = GetCodeTablesByType(ComUtil.CodeType.JoinJWY);

            // 销售助理提交团长审批
            CodeTableXSZLTJTZSP = GetCodeTablesByType(ComUtil.CodeType.XSZLTJTZSP);
            // 团长审批结果
            CodeTableTZSPJG = GetCodeTablesByType(ComUtil.CodeType.TZSPJG);
            // 设计状态
            CodeTableSJZT = GetCodeTablesByType(ComUtil.CodeType.SJZT);


            // 设计是否确认完成
            CodeTableSJSFWC = GetCodeTablesByType(ComUtil.CodeType.SJSFWC);
            // 团链是否确认
            CodeTableTLSFQR = GetCodeTablesByType(ComUtil.CodeType.TLSFQR);


            // 摄影类别
            CodeTableSYLB = GetCodeTablesByType(ComUtil.CodeType.SYLB);
            // 是否赠送假睫毛
            CodeTableSFZSJJM = GetCodeTablesByType(ComUtil.CodeType.SFZSJJM);
            // 是否包含门票
            CodeTableSFBHMP = GetCodeTablesByType(ComUtil.CodeType.SFBHMP);
            // 是否进购物店
            CodeTableSFJGWD = GetCodeTablesByType(ComUtil.CodeType.SFJGWD);
            // 交通工具
            CodeTableJTGJ = GetCodeTablesByType(ComUtil.CodeType.JTGJ);

        }

        private static IList<CodeTable> GetCodeTablesByType(ComUtil.CodeType codeType)
        {
            var listReturn = (from r in CodeTableAll where r.CodeTypeID == (int)codeType select r).ToList();

            return listReturn;
        }

    }
}