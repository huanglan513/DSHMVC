using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSHOrder.Web.Common.Application.GroupByGroup
{
    public class GBGFlowInfo
    {
        public static GBGFlowInfo Instance { get; set; }
        static GBGFlowInfo()
        {
            Instance = new GBGFlowInfo();
        }

        private Dictionary<GBGFlowEnumFLowNode, GBGFlowNodeInfo> Nodes = null;

        #region GBGFlowInfo

        public GBGFlowInfo()
        {
            this.Nodes = new Dictionary<GBGFlowEnumFLowNode, GBGFlowNodeInfo>();

            AddNode_YWYTXTGSQB();
            AddNode_BMJLJZJYS();
            AddNode_FKBES();
            AddNode_ZJLSP();
            AddNode_XSZLTJTZSP();
            AddNode_TZSP();
            AddNode_CHB();
            AddNode_SJB();
            AddNode_XSZLQRTL();
            AddNode_END();
        }

        #endregion

        #region AddNode

        private void AddNode_YWYTXTGSQB()
        {
            // 业务员填写团购申请表
            GBGFlowNodeInfo node = new GBGFlowNodeInfo(GBGFlowEnumFLowNode.YWYTXTGSQB);
            // 业务员填写团购申请表_提交
            node.AddAction(GBGFlowEnumFLowAction.YWYTXTGSQB_Submit, GBGFlowEnumFLowStatus.YWYTXTGSQB_Submited, GBGFlowEnumFLowNode.BMJLJZJYS, " 提 交 ");
            node.AddCancelButton();

            var Permission = CreateCanReadFieldsPermission();
            int i = 0;
            Permission.AddCanWriteFields(m => m.GroupByGroup.GroupByContractNo);
            Permission.AddCanWriteFields(m => m.GroupByGroup.CustomerID);

            Permission.AddCanWriteFields(m => m.GroupGeneralModel);
            Permission.AddCanWriteFields(m => m.GroupLvYouModel);
            Permission.AddCanWriteFields(m => m.GroupSheYingModel);
            Permission.AddCanWriteFields(m => m.GroupDRDWuLiuModel);

            Permission.AddCanWriteFields(m => m.Sales);
            Permission.AddCanWriteFields(m => m.GroupByGroup.GroupByGroupName);
            Permission.AddCanWriteFields(m => m.GroupByGroup.PriceMarket);
            Permission.AddCanWriteFields(m => m.GroupByGroup.PriceSettlement);
            Permission.AddCanWriteFields(m => m.GroupByGroup.PriceSuggest);
            Permission.AddCanWriteFields(m => m.GroupByGroup.PriceOther);
            Permission.AddCanWriteFields(m => m.GroupByGroup.ShopCount);

            Permission.AddCanWriteFields(m => m.GroupByGroup.ShopNames);
            Permission.AddCanWriteFields(m => m.GroupByGroup.SettlementType);
            Permission.AddCanWriteFields(m => m.GroupByGroup.SettlementDate);
            Permission.AddCanWriteFields(m => m.GroupByGroup.ProductPicture1FileID);
            Permission.AddCanWriteFields(m => m.GroupByGroup.ProductPicture2FileID);
            Permission.AddCanWriteFields(m => m.GroupByGroup.MaterialFileID);
            Permission.AddCanWriteFields(m => m.GroupByGroup.OtherHistryCase);
            Permission.AddCanWriteFields(m => m.GroupByGroup.DPOrKBAddress);

            Permission.AddCanWriteFields(m => m.ListGroupByPortal);
            Permission.AddCanWriteFields(m => m.GroupByItemModel[i]);
            Permission.AddCanWriteFields(m => m.ListCity);

            // 设置可写角色
            Permission.NPOInclude.CanWriteRoles.Add("业务员");
            Permission.NPOInclude.CanWriteRoles.Add("总经理");

            node.Permission = Permission;

            this.Nodes.Add(node.FlowNode, node);
        }
        private void AddNode_BMJLJZJYS()
        {
            // 部门经理及总监一审
            GBGFlowNodeInfo node = new GBGFlowNodeInfo(GBGFlowEnumFLowNode.BMJLJZJYS);
            // 部门经理及总监一审_通过
            node.AddAction(GBGFlowEnumFLowAction.BMJLJZJYS_Pass, GBGFlowEnumFLowStatus.BMJLJZJYS_Passed, GBGFlowEnumFLowNode.FKBES, " 通 过 ");
            // 部门经理及总监一审_拒绝
            node.AddAction(GBGFlowEnumFLowAction.BMJLJZJYS_Refuse, GBGFlowEnumFLowStatus.BMJLJZJYS_Refused, GBGFlowEnumFLowNode.YWYTXTGSQB, " 拒 绝 ");
            node.AddCancelButton();

            var Permission = CreateCanReadFieldsPermission();
            Permission.AddCanWriteFields(m => m.GroupByGroup.ApproveBMJLMemo);
            Permission.AddCanWriteFields(m => m.GroupByGroup.ApproveBMJLActionTime);

            // 设置可写角色
            Permission.NPOInclude.CanWriteRoles.Add("部门经理及总监");

            node.Permission = Permission;

            this.Nodes.Add(node.FlowNode, node);
        }
        private void AddNode_FKBES()
        {
            // 风控部二审
            GBGFlowNodeInfo node = new GBGFlowNodeInfo(GBGFlowEnumFLowNode.FKBES);
            // 风控部二审_通过
            node.AddAction(GBGFlowEnumFLowAction.FKBES_Pass, GBGFlowEnumFLowStatus.FKBES_Passed, GBGFlowEnumFLowNode.ZJLSP, " 通 过 ");
            // 风控部二审_拒绝
            node.AddAction(GBGFlowEnumFLowAction.FKBES_Refuse, GBGFlowEnumFLowStatus.FKBES_Refused, GBGFlowEnumFLowNode.YWYTXTGSQB, " 拒 绝 ");
            node.AddCancelButton();

            var Permission = CreateCanReadFieldsPermission();
            Permission.AddCanWriteFields(m => m.GroupByGroup.ApproveFKRBMemo);
            Permission.AddCanWriteFields(m => m.GroupByGroup.ApproveFKRBActionTime);

            // 设置可写角色
            Permission.NPOInclude.CanWriteRoles.Add("风控部");

            node.Permission = Permission;

            this.Nodes.Add(node.FlowNode, node);
        }
        private void AddNode_ZJLSP()
        {
            // 总经理审批
            GBGFlowNodeInfo node = new GBGFlowNodeInfo(GBGFlowEnumFLowNode.ZJLSP);
            // 总经理审批_通过
            node.AddAction(GBGFlowEnumFLowAction.ZJLSP_Pass, GBGFlowEnumFLowStatus.ZJLSP_Passed, GBGFlowEnumFLowNode.XSZLTJTZSP, " 通 过 ");
            // 总经理审批_拒绝
            node.AddAction(GBGFlowEnumFLowAction.ZJLSP_Refuse, GBGFlowEnumFLowStatus.ZJLSP_Refused, GBGFlowEnumFLowNode.YWYTXTGSQB, " 拒 绝 ");
            node.AddCancelButton();

            var Permission = CreateCanReadFieldsPermission();
            Permission.AddCanWriteFields(m => m.GroupByGroup.ApproveZJLMemo);
            Permission.AddCanWriteFields(m => m.GroupByGroup.ApproveZJLActionTime);

            // 设置可写角色
            Permission.NPOInclude.CanWriteRoles.Add("总经理");

            node.Permission = Permission;

            this.Nodes.Add(node.FlowNode, node);
        }
        private void AddNode_XSZLTJTZSP()
        {
            // 销售助理提交团长审批
            GBGFlowNodeInfo node = new GBGFlowNodeInfo(GBGFlowEnumFLowNode.XSZLTJTZSP);
            // 销售助理提交团长审批_已提交
            node.AddAction(GBGFlowEnumFLowAction.XSZLTJTZSP_Submit, GBGFlowEnumFLowStatus.XSZLTJTZSP_Submited, GBGFlowEnumFLowNode.TZSP);
            // 销售助理提交团长审批_未提交
            node.AddAction(GBGFlowEnumFLowAction.XSZLTJTZSP_NotSubmit, GBGFlowEnumFLowStatus.XSZLTJTZSP_NotSubmited, GBGFlowEnumFLowNode.XSZLTJTZSP, " 保 存 ");
            node.AddCancelButton();

            var Permission = CreateCanReadFieldsPermission();
            Permission.AddCanWriteFields(m => m.Helper_XSZLTJTZSP_SubmitStatus);
            Permission.AddCanWriteFields(m => m.GroupByGroup.ApproveXSZLIsCrossCity);

            Permission.AddCanWriteFields(m => m.GroupByGroup.ApproveXSZLContractFileID);
            Permission.AddCanWriteFields(m => m.GroupByGroup.ApproveXSZLMemo);
            Permission.AddCanWriteFields(m => m.GroupByGroup.ApproveXSZLActionTime);

            // 设置可写角色
            Permission.NPOInclude.CanWriteRoles.Add("销售助理");

            node.Permission = Permission;

            this.Nodes.Add(node.FlowNode, node);
        }
        private void AddNode_TZSP()
        {
            // 团长审批
            GBGFlowNodeInfo node = new GBGFlowNodeInfo(GBGFlowEnumFLowNode.TZSP);
            // 团长审批_未处理
            node.AddAction(GBGFlowEnumFLowAction.TZSP_None, GBGFlowEnumFLowStatus.TZSP_Noned, GBGFlowEnumFLowNode.TZSP, " 保 存 ");
            // 团长审批_通过
            node.AddAction(GBGFlowEnumFLowAction.TZSP_Pass, GBGFlowEnumFLowStatus.TZSP_Passed, GBGFlowEnumFLowNode.CHB);
            // 团长审批_未通过
            node.AddAction(GBGFlowEnumFLowAction.TZSP_Refuse, GBGFlowEnumFLowStatus.TZSP_Refused, GBGFlowEnumFLowNode.YWYTXTGSQB);
            node.AddCancelButton();

            var Permission = CreateCanReadFieldsPermission();

            Permission.AddCanWriteFields(m => m.Helper_TZSP_ApproveResult);
            Permission.AddCanWriteFields(m => m.GroupByItem.ApproveTZMemo);
            Permission.AddCanWriteFields(m => m.GroupByItem.ApproveTZGroupByFlowNo);
            Permission.AddCanWriteFields(m => m.GroupByItem.ApproveTZProcessTZTime);
            Permission.AddCanWriteFields(m => m.GroupByItem.ApproveTZActionTime);

            // 设置可写角色
            Permission.NPOInclude.CanWriteRoles.Add("销售助理");

            node.Permission = Permission;

            this.Nodes.Add(node.FlowNode, node);
        }
        private void AddNode_CHB()
        {
            // 策划部
            GBGFlowNodeInfo node = new GBGFlowNodeInfo(GBGFlowEnumFLowNode.CHB);
            // 策划部_未完成
            node.AddAction(GBGFlowEnumFLowAction.CHB_NotFinish, GBGFlowEnumFLowStatus.CHB_NotFinished, GBGFlowEnumFLowNode.CHB, " 保 存 ");
            // 策划部_已完成
            node.AddAction(GBGFlowEnumFLowAction.CHB_Finish, GBGFlowEnumFLowStatus.CHB_Finished, GBGFlowEnumFLowNode.SJB);
            node.AddCancelButton();

            var Permission = CreateCanReadFieldsPermission();

            Permission.AddCanWriteFields(m => m.GroupByItem.ApproveCHBUploadFileID);
            Permission.AddCanWriteFields(m => m.GroupByItem.ApproveCHBMemo);
            Permission.AddCanWriteFields(m => m.Helper_CHB_Status_17);
            Permission.AddCanWriteFields(m => m.GroupByItem.ApproveCHBActionTime);

            // 设置可写角色
            Permission.NPOInclude.CanWriteRoles.Add("策划部");

            node.Permission = Permission;

            this.Nodes.Add(node.FlowNode, node);
        }
        private void AddNode_SJB()
        {
            // 设计部
            GBGFlowNodeInfo node = new GBGFlowNodeInfo(GBGFlowEnumFLowNode.SJB);
            // 设计部_已确认
            node.AddAction(GBGFlowEnumFLowAction.SJB_Sure, GBGFlowEnumFLowStatus.SJB_Sured, GBGFlowEnumFLowNode.XSZLQRTL);
            // 设计部_未确认
            node.AddAction(GBGFlowEnumFLowAction.SJB_NotSure, GBGFlowEnumFLowStatus.SJB_NotSured, GBGFlowEnumFLowNode.SJB, " 保 存 ");
            node.AddCancelButton();

            var Permission = CreateCanReadFieldsPermission();

            Permission.AddCanWriteFields(m => m.Helper_SJB_Status_18);
            Permission.AddCanWriteFields(m => m.GroupByItem.ApproveSJBMemo);
            Permission.AddCanWriteFields(m => m.GroupByItem.ApproveSJBActionTime);

            // 设置可写角色
            Permission.NPOInclude.CanWriteRoles.Add("设计部");

            node.Permission = Permission;

            this.Nodes.Add(node.FlowNode, node);
        }
        private void AddNode_XSZLQRTL()
        {
            // 销售助理确认团链
            GBGFlowNodeInfo node = new GBGFlowNodeInfo(GBGFlowEnumFLowNode.XSZLQRTL);
            // 销售助理确认团链_已确认
            node.AddAction(GBGFlowEnumFLowAction.XSZLQRTL_Sure, GBGFlowEnumFLowStatus.XSZLQRTL_Sured, GBGFlowEnumFLowNode.END);
            // 销售助理确认团链_未确认
            node.AddAction(GBGFlowEnumFLowAction.XSZLQRTL_NotSure, GBGFlowEnumFLowStatus.XSZLQRTL_NotSured, GBGFlowEnumFLowNode.XSZLQRTL, " 保 存 ");
            node.AddCancelButton();

            var Permission = CreateCanReadFieldsPermission();

            Permission.AddCanWriteFields(m => m.Helper_XSZLQRTL_Status_19);
            Permission.AddCanWriteFields(m => m.GroupByItem.ApproveXSZLMemo);
            Permission.AddCanWriteFields(m => m.GroupByItem.ApproveXSZLActionTime);

            // 设置可写角色
            Permission.NPOInclude.CanWriteRoles.Add("销售助理");

            node.Permission = Permission;

            this.Nodes.Add(node.FlowNode, node);
        }
        private void AddNode_END()
        {
            // 结束
            GBGFlowNodeInfo node = new GBGFlowNodeInfo(GBGFlowEnumFLowNode.END);

            node.Permission = CreateCanReadFieldsPermission();

            this.Nodes.Add(node.FlowNode, node);
        }


        private GBGFlowNodePermission CreateCanReadFieldsPermission()
        {
            // Permission
            int i = 0;
            GBGFlowNodePermission Permission = new GBGFlowNodePermission();
            Permission.AddCanReadFields(m => m.GroupByGroup);
            Permission.AddCanReadFields(m => m.GroupByGroup.Customer);

            Permission.AddCanReadFields(m => m.GroupGeneralModel);
            Permission.AddCanReadFields(m => m.GroupLvYouModel);
            Permission.AddCanReadFields(m => m.GroupSheYingModel);
            Permission.AddCanReadFields(m => m.GroupDRDWuLiuModel);

            Permission.AddCanReadFields(m => m.GroupByItem);

            Permission.AddCanReadFields(m => m.Sales);

            Permission.AddCanReadFields(m => m.Helper_XSZLTJTZSP_SubmitStatus);
            Permission.AddCanReadFields(m => m.Helper_TZSP_ApproveResult);
            Permission.AddCanReadFields(m => m.Helper_CHB_Status_17);
            Permission.AddCanReadFields(m => m.Helper_SJB_Status_18);
            Permission.AddCanReadFields(m => m.Helper_XSZLQRTL_Status_19);

            Permission.AddCanReadFields(m => m.ListGroupByPortal);

            Permission.AddCanReadFields(m => m.GroupByItemModel[i]);
            Permission.AddCanReadFields(m => m.ListCity);

            return Permission;
        }
        #endregion

        #region public method

        public GBGFlowNodeInfo GetNodeInfo(string strEnumNode)
        {
            var e = (GBGFlowEnumFLowNode)Enum.Parse(typeof(GBGFlowEnumFLowNode), strEnumNode);
            return Nodes[e];
        }
        public GBGFlowNodeInfo GetNodeInfo(GBGFlowEnumFLowNode EnumNode)
        {
            return Nodes[EnumNode];
        }

        #endregion
    }
}