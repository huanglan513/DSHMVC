using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Web.Models;

namespace DSHOrder.Web.Common.Application.GroupByGroup
{
    public class GBGFlowNodeInfo
    {
        private string NodeName { get; set; }
        private Dictionary<GBGFlowEnumFLowAction, ActionInfo> Actions { get; set; }

        public GBGFlowEnumFLowNode FlowNode { get; private set; }
        public List<ActionButton> Buttons { get; private set; }

        public GBGFlowNodePermission Permission { get; set; }

        public GBGFlowNodeInfo(GBGFlowEnumFLowNode Node)
        {
            this.FlowNode = Node;
            this.NodeName = EnumDescriptionAttribute.GetEnumDescription(Node);

            this.Actions = new Dictionary<GBGFlowEnumFLowAction, ActionInfo>();
            this.Buttons = new List<ActionButton>();
        }

        public void AddAction(GBGFlowEnumFLowAction Action, GBGFlowEnumFLowStatus NextStatus, GBGFlowEnumFLowNode NextNode)
        {
            this.AddAction(Action, NextStatus, NextNode, "");
        }
        public void AddAction(GBGFlowEnumFLowAction Action, GBGFlowEnumFLowStatus NextStatus, GBGFlowEnumFLowNode NextNode, string ButtonText)
        {
            ActionInfo ai = new ActionInfo
            {
                Action = Action,
                NextStatus = NextStatus,
                NextNode = NextNode,
                CurrentNode = this.FlowNode,
                CurrentNodeName = this.NodeName
            };

            this.Actions.Add(Action, ai);
            GBGActions.AllActions.Add(Action, ai);

            AddButton(ButtonText, Action);
            //if (!string.IsNullOrEmpty(ButtonText))
            //{
            //    string strButtonType = "submit";
            //    if (Action == GBGFlowEnumFLowAction.None)
            //    {
            //        strButtonType = "button";
            //    }
            //    var button = new ActionButton { ButtonText = ButtonText, FLowAction = Action, ButtonType = strButtonType };
            //    this.Buttons.Add(button);
            //}
        }
        public void AddCancelButton()
        {
            AddButton(" 取 消 ", GBGFlowEnumFLowAction.None);
        }

        private void AddButton(string ButtonText, GBGFlowEnumFLowAction FLowAction)
        {
            if (!string.IsNullOrEmpty(ButtonText))
            {
                string strButtonType = "submit";
                if (FLowAction == GBGFlowEnumFLowAction.None)
                {
                    strButtonType = "button";
                }
                var button = new ActionButton { ButtonText = ButtonText, FLowAction = FLowAction, ButtonType = strButtonType };

                this.Buttons.Add(button);
            }
        }

        public void ExecAction(GBGFlowEnumFLowAction FlowAction, GroupByFlowInfo model)
        {
            if (!this.Actions.ContainsKey(FlowAction))
            {
                throw new Exception("该节点无此动作！");
            }

            GBGActions gbga = new GBGActions(FlowAction);

            switch (FlowAction)
            {
                case GBGFlowEnumFLowAction.None:
                    break;
                case GBGFlowEnumFLowAction.YWYTXTGSQB_Submit:
                    gbga.YWYTXTGSQB_Submit(model, FlowAction);
                    break;
                case GBGFlowEnumFLowAction.BMJLJZJYS_Pass:
                    gbga.BMJLJZJYS_Pass(model, FlowAction);
                    break;
                case GBGFlowEnumFLowAction.BMJLJZJYS_Refuse:
                    gbga.BMJLJZJYS_Refuse(model, FlowAction);
                    break;
                case GBGFlowEnumFLowAction.FKBES_Pass:
                    gbga.FKBES_Pass(model, FlowAction);
                    break;
                case GBGFlowEnumFLowAction.FKBES_Refuse:
                    gbga.FKBES_Refuse(model, FlowAction);
                    break;
                case GBGFlowEnumFLowAction.ZJLSP_Pass:
                    gbga.ZJLSP_Pass(model, FlowAction);
                    break;
                case GBGFlowEnumFLowAction.ZJLSP_Refuse:
                    gbga.ZJLSP_Refuse(model, FlowAction);
                    break;
                case GBGFlowEnumFLowAction.XSZLTJTZSP_Submit:
                    gbga.XSZLTJTZSP_Submit(model, FlowAction);
                    break;
                case GBGFlowEnumFLowAction.XSZLTJTZSP_NotSubmit:
                    gbga.XSZLTJTZSP_NotSubmit(model, FlowAction);
                    break;
                case GBGFlowEnumFLowAction.TZSP_None:
                    gbga.TZSP_None(model, FlowAction);
                    break;
                case GBGFlowEnumFLowAction.TZSP_Pass:
                    gbga.TZSP_Pass(model, FlowAction);
                    break;
                case GBGFlowEnumFLowAction.TZSP_Refuse:
                    gbga.TZSP_Refuse(model, FlowAction);
                    break;
                case GBGFlowEnumFLowAction.CHB_Finish:
                    gbga.CHB_Finish(model, FlowAction);
                    break;
                case GBGFlowEnumFLowAction.CHB_NotFinish:
                    gbga.CHB_NotFinish(model, FlowAction);
                    break;
                case GBGFlowEnumFLowAction.SJB_Sure:
                    gbga.SJB_Sure(model, FlowAction);
                    break;
                case GBGFlowEnumFLowAction.SJB_NotSure:
                    gbga.SJB_NotSure(model, FlowAction);
                    break;
                case GBGFlowEnumFLowAction.XSZLQRTL_Sure:
                    gbga.XSZLQRTL_Sure(model, FlowAction);
                    break;
                case GBGFlowEnumFLowAction.XSZLQRTL_NotSure:
                    gbga.XSZLQRTL_NotSure(model, FlowAction);
                    break;
                default:
                    break;
            }



        }
    }


    public class ActionInfo
    {
        public GBGFlowEnumFLowAction Action { get; set; }

        // 流程的当前节点
        public GBGFlowEnumFLowNode CurrentNode { get; set; }
        // 流程的当前节点名称
        public string CurrentNodeName { get; set; }

        // 执行完这个动作后， 流程状态
        public GBGFlowEnumFLowStatus NextStatus { get; set; }
        // 执行完这个动作后， 流程的当前节点
        public GBGFlowEnumFLowNode NextNode { get; set; }
    }

    public class ActionButton
    {
        public string ButtonType { get; set; }
        public string ButtonText { get; set; }
        public GBGFlowEnumFLowAction FLowAction { get; set; }
    }
}