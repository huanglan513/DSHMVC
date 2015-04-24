using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DSHOrder.Web.Models;
using System.Linq.Expressions;
using DSHOrder.Service.Interface;
using DSHOrder.Entity;
using DSHOrder.Service;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace DSHOrder.Web.Common.Application.GroupByGroup
{
    public class GBGFlowNodePermission
    {
        public static string EveryOne = "EVERYONE";

        private HashSet<string> CanReadFields { get; set; }
        private HashSet<string> CanWriteFields { get; set; }

        public NodeProcessObjects NPOInclude { get; set; }
        public NodeProcessObjects NPOExclude { get; set; }

        #region GBGFlowNodePermission

        public GBGFlowNodePermission()
        {
            NPOInclude = new NodeProcessObjects();
            NPOExclude = new NodeProcessObjects();

            // 默认所有的角色可读
            NPOInclude.CanReadRoles.Add(EveryOne);
            // 所有角色可写
            // NPOInclude.CanReadUsers.Add(EveryOne);
            // 所有用户可写
            // NPOInclude.CanWriteUsers.Add(EveryOne);

            // 设置可读、可写字段
            CanReadFields = new HashSet<string>();
            CanWriteFields = new HashSet<string>();
        }

        #endregion

        public void AddCanReadFields<TValue>(Expression<Func<GroupByFlowInfo, TValue>> field)
        {
            GetFields(field, CanReadFields);
        }
        public void RemoveCanReadFields<TValue>(Expression<Func<GroupByFlowInfo, TValue>> field)
        {
            HashSet<string> hsTemp = new HashSet<string>();
            GetFields(field, hsTemp);

            var tmp = this.CanReadFields.Except(hsTemp);
            this.CanReadFields = new HashSet<string>(tmp);
        }

        public void AddCanWriteFields<TValue>(Expression<Func<GroupByFlowInfo, TValue>> field)
        {
            GetFields(field, CanWriteFields);
        }
        public void RemoveCanWriteFields<TValue>(Expression<Func<GroupByFlowInfo, TValue>> field)
        {
            HashSet<string> hsTemp = new HashSet<string>();
            GetFields(field, hsTemp);

            var tmp = this.CanWriteFields.Except(hsTemp);
            this.CanWriteFields = new HashSet<string>(tmp);
        }


        private void GetFields<TValue>(Expression<Func<GroupByFlowInfo, TValue>> field, HashSet<string> hs)
        {
            string strField = GBGFlowHelper.GetFieldString(field);

            string strEntityObjectName = typeof(EntityObject).FullName;
            if (field.Body.Type.BaseType != null && field.Body.Type.BaseType.FullName == strEntityObjectName)
            {
                string strDataMemberAttributeName = typeof(DataMemberAttribute).FullName;
                var columnProperties = (from p in field.Body.Type.GetProperties()
                                        where p.GetCustomAttributes(false).Any(a =>
                                            {
                                                Attribute attr = a as Attribute;
                                                return attr.GetType().FullName == strDataMemberAttributeName;
                                            })
                                        select p).ToList();

                columnProperties.ForEach(m => hs.Add(string.Concat(strField, ".", m.Name)));
            }
            else
            {
                hs.Add(strField);
            }
        }

        public GBGFlowEnumPermission CalcNodePermissionForUser()
        {
            #region user info (username, role)

            string strUser = HttpContext.Current.User.Identity.Name;
            IUserRoleService service = new UserRoleService();
            UserRole ur = service.GetRoleByUserName(strUser);
            string strRole = ur.Role.RoleName;

            #endregion

            GBGFlowEnumPermission eReturn = GBGFlowEnumPermission.None;

            // 如果无权限编辑的角色或用户中包含了当前用户， 那它一定不会返回 CanWrite
            if (InObjects(strRole, NPOExclude.CanWriteRoles) || InObjects(strUser, NPOExclude.CanWriteUsers))
            {
                eReturn = GBGFlowEnumPermission.None;
            }
            else if (InObjects(strRole, NPOInclude.CanWriteRoles) || InObjects(strUser, NPOInclude.CanWriteUsers))
            {
                return GBGFlowEnumPermission.CanWrite;
            }

            if (InObjects(strRole, NPOExclude.CanReadRoles) || InObjects(strUser, NPOExclude.CanReadUsers))
            {
                eReturn = GBGFlowEnumPermission.None;
            }
            else if (InObjects(strRole, NPOInclude.CanReadRoles) || InObjects(strUser, NPOInclude.CanReadUsers))
            {
                return GBGFlowEnumPermission.CanRead;
            }

            // 程序运行到这里，只可能返回 None
            return eReturn;
        }


        public GBGFlowEnumPermission CalcFieldPermissionForUser<TValue>(Expression<Func<GroupByFlowInfo, TValue>> field)
        {
            #region user info (username, role)

            string strUser = HttpContext.Current.User.Identity.Name;
            IUserRoleService service = new UserRoleService();
            UserRole ur = service.GetRoleByUserName(strUser);
            string strRole = ur.Role.RoleName;

            #endregion

            GBGFlowEnumPermission eReturn = GBGFlowEnumPermission.None;

            string strField = GBGFlowHelper.GetFieldString(field);

            // 如果该字段可编辑
            if (CanWriteFields.Contains(strField))
            {
                // 以下判断该用户是否有编辑权限

                // 如果无权限编辑的角色或用户中包含了当前用户， 那它一定不会返回 CanWrite
                if (InObjects(strRole, NPOExclude.CanWriteRoles) || InObjects(strUser, NPOExclude.CanWriteUsers))
                {
                    eReturn = GBGFlowEnumPermission.None;
                }
                if (InObjects(strRole, NPOInclude.CanWriteRoles) || InObjects(strUser, NPOInclude.CanWriteUsers))
                {
                    return GBGFlowEnumPermission.CanWrite;
                }
            }

            // 如果该字段可查看
            if (CanReadFields.Contains(strField))
            {
                // 以下判断该用户是否有查看权限

                // 如果无权限查看的角色或用户中包含了当前用户， 就直接返回 None
                if (InObjects(strRole, NPOExclude.CanReadRoles) || InObjects(strUser, NPOExclude.CanReadUsers))
                {
                    eReturn = GBGFlowEnumPermission.None;
                }
                if (InObjects(strRole, NPOInclude.CanReadRoles) || InObjects(strUser, NPOInclude.CanReadUsers))
                {
                    return GBGFlowEnumPermission.CanRead;
                }
            }

            // 程序运行到这里，只可能返回 None
            return eReturn;
        }

        private bool InObjects(string strObject, List<string> strObjects)
        {
            return strObjects.Exists(m => m.Trim().ToUpper() == strObject.Trim().ToUpper() || m.Trim().ToUpper() == EveryOne);
        }

    }

    public class GBGField
    {
        private GBGFlowNodePermission _node = null;
        private HtmlHelper<GroupByFlowInfo> _helper = null;
        public GBGField(GBGFlowNodePermission node, HtmlHelper<GroupByFlowInfo> helper)
        {
            this._node = node;
            this._helper = helper;
        }

        public MvcHtmlString ShowField<TValue>(Expression<Func<GroupByFlowInfo, TValue>> field)
        {
            return ShowField(field, GBGFlowEnumEditType.Editor);
        }
        public MvcHtmlString ShowField<TValue>(Expression<Func<GroupByFlowInfo, TValue>> field, GBGFlowEnumEditType editType)
        {
            return ShowField(field, editType, null);
        }
        public MvcHtmlString ShowField<TValue>(Expression<Func<GroupByFlowInfo, TValue>> field, GBGFlowEnumEditType editType, object otherData)
        {
            var e = this._node.CalcFieldPermissionForUser(field);

            MvcHtmlString strReturn = null;

            switch (e)
            {
                case GBGFlowEnumPermission.CanWrite:
                    switch (editType)
                    {
                        case GBGFlowEnumEditType.Editor:
                            strReturn = this._helper.EditorFor(field);
                            break;
                        case GBGFlowEnumEditType.TextArea:
                            strReturn = this._helper.TextAreaFor(field);
                            break;
                        case GBGFlowEnumEditType.DropDownList:
                            SelectList sl = otherData as SelectList;
                            strReturn = this._helper.DropDownListFor(field, sl);
                            break;
                        case GBGFlowEnumEditType.CodeTableRadioButtonList:
                            IList<CodeTable> ct = otherData as IList<CodeTable>;
                            strReturn = this._helper.CodeTableRadioButtonListFor(field, ct);
                            break;
                        default:
                            break;
                    }
                    break;
                case GBGFlowEnumPermission.CanRead:
                    switch (editType)
                    {
                        case GBGFlowEnumEditType.Editor:
                            strReturn = this._helper.DisplayFor(field);
                            break;
                        case GBGFlowEnumEditType.TextArea:
                            strReturn = this._helper.DisplayFor(field);
                            break;
                        case GBGFlowEnumEditType.DropDownList:
                            SelectList sl = otherData as SelectList;
                            strReturn = this.GetStringFromSelectListCodeTable(field, sl);
                            break;
                        case GBGFlowEnumEditType.CodeTableRadioButtonList:
                            IList<CodeTable> ct = otherData as IList<CodeTable>;
                            strReturn = this.GetStringFromCodeTable(field, ct);
                            break;
                        default:
                            break;
                    }
                    break;
                case GBGFlowEnumPermission.None:
                    break;
                default:
                    break;
            }

            return strReturn;
        }


        public GBGFlowEnumPermission CalcField<TValue>(Expression<Func<GroupByFlowInfo, TValue>> field)
        {
            var e = this._node.CalcFieldPermissionForUser(field);
            return e;
        }

        public GBGFlowEnumPermission CalcNode()
        {
            var e = this._node.CalcNodePermissionForUser();
            return e;
        }

        private MvcHtmlString GetStringFromSelectListCodeTable<TValue>(Expression<Func<GroupByFlowInfo, TValue>> field, SelectList sl)
        {
            string strValue = this._helper.DisplayFor(field).ToString();
            var tmp = from r in sl where r.Value == strValue select r.Text;

            foreach (var item in sl)
            {
                System.Diagnostics.Debug.WriteLine(string.Concat(item.Value, ", ", item.Text));
            }
            return new MvcHtmlString(tmp.SingleOrDefault());
        }

        private MvcHtmlString GetStringFromCodeTable<TValue>(Expression<Func<GroupByFlowInfo, TValue>> field, IList<CodeTable> ct)
        {
            string strValue = this._helper.DisplayFor(field).ToString();
            var tmp = from r in ct where r.CodeValue == strValue select r.CodeDesc;
            return new MvcHtmlString(tmp.SingleOrDefault());
        }
    }



    // 结点处理对象（人、角色）
    public class NodeProcessObjects
    {
        public List<string> CanReadRoles { get; set; }
        public List<string> CanReadUsers { get; set; }
        public List<string> CanWriteRoles { get; set; }
        public List<string> CanWriteUsers { get; set; }

        public NodeProcessObjects()
        {
            CanReadRoles = new List<string>();
            CanReadUsers = new List<string>();
            CanWriteRoles = new List<string>();
            CanWriteUsers = new List<string>();
        }
    }
}