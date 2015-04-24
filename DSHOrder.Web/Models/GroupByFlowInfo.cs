using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Entity;
using System.Data.Objects.DataClasses;
using DSHOrder.Service.Interface;
using DSHOrder.Service;
using System.Web.Mvc;
using System.Collections;
using DSHOrder.Web.Common.Application.GroupByGroup;
using FluentValidation.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DSHOrder.Web.Models
{
    public class GroupByFlowInfo
    {
        private int _GroupByGroupId = -1;
        private int _GroupByItemId = -1;

        Lazy<GBGFlowEnumFLowNode> _CurrentNode = null;
        public GBGFlowEnumFLowNode CurrentNode
        {
            get
            {
                return _CurrentNode.Value;
            }
        }

        public string GetCurrentNodeName()
        {
            return GetNodeName(CurrentNode);
        }
        public string GetNodeName(GBGFlowEnumFLowNode node)
        {
            return EnumDescriptionAttribute.GetEnumDescription(node);
        }
        public string GetNodeName(string strNode)
        {
            var node = (GBGFlowEnumFLowNode)Enum.Parse(typeof(GBGFlowEnumFLowNode), strNode);
            return GetNodeName(node);
        }

        public GroupByFlowInfo()
        {
            this.GroupByGroup = new GroupByGroup();
            this.GroupByGroup.CurrentNode = GBGFlowEnumFLowNode.YWYTXTGSQB.ToString();
            this.GroupByGroup.CurrentStatus = GBGFlowEnumFLowNode.None.ToString();

            this.InitLazyObjects();
        }
        public GroupByFlowInfo(int GroupByGroupId)
        {
            IGroupByGroupService service = new GroupByGroupService();
            this.GroupByGroup = service.GetById(GroupByGroupId);

            this._GroupByGroupId = GroupByGroupId;

            this.InitLazyObjects();
        }
        public GroupByFlowInfo(int GroupByGroupId, int GroupByItemId)
        {
            IGroupByItemService service = new GroupByItemService();
            var item = service.GetById(GroupByItemId);

            // 数据合法性验证
            if (item.GroupByGroupID == GroupByGroupId)
            {
                this.GroupByItem = item;
                this.GroupByGroup = item.GroupByGroup;

                this._GroupByGroupId = GroupByGroupId;
                this._GroupByItemId = GroupByItemId;
            }

            this.InitLazyObjects();
        }

        public string GetFLowActionDesc(string strEnum)
        {
            GBGFlowEnumFLowAction action = (GBGFlowEnumFLowAction)Enum.Parse(typeof(GBGFlowEnumFLowAction), strEnum);
            return EnumDescriptionAttribute.GetEnumDescription(action);
        }

        public static string GetCheckBoxChecked_GroupByCity(int CityID, GroupByItem gbi)
        {
            string strReturn = "";
            var list = gbi.GroupByCity.ToList();
            if (list.Exists(m => m.CityID == CityID))
            {
                strReturn = "checked";
            }

            return strReturn;
        }

        public static string GetCheckBox_GroupByCity(GroupByItem gbi)
        {
            string strReturn = "";

            if (gbi != null)
            {
                gbi.GroupByCity.ToList().ForEach(m => strReturn += ";" + m.City.CityName);
            }

            return strReturn.Trim(';', ' ');
        }

        #region all objects for model

        #region objects for model

        public StartFormModel StartForm { get; set; }
        public List<GroupByItem> GroupByItemModel { get; set; }
        public GroupByItem GroupByItem { get; set; }
        public List<Payment> PaymentModel { get; set; }

        [DisplayName("业务员")]
        [Required(ErrorMessage = "业务员不能为空")]
        public string Sales { get; set; }
        public string SalesText { get; set; }

        public GroupByGroup GroupByGroup { get; set; }

        public GroupGeneral GroupGeneralModel { get; set; }
        public GroupLvYou GroupLvYouModel { get; set; }
        public GroupSheYing GroupSheYingModel { get; set; }
        public GroupDRDWuLiu GroupDRDWuLiuModel { get; set; }

        [Required(ErrorMessage = "至少选择一个平台")]
        public string GroupByItemIds { get; set; }

        public string Helper_XSZLTJTZSP_SubmitStatus { get; set; }
        public string Helper_TZSP_ApproveResult { get; set; }
        public string Helper_CHB_Status_17 { get; set; }
        public string Helper_SJB_Status_18 { get; set; }
        public string Helper_XSZLQRTL_Status_19 { get; set; }

        public int IndustryID { get; set; }

        #endregion

        #region objects for model (read only)

        public static SelectList PaymentTypeList
        {
            get
            {
                ICodeTableService codeTableService = new CodeTableService();
                IList<CodeTable> paymentTypeList = codeTableService.GetCodeTablesByType((int)ComUtil.CodeType.PaymentType);
                var list = new SelectList(paymentTypeList, "CodeValue", "CodeDesc");

                return list;
            }
        }
        public static SelectList CityList
        {
            get
            {
                ICityService cityService = new CityService();
                IList<City> cityList = cityService.GetAllCities();

                Entity.City city = new Entity.City();
                city.CityID = -1;
                city.CityName = "请选择城市";
                cityList.Insert(0, city);

                var list = new SelectList(cityList, "CityID", "CityName");

                return list;
            }
        }
        public static SelectList DepartmentList
        {
            get
            {
                IDepartmentService departmentService = new DepartmentService();
                var departmentList = departmentService.GetAllDepartments();

                Entity.Department department = new Entity.Department();
                department.DepartmentID = -1;
                department.DepartmentName = "请选择部门";
                departmentList.Insert(0, department);

                var list = new SelectList(departmentList, "DepartmentID", "DepartmentName");

                return list;
            }
        }
        public static SelectList EmptySelectList
        {
            get
            {
                var al = new ArrayList();
                var list = new SelectList(al);

                return list;
            }
        }

        public SelectList SettlementDateList
        {
            get
            {
                var al = new ArrayList();
                for (int i = 1; i <= 31; i++)
                {
                    al.Add(new { v = i, t = i });
                }
                var list = new SelectList(al, "v", "t");

                return list;
            }
        }

        public IList<GroupByPortal> ListGroupByPortal
        {
            get
            {
                IGroupByPortalService service = new GroupByPortalService();
                return service.GetAllPortals();
            }
        }

        public IList<City> ListCity
        {
            get
            {
                ICityService cityService = new CityService();
                return cityService.GetAllCities();
            }
        }

        public IList<ApproveRecordGroupBy> ThisPortalApproveRecords
        {
            get
            {
                if (this._GroupByGroupId != -1)
                {
                    return this.GroupByGroup.ApproveRecordGroupBy.Where(m =>
                               m.GroupByPortalID == null
                            || (this._GroupByItemId != -1 && m.GroupByPortalID == this.GroupByItem.GroupByPortalID)
                        ).ToList();
                }

                return new List<ApproveRecordGroupBy>();
            }
        }

        private bool _PZYRZ = false;
        [Required(ErrorMessage = "请先认证牌照")]
        public bool PZYRZ { get { return _PZYRZ; } set { _PZYRZ = value; } }

        private bool _LSTGYJY = false;
        [Required(ErrorMessage = "请先检验历史团购")]
        public bool LSTGYJY { get { return _LSTGYJY; } set { _LSTGYJY = value; } }

        public string IndustryViewName
        {
            get
            {
                //1	当日达
                //2	摄影
                //3	娱乐休闲
                //4	旅游票务
                //5	餐饮糕点
                //6	丽人
                //7	其他类
                //8	话费

                //GroupDRDWuLiu 
                //GroupGeneral 
                //GroupLvYou 
                //GroupSheYing 

                string strReturn = "_GroupGeneral";
                int id = this.GroupByGroup.SubIndustryID.Value;
                ISubIndustryService service = new SubIndustryService();
                var si = service.GetByID(id);
                int industryId = si.IndustryID.Value;

                if (industryId == 1)
                {
                    strReturn = "_GroupDRDWuLiu";
                }
                else if (industryId == 7)
                {
                    strReturn = "_GroupGeneral";
                }
                else if (industryId == 4)
                {
                    strReturn = "_GroupLvYou";
                }
                else if (industryId == 2)
                {
                    strReturn = "_GroupSheYing";
                }

                return strReturn;
            }
        }



        #endregion

        #endregion

        #region parse method

        public void ParseToModel()
        {
            // this.GroupByItemModel
            if (this.GroupByItemModel == null)
            {
                this.GroupByItemModel = new List<GroupByItem>();
                foreach (var item in ListGroupByPortal)
                {
                    var gbi = this.GroupByGroup.GroupByItem.ToList().Find(m => m.GroupByPortalID == item.GroupByPortalID);
                    if (gbi == null)
                    {
                        gbi = new GroupByItem();
                        gbi.GroupByPortalID = item.GroupByPortalID;
                    }
                    this.GroupByItemModel.Add(gbi);
                }
            }

            // this.GroupByItemIds
            this.GroupByItemIds = "";
            if (this.GroupByGroup.GroupByItem != null)
            {
                this.GroupByGroup.GroupByItem.ToList().ForEach(s => this.GroupByItemIds += "|" + s.GroupByPortalID.ToString());
                this.GroupByItemIds = this.GroupByItemIds.Trim('|');
            }

            //this.PaymentModel 
            //表结构修改，GroupByGroup 与Payment之间无外键关联了，因此暂时屏蔽
            //  this.PaymentModel = GetListFromEntityCollection(this.GroupByGroup.Payment);

            // this.Sales
            this.Sales = "";
            if (this.GroupByGroup.GroupBySales != null)
            {
                this.GroupByGroup.GroupBySales.ToList().ForEach(s => this.Sales += "|" + s.GroupBySalesID.ToString());
                this.Sales = this.Sales.Trim('|');
            }
            this.SalesText = "";
            if (this.GroupByGroup.GroupBySales != null)
            {
                this.GroupByGroup.GroupBySales.ToList().ForEach(s => this.SalesText += "; " + s.User.UserName);
                this.SalesText = this.SalesText.Trim(';');
            }

            // this.GroupGeneralModel
            if (this.GroupByGroup.GroupGeneral != null && this.GroupByGroup.GroupGeneral.Count > 0)
            {
                this.GroupGeneralModel = this.GroupByGroup.GroupGeneral.ToList()[0];
            }
            if (this.GroupByGroup.GroupLvYou != null && this.GroupByGroup.GroupLvYou.Count > 0)
            {
                this.GroupLvYouModel = this.GroupByGroup.GroupLvYou.ToList()[0];
            }
            if (this.GroupByGroup.GroupSheYing != null && this.GroupByGroup.GroupSheYing.Count > 0)
            {
                this.GroupSheYingModel = this.GroupByGroup.GroupSheYing.ToList()[0];
            }
            if (this.GroupByGroup.GroupDRDWuLiu != null && this.GroupByGroup.GroupDRDWuLiu.Count > 0)
            {
                this.GroupDRDWuLiuModel = this.GroupByGroup.GroupDRDWuLiu.ToList()[0];
            }

            // Helper_XSZLTJTZSP_SubmitStatus
            if (string.IsNullOrEmpty(this.GroupByGroup.ApproveXSZLResult) || this.GroupByGroup.ApproveXSZLResult == GBGFlowEnumFLowAction.XSZLTJTZSP_NotSubmit.ToString())
            {
                this.Helper_XSZLTJTZSP_SubmitStatus = "0";
            }
            else if (this.GroupByGroup.ApproveXSZLResult == GBGFlowEnumFLowAction.XSZLTJTZSP_Submit.ToString())
            {
                this.Helper_XSZLTJTZSP_SubmitStatus = "1";
            }

            if (this.GroupByItem != null)
            {
                // Helper_TZSP_ApproveResult
                if (string.IsNullOrEmpty(this.GroupByItem.ApproveTZResult) || this.GroupByGroup.ApproveXSZLResult == GBGFlowEnumFLowAction.TZSP_None.ToString())
                {
                    this.Helper_TZSP_ApproveResult = "0";
                }
                else if (this.GroupByItem.ApproveTZResult == GBGFlowEnumFLowAction.TZSP_Refuse.ToString())
                {
                    this.Helper_TZSP_ApproveResult = "1";
                }
                else if (this.GroupByItem.ApproveTZResult == GBGFlowEnumFLowAction.TZSP_Pass.ToString())
                {
                    this.Helper_TZSP_ApproveResult = "2";
                }


                // Helper_CHB_Status_17
                if (string.IsNullOrEmpty(this.GroupByItem.ApproveCHBResult) || this.GroupByItem.ApproveCHBResult == GBGFlowEnumFLowAction.CHB_NotFinish.ToString())
                {
                    this.Helper_CHB_Status_17 = "2";
                }
                else if (this.GroupByItem.ApproveCHBResult == GBGFlowEnumFLowAction.CHB_Finish.ToString())
                {
                    this.Helper_CHB_Status_17 = "1";
                }

                // Helper_SJB_Status_18
                if (string.IsNullOrEmpty(this.GroupByItem.ApproveSJBResult) || this.GroupByItem.ApproveSJBResult == GBGFlowEnumFLowAction.SJB_NotSure.ToString())
                {
                    this.Helper_SJB_Status_18 = "2";
                }
                else if (this.GroupByItem.ApproveSJBResult == GBGFlowEnumFLowAction.SJB_Sure.ToString())
                {
                    this.Helper_SJB_Status_18 = "1";
                }

                // Helper_XSZLQRTL_Status_19
                if (string.IsNullOrEmpty(this.GroupByItem.ApproveXSZLResult) || this.GroupByItem.ApproveXSZLResult == GBGFlowEnumFLowAction.XSZLQRTL_NotSure.ToString())
                {
                    this.Helper_XSZLQRTL_Status_19 = "2";
                }
                else if (this.GroupByItem.ApproveXSZLResult == GBGFlowEnumFLowAction.XSZLQRTL_Sure.ToString())
                {
                    this.Helper_XSZLQRTL_Status_19 = "1";
                }
            }
        }
        public void ParseFromModel(FormCollection form)
        {
            // this.GroupByGroup.GroupByItem
            this.GroupByGroup.GroupByItem = new EntityCollection<GroupByItem>();
            this.GroupByItemIds = this.GroupByItemIds.Trim('|');
            var ids = this.GroupByItemIds.Split('|');
            foreach (string portalId in ids)
            {
                var p = this.GroupByItemModel.Find(m => m.GroupByPortalID == int.Parse(portalId));
                this.GroupByGroup.GroupByItem.Add(p);

                // p.GroupByCity
                p.GroupByCity = new EntityCollection<GroupByCity>();

                string strCities = form[string.Concat("City_", portalId)];
                if (!string.IsNullOrEmpty(strCities))
                {
                    var cityIds = strCities.Split(',');

                    foreach (var cid in cityIds)
                    {
                        GroupByCity gbc = new GroupByCity();
                        gbc.CityID = int.Parse(cid);

                        p.GroupByCity.Add(gbc);
                    }
                }
            }

            // this.GroupByGroup.Payment
            //表结构修改，GroupByGroup 与Payment之间无外键关联了，因此暂时屏蔽
            //   this.GroupByGroup.Payment = GetEntityCollectionFromList(this.PaymentModel);

            // this.GroupByGroup.GroupBySales
            if (!string.IsNullOrEmpty(this.Sales))
            {
                this.Sales = this.Sales.Trim('|');
                this.GroupByGroup.GroupBySales = new EntityCollection<GroupBySales>();
                this.Sales.Split('|').ToList().ForEach(id =>
                    {
                        Entity.GroupBySales s = new GroupBySales();
                        s.UserID = int.Parse(id);
                        s.GroupByGroupID = this.GroupByGroup.GroupByGroupID;

                        this.GroupByGroup.GroupBySales.Add(s);
                    }
                );
            }

            // this.GroupByGroup.GroupGeneral
            if (this.GroupGeneralModel != null)
            {
                this.GroupByGroup.GroupGeneral = new EntityCollection<GroupGeneral>();
                this.GroupByGroup.GroupGeneral.Add(this.GroupGeneralModel);
            }
            if (this.GroupLvYouModel != null)
            {
                this.GroupByGroup.GroupLvYou = new EntityCollection<GroupLvYou>();
                this.GroupByGroup.GroupLvYou.Add(this.GroupLvYouModel);
            }
            if (this.GroupSheYingModel != null)
            {
                this.GroupByGroup.GroupSheYing = new EntityCollection<GroupSheYing>();
                this.GroupByGroup.GroupSheYing.Add(this.GroupSheYingModel);
            }
            if (this.GroupDRDWuLiuModel != null)
            {
                this.GroupByGroup.GroupDRDWuLiu = new EntityCollection<GroupDRDWuLiu>();
                this.GroupByGroup.GroupDRDWuLiu.Add(this.GroupDRDWuLiuModel);
            }
        }

        #endregion

        #region private method

        private List<TEntity> GetListFromEntityCollection<TEntity>(EntityCollection<TEntity> ec) where TEntity : class
        {
            var listReturn = new List<TEntity>();

            if (ec != null)
            {
                ec.ToList().ForEach(m => listReturn.Add(m));
            }

            return listReturn;
        }
        private EntityCollection<TEntity> GetEntityCollectionFromList<TEntity>(List<TEntity> l) where TEntity : class
        {
            var ecReturn = new EntityCollection<TEntity>();

            if (l != null)
            {
                l.ForEach(m => ecReturn.Add(m));
            }

            return ecReturn;
        }

        private void InitLazyObjects()
        {
            _CurrentNode = new Lazy<GBGFlowEnumFLowNode>(() =>
            {
                string strNode = this.GroupByGroup.CurrentNode;

                if (strNode == GBGFlowEnumFLowNode.None.ToString()) return GBGFlowEnumFLowNode.None;
                if (strNode == GBGFlowEnumFLowNode.YWYTXTGSQB.ToString()) return GBGFlowEnumFLowNode.YWYTXTGSQB;
                if (strNode == GBGFlowEnumFLowNode.BMJLJZJYS.ToString()) return GBGFlowEnumFLowNode.BMJLJZJYS;
                if (strNode == GBGFlowEnumFLowNode.FKBES.ToString()) return GBGFlowEnumFLowNode.FKBES;
                if (strNode == GBGFlowEnumFLowNode.ZJLSP.ToString()) return GBGFlowEnumFLowNode.ZJLSP;

                if (strNode == GBGFlowEnumFLowNode.XSZLTJTZSP.ToString() && (this.GroupByItem == null || (this.GroupByItem.GroupByItemID <= 0))) return GBGFlowEnumFLowNode.XSZLTJTZSP;

                if (this.GroupByItem != null)
                {
                    strNode = this.GroupByItem.CurrentNode;

                    // todo: 完善后要去掉这个判断
                    if (string.IsNullOrEmpty(strNode) && this.GroupByGroup.CurrentNode == GBGFlowEnumFLowNode.TZSP.ToString())
                    {
                        strNode = this.GroupByGroup.CurrentNode;
                    }
                }

                return (GBGFlowEnumFLowNode)Enum.Parse(typeof(GBGFlowEnumFLowNode), strNode);

            }, true);
        }



        #endregion
    }
}