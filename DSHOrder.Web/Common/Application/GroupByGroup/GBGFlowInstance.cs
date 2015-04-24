using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Service.Interface;
using DSHOrder.Service;

namespace DSHOrder.Web.Common.Application.GroupByGroup
{

    public class GBGFlowInstanceBase
    {
        protected Entity.GroupByGroup GroupByGroup = null;

        public GBGFlowInfo GBGFlowInfo { get; set; }
        public GBGFlowEnumFLowStatus CurrentStatus { get; set; }
        public GBGFlowEnumFLowNode CurrentNodeCode { get; set; }


        protected GBGFlowInstanceBase()
        {
            this.GBGFlowInfo = GBGFlowInfo.Instance;
        }

        public virtual void CalcStatus()
        {

        }

        // 计算各个字段只读或可写属性
        protected virtual void CalcFieldStatus()
        {

        }
    }

    public class GBGFlowInstance : GBGFlowInstanceBase
    {
        public GBGFlowInstance(int GroupByGroupId)
            : base()
        {
            IGroupByGroupService service = new GroupByGroupService();
            this.GroupByGroup = service.GetById(GroupByGroupId);
        }

    }


    public class GBGFlowInstanceForItem : GBGFlowInstanceBase
    {
        protected Entity.GroupByItem GroupByItem = null;

        public GBGFlowInstanceForItem(int GroupByItemId)
            : base()
        {
            IGroupByItemService service = new GroupByItemService();
            this.GroupByItem = service.GetById(GroupByItemId);
            this.GroupByGroup = this.GroupByItem.GroupByGroup;
        }

        public override void CalcStatus()
        {
            // base.CalcStatus();
        }

        protected override void CalcFieldStatus()
        {
            // base.CalcFieldStatus();
        }

    }
}