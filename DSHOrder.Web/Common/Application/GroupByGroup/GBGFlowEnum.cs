using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace DSHOrder.Web.Common.Application.GroupByGroup
{

    // ex: EnumDescriptionAttribute.GetEnumDescription(GBGFlowEnumFLowNodeCode.YWY)
    public enum GBGFlowEnumFLowNode
    {
        [EnumDescription("兼容之前的数据")]
        None = 0,

        [EnumDescription("业务员填写团购申请表")]
        YWYTXTGSQB,

        [EnumDescription("部门经理及总监一审")]
        BMJLJZJYS,

        [EnumDescription("风控部二审")]
        FKBES,

        [EnumDescription("总经理审批")]
        ZJLSP,

        [EnumDescription("销售助理提交团长审批")]
        XSZLTJTZSP,

        [EnumDescription("团长审批")]
        TZSP,

        //[EnumDescription("销售助理补开团购合同及排期")]
        //XSZLBKTGHTJPQ,

        [EnumDescription("策划部")]
        CHB,

        [EnumDescription("设计部")]
        SJB,

        [EnumDescription("销售助理确认团链")]
        XSZLQRTL,

        [EnumDescription("完成")]
        END
    }

    public enum GBGFlowEnumFLowAction
    {
        [EnumDescription("")]
        None = 0,

        [EnumDescription("业务员填写团购申请表:提交")]
        YWYTXTGSQB_Submit,

        [EnumDescription("部门经理及总监一审:通过")]
        BMJLJZJYS_Pass,
        [EnumDescription("部门经理及总监一审:拒绝")]
        BMJLJZJYS_Refuse,


        [EnumDescription("风控部二审:通过")]
        FKBES_Pass,
        [EnumDescription("风控部二审:拒绝")]
        FKBES_Refuse,

        [EnumDescription("总经理审批:通过")]
        ZJLSP_Pass,
        [EnumDescription("总经理审批:拒绝")]
        ZJLSP_Refuse,

        [EnumDescription("销售助理提交团长审批:已提交")]
        XSZLTJTZSP_Submit,
        [EnumDescription("销售助理提交团长审批:未提交")]
        XSZLTJTZSP_NotSubmit,

        [EnumDescription("团长审批:未处理")]
        TZSP_None,
        [EnumDescription("团长审批:通过")]
        TZSP_Pass,
        [EnumDescription("团长审批:未通过")]
        TZSP_Refuse,

        //[EnumDescription("销售助理补开团购合同及排期")]
        //XSZLBKTGHTJPQ,

        [EnumDescription("策划部:已完成")]
        CHB_Finish,
        [EnumDescription("策划部:未完成")]
        CHB_NotFinish,

        [EnumDescription("设计部:已确认")]
        SJB_Sure,
        [EnumDescription("设计部:未确认")]
        SJB_NotSure,

        [EnumDescription("销售助理确认团链:已确认")]
        XSZLQRTL_Sure,
        [EnumDescription("销售助理确认团链:未确认")]
        XSZLQRTL_NotSure
    }

    public enum GBGFlowEnumFLowStatus
    {
        [EnumDescription("兼容之前的数据")]
        None = 0,

        [EnumDescription("业务员填写团购申请表_提交_状态")]
        YWYTXTGSQB_Submited,

        [EnumDescription("部门经理及总监一审_通过_状态")]
        BMJLJZJYS_Passed,
        [EnumDescription("部门经理及总监一审_拒绝_状态")]
        BMJLJZJYS_Refused,


        [EnumDescription("风控部二审_通过_状态")]
        FKBES_Passed,
        [EnumDescription("风控部二审_拒绝_状态")]
        FKBES_Refused,

        [EnumDescription("总经理审批_通过_状态")]
        ZJLSP_Passed,
        [EnumDescription("总经理审批_拒绝_状态")]
        ZJLSP_Refused,

        [EnumDescription("销售助理提交团长审批_已提交_状态")]
        XSZLTJTZSP_Submited,
        [EnumDescription("销售助理提交团长审批_未提交_状态")]
        XSZLTJTZSP_NotSubmited,

        [EnumDescription("团长审批_未处理_状态")]
        TZSP_Noned,
        [EnumDescription("团长审批_通过_状态")]
        TZSP_Passed,
        [EnumDescription("团长审批_未通过_状态")]
        TZSP_Refused,

        //[EnumDescription("销售助理补开团购合同及排期")]
        //XSZLBKTGHTJPQ,

        [EnumDescription("策划部_已完成_状态")]
        CHB_Finished,
        [EnumDescription("策划部_未完成_状态")]
        CHB_NotFinished,

        [EnumDescription("设计部_已确认_状态")]
        SJB_Sured,
        [EnumDescription("设计部_未确认_状态")]
        SJB_NotSured,

        [EnumDescription("销售助理确认团链_已确认_状态")]
        XSZLQRTL_Sured,
        [EnumDescription("销售助理确认团链_未确认_状态")]
        XSZLQRTL_NotSured
    }


    public enum GBGFlowEnumPermission
    {
        [EnumDescription("")]
        None = 0,

        [EnumDescription("可读")]
        CanRead,
        [EnumDescription("可编辑")]
        CanWrite
    }


    public enum GBGFlowEnumEditType
    {
        [EnumDescription("单行文本框")]
        Editor,
        [EnumDescription("多行文本框")]
        TextArea,
        [EnumDescription("下拉框")]
        DropDownList,
        [EnumDescription("单选按钮组")]
        CodeTableRadioButtonList
    }

    public enum GBGFlowEnumSearchType
    {
        [EnumDescription("全部团购")]
        Node2Type1 = 1,
        [EnumDescription("进行中的团购")]
        Node2Type2,
        [EnumDescription("往期团购")]
        Node2Type3,
        [EnumDescription("未开始的团购")]
        Node2Type4,

        [EnumDescription("未审批的团购")]
        Node3Type1,
        [EnumDescription("已审批的团购")]
        Node3Type2,

        [EnumDescription("未审批的团购")]
        Node4Type1,
        [EnumDescription("已审批的团购")]
        Node4Type2,

        [EnumDescription("未审批的团购")]
        Node5Type1,
        [EnumDescription("已审批的团购")]
        Node5Type2,

        [EnumDescription("未提交的团购")]
        Node6Type1,
        [EnumDescription("已提交的团购")]
        Node6Type2,

        [EnumDescription("未审批的团购")]
        Node7Type1,
        [EnumDescription("已审批的团购")]
        Node7Type2,

        [EnumDescription("未设计的团购")]
        Node8Type1,
        [EnumDescription("已设计的团购")]
        Node8Type2,

        [EnumDescription("未处理的团购")]
        Node9Type1,
        [EnumDescription("已处理的团购")]
        Node9Type2,

        [EnumDescription("未处理的团购")]
        Node10Type1,
        [EnumDescription("已处理的团购")]
        Node10Type2
    }



    #region EnumDescriptionAttribute

    public class EnumDescriptionAttribute : Attribute
    {
        private string strDescription = "";

        public EnumDescriptionAttribute(string strDescription)
        {
            this.strDescription = strDescription;
        }

        public string Description
        {
            get
            {
                return strDescription;
            }
        }

        public static string GetEnumDescription(Enum enumObj)
        {
            string strReturn = "";

            EnumDescriptionAttribute attr = GetDescriptionAttribute(enumObj);
            if (attr != null)
            {
                strReturn = attr.Description;
            }

            return strReturn;
        }

        public static EnumDescriptionAttribute GetDescriptionAttribute(Enum enumObj)
        {
            EnumDescriptionAttribute enumReturn = null;

            FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());
            object[] attribArray = fieldInfo.GetCustomAttributes(false);
            var attr = (from a in attribArray where a.GetType() == typeof(EnumDescriptionAttribute) select a).Single();
            if (attr != null)
            {
                enumReturn = attr as EnumDescriptionAttribute;
            }

            return enumReturn;
        }

    }

    #endregion

}