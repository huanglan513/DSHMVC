using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Web.Models;
using DSHOrder.Service.Interface;
using DSHOrder.Service;
using DSHOrder.Entity;

namespace DSHOrder.Web.Common.Application.GroupByGroup
{
    public class GBGActions
    {
        public static Dictionary<GBGFlowEnumFLowAction, ActionInfo> AllActions = new Dictionary<GBGFlowEnumFLowAction, ActionInfo>();

        #region private var

        private DateTime dtNow = DateTime.Now;
        private string currentUser = "";
        private int currentUserId = -1;
        private ActionInfo ai = null;

        #endregion

        #region 构造函数

        public GBGActions(GBGFlowEnumFLowAction CurrentAction)
        {
            dtNow = DateTime.Now;
            currentUser = HttpContext.Current.User.Identity.Name;
            currentUserId = GetUserIdByName(currentUser);
            ai = AllActions[CurrentAction];
        }

        #endregion

        #region approve actions

        // 业务员填写团购申请表_提交
        public void YWYTXTGSQB_Submit(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            // ActionInfo ai = GBGFlowInfo.Instance.GetActionInfo(FLowAction);
            ActionInfo ai = AllActions[FlowAction];

            UpdateBaseField(model, ai, true);

            model.GroupByGroup.CurrentNode = ai.NextNode.ToString();
            model.GroupByGroup.CurrentStatus = ai.NextStatus.ToString();

            IGroupByGroupService service = new GroupByGroupService();
            service.Add(model.GroupByGroup);

            InsertAR(model.GroupByGroup.GroupByGroupID, model.GroupByGroup.GroupByGroupName,
                null, null,
                ai.CurrentNode.ToString(), ai.CurrentNodeName,
                ai.Action.ToString(), "", "");
        }

        // 部门经理及总监一审_通过
        public void BMJLJZJYS_Pass(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            BMJLJZJYS(model, FlowAction);
        }
        // 部门经理及总监一审_拒绝 (尚未测试)
        public void BMJLJZJYS_Refuse(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            BMJLJZJYS(model, FlowAction);
        }
        private void BMJLJZJYS(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            var gbg = ReadySaveApprove(model, FlowAction);

            gbg.ApproveBMJLActionTime = dtNow;

            gbg.ApproveBMJLMemo = model.GroupByGroup.ApproveBMJLMemo;

            gbg.ApproveBMJLResult = ai.Action.ToString();
            gbg.ApproveBMJLUserID = currentUserId;

            SaveApprove(gbg, ai, gbg.ApproveBMJLMemo, "");
        }

        public void FKBES_Pass(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            FKBES(model, FlowAction);
        }
        public void FKBES_Refuse(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            FKBES(model, FlowAction);
        }
        private void FKBES(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            var gbg = ReadySaveApprove(model, FlowAction);

            gbg.ApproveFKRBActionTime = dtNow;

            gbg.ApproveFKRBMemo = model.GroupByGroup.ApproveFKRBMemo;

            gbg.ApproveFKRBResult = ai.Action.ToString();
            gbg.ApproveFKRBUserID = currentUserId;

            SaveApprove(gbg, ai, gbg.ApproveFKRBMemo, "");
        }

        public void ZJLSP_Pass(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            ZJLSP(model, FlowAction);
        }
        public void ZJLSP_Refuse(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            ZJLSP(model, FlowAction);
        }
        private void ZJLSP(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            var gbg = ReadySaveApprove(model, FlowAction);

            gbg.ApproveZJLActionTime = dtNow;

            gbg.ApproveZJLMemo = model.GroupByGroup.ApproveZJLMemo;

            gbg.ApproveZJLResult = ai.Action.ToString();
            gbg.ApproveZJLUserID = currentUserId;

            SaveApprove(gbg, ai, gbg.ApproveZJLMemo, "");
        }

        public void XSZLTJTZSP_Submit(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            // todo: 更新所有平台的当前status为 "TZSP"
            XSZLTJTZSP(model, FlowAction);
        }
        public void XSZLTJTZSP_NotSubmit(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            XSZLTJTZSP(model, FlowAction);
        }
        private void XSZLTJTZSP(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            var gbg = ReadySaveApprove(model, FlowAction);

            gbg.ApproveXSZLActionTime = dtNow;

            // todo: 

            // 是否跨城市
            gbg.ApproveXSZLIsCrossCity = model.GroupByGroup.ApproveXSZLIsCrossCity;
            gbg.ApproveXSZLCrossCityID = model.GroupByGroup.ApproveXSZLCrossCityID;
            gbg.ApproveXSZLCrossCityUserID = model.GroupByGroup.ApproveXSZLCrossCityUserID;
            
            // 合同扫描件
            gbg.ApproveXSZLContractFileID = model.GroupByGroup.ApproveXSZLContractFileID;

            gbg.ApproveXSZLMemo = model.GroupByGroup.ApproveXSZLMemo;

            gbg.ApproveXSZLResult = ai.Action.ToString();
            gbg.ApproveXSZLUserID = currentUserId;

            SaveApprove(gbg, ai, gbg.ApproveXSZLMemo, "");
        }

        // 团长审批
        public void TZSP_None(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            TZSP(model, FlowAction);
        }
        public void TZSP_Pass(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            TZSP(model, FlowAction);
        }
        public void TZSP_Refuse(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            TZSP(model, FlowAction);
        }
        private void TZSP(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            var item = ReadySaveApproveItem(model.GroupByItem, FlowAction);

            item.ApproveTZActionTime = dtNow;

            // todo: 
            // 团长处理状态, js中设置

            // 备注
            item.ApproveTZMemo = model.GroupByItem.ApproveTZMemo;
            // 审批结果
            item.ApproveTZResult = ai.Action.ToString();
            // 团购流水号
            item.ApproveTZGroupByFlowNo = model.GroupByItem.ApproveTZGroupByFlowNo;
            // 团长审批时间
            item.ApproveTZProcessTZTime = model.GroupByItem.ApproveTZProcessTZTime;

            item.ApproveTZUserID = currentUserId;

            SaveApproveItem(item, ai, item.ApproveTZMemo, "");
        }

        // 策划部
        public void CHB_Finish(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            CHB(model, FlowAction);
        }
        public void CHB_NotFinish(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            CHB(model, FlowAction);
        }
        private void CHB(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            var item = ReadySaveApproveItem(model.GroupByItem, FlowAction);

            item.ApproveCHBActionTime = dtNow;

            // todo: 
            // 设计状态, js中设置

            // Word
            item.ApproveCHBUploadFileID = model.GroupByItem.ApproveCHBUploadFileID;

            // 备注
            item.ApproveCHBMemo = model.GroupByItem.ApproveCHBMemo;
            // 审批结果
            item.ApproveCHBResult = ai.Action.ToString();

            item.ApproveCHBUserID = currentUserId;

            SaveApproveItem(item, ai, item.ApproveCHBMemo, "");
        }


        // 设计部
        public void SJB_Sure(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            SJB(model, FlowAction);
        }
        public void SJB_NotSure(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            SJB(model, FlowAction);
        }
        private void SJB(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            var item = ReadySaveApproveItem(model.GroupByItem, FlowAction);

            item.ApproveSJBActionTime = dtNow;

            // 备注
            item.ApproveSJBMemo = model.GroupByItem.ApproveSJBMemo;
            // 审批结果
            item.ApproveSJBResult = ai.Action.ToString();

            item.ApproveSJBUserID = currentUserId;

            SaveApproveItem(item, ai, item.ApproveSJBMemo, "");
        }


        // 设计部
        public void XSZLQRTL_Sure(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            XSZLQRTL(model, FlowAction);
        }
        public void XSZLQRTL_NotSure(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            XSZLQRTL(model, FlowAction);
        }
        private void XSZLQRTL(GroupByFlowInfo model, GBGFlowEnumFLowAction FlowAction)
        {
            var item = ReadySaveApproveItem(model.GroupByItem, FlowAction);

            item.ApproveXSZLActionTime = dtNow;

            // 备注
            item.ApproveXSZLMemo = model.GroupByItem.ApproveXSZLMemo;
            // 审批结果
            item.ApproveXSZLResult = ai.Action.ToString();

            item.ApproveXSZLUserID = currentUserId;

            SaveApproveItem(item, ai, item.ApproveXSZLMemo, "");
        }


        #endregion

        #region private method

        private Entity.GroupByGroup ReadySaveApprove(GroupByFlowInfo model, GBGFlowEnumFLowAction FLowAction)
        {
            // DateTime dtNow = DateTime.Now;
            // string strUser = HttpContext.Current.User.Identity.Name;

            ActionInfo ai = AllActions[FLowAction];

            UpdateBaseField(model, ai);

            IGroupByGroupService service = new GroupByGroupService();
            var gbg = service.GetById(model.GroupByGroup.GroupByGroupID);
            gbg.CurrentNode = ai.NextNode.ToString();
            gbg.CurrentStatus = ai.NextStatus.ToString();

            return gbg;
        }
        private Entity.GroupByItem ReadySaveApproveItem(GroupByItem model, GBGFlowEnumFLowAction FLowAction)
        {
            // DateTime dtNow = DateTime.Now;
            // string strUser = HttpContext.Current.User.Identity.Name;

            ActionInfo ai = AllActions[FLowAction];

            // UpdateBaseField(model, ai);

            IGroupByItemService service = new GroupByItemService();
            var item = service.GetById(model.GroupByItemID);
            item.CurrentNode = ai.NextNode.ToString();
            item.CurrentStatus = ai.NextStatus.ToString();
            item.LastModifyBy = this.currentUser;
            item.LastModifyTime = dtNow;

            return item;
        }
        private void SaveApprove(Entity.GroupByGroup gbg, ActionInfo ai, string strMemo, string strMemoMore)
        {
            IGroupByGroupService service = new GroupByGroupService();

            service.Update(gbg);
            
            InsertAR(gbg.GroupByGroupID, gbg.GroupByGroupName,
                null, null,
                ai.CurrentNode.ToString(), ai.CurrentNodeName,
                ai.Action.ToString(), strMemo, strMemoMore);
        }
        private void SaveApproveItem(Entity.GroupByItem item, ActionInfo ai, string strMemo, string strMemoMore)
        {
            IGroupByItemService service = new GroupByItemService();

            service.Update(item);

            InsertAR(item.GroupByGroupID.Value, item.GroupByGroup.GroupByGroupName,
                item.GroupByPortalID, item.GroupByPortal.PortalName,
                ai.CurrentNode.ToString(), ai.CurrentNodeName,
                ai.Action.ToString(), strMemo, strMemoMore);
        }

        private void UpdateBaseField(GroupByFlowInfo model, ActionInfo ai)
        {
            UpdateBaseField(model, ai, false);
        }
        private void UpdateBaseField(GroupByFlowInfo model, ActionInfo ai, bool isCreate)
        {
            DateTime dtNow = DateTime.Now;

            if (isCreate)
            {
                model.GroupByGroup.CreateBy = currentUser;
                model.GroupByGroup.CreateTime = dtNow;

                model.GroupByGroup.GroupByItem.ToList().ForEach(m => { m.CreateBy = currentUser; m.CreateTime = dtNow; });
            }
            else
            {
                model.GroupByGroup.LastModifyBy = currentUser;
                model.GroupByGroup.LastModifyTime = dtNow;

                model.GroupByGroup.GroupByItem.ToList().ForEach(m => { m.LastModifyBy = currentUser; m.LastModifyTime = dtNow; });
            }
        }

        private void InsertAR(
            int GroupByGroupID, string GroupByGroupName,
            int? GroupByPortalID, string PortalName,
            string NodeCode, string NodeName,
            string ApproveResult, string ApproveMemo, string ApproveMemoMore
            )
        {
            if (this.SkipInsertAR(ai.Action)) return;

            ApproveRecordGroupBy ar = new ApproveRecordGroupBy();
            ar.CreateBy = currentUser;
            ar.CreateTime = dtNow;

            ar.LastModifyBy = currentUser;
            ar.LastModifyTime = dtNow;

            ar.DeleteInd = false;
            ar.HandleUserName = currentUser;
            ar.HandleTime = dtNow;
            ar.HandleUserID = currentUserId;

            ar.GroupByGroupID = GroupByGroupID;
            ar.GroupByGroupName = GroupByGroupName;
            ar.GroupByPortalID = GroupByPortalID;
            ar.PortalName = PortalName;
            ar.NodeCode = NodeCode;
            ar.NodeName = NodeName;

            ar.ApproveResult = ApproveResult;
            ar.ApproveMemo = ApproveMemo;
            ar.ApproveMemoMore = ApproveMemoMore;

            IApproveRecordGroupByService service = new ApproveRecordGroupByService();

            service.Add(ar);
        }

        private bool SkipInsertAR(GBGFlowEnumFLowAction FlowAction)
        {
            if (FlowAction == GBGFlowEnumFLowAction.XSZLTJTZSP_NotSubmit) return true;
            if (FlowAction == GBGFlowEnumFLowAction.TZSP_None) return true;
            if (FlowAction == GBGFlowEnumFLowAction.CHB_NotFinish) return true;
            if (FlowAction == GBGFlowEnumFLowAction.SJB_NotSure) return true;
            if (FlowAction == GBGFlowEnumFLowAction.XSZLQRTL_NotSure) return true;

            return false;
        }

        private int GetUserIdByName(string strName)
        {
            IUserService service = new UserService();
            var User = service.GetUserByName(strName);
            return User.UserID;
        }

        #endregion
    }
}