using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DSHOrder.Service.Interface;
using DSHOrder.Service;
using System.Web.Routing;
using DSHOrder.Web.Models;
using DSHOrder.Common;
using Webdiyer.WebControls.Mvc;
using DSHOrder.Web.Common;

namespace DSHOrder.Web.Controllers
{
    public class UserController : BaseController
    {
        IDepartmentService deptService { set; get; }
        IRoleService roleService { get; set; }
        IUserService userService { get; set; }
        IUserDepartmentService udService { get; set; }
        IUserRoleService urService { get; set; }
        ICityService cityService { get; set; }
        ICustomerService customerService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (deptService == null) { deptService = new DepartmentService(); }
            if (roleService == null) { roleService = new RoleService(); }
            if (userService == null) { userService = new UserService(); }
            if (udService == null) { udService = new UserDepartmentService(); }
            if (urService == null) { urService = new UserRoleService(); }
            if (cityService == null) { cityService = new CityService(); }
            if (customerService == null) { customerService = new CustomerService(); }

            base.Initialize(requestContext);
        }

        public ActionResult AccessDenyView()
        {
            return RedirectToAction("Security", "Home", new { pageUrl = this.Request.RawUrl });
        }

        //
        // GET: /User/
        public ActionResult Index(int? id)
        {
            if (ValidateRole(null) == false) return AccessDenyView();

            ViewBag.Departments = deptService.GetAllDepartments();
            ViewBag.Roles = roleService.GetAllRoles();
            ViewBag.UserNameTtl = "用户名";
            ViewBag.SexTtl = "性别";
            ViewBag.Status = "状态"; 

            id = id ?? 1;
            Pagination paging = new Pagination();
            paging.PageSize = 10;
            paging.CurrentPageIndex = id.Value;
            IList<DSHOrder.Entity.User> list = userService.GetUsersByPaging(paging);
            IList<UserModel> modelList = new List<UserModel>();
            foreach (var user in list)
            {
                UserModel um = new UserModel();
                um.UserName = user.UserName;
                um.UserID = user.UserID;                
                //um.Password = user.Password;
                //um.ConfirmPassword = user.Password;
                um.Title = user.Title;
                um.Sex = user.Sex.HasValue && user.Sex.Value == 1 ? "男" : "女";
                um.Status = user.DeleteInd.HasValue && user.DeleteInd == 1 ? false  : true;

                //Department
                List<int> depts = new List<int>();
                IList<Entity.UserDepartment> uds = udService.GetAllUserDepartments().Where<Entity.UserDepartment>(p=>p.UserID == um.UserID).ToList<Entity.UserDepartment>();

                foreach (var d in uds)
                {
                    depts.Add(d.DepartmentID.Value);
                }
                um.Departments = depts;
                //Role
                Entity.UserRole urTemp = urService.GetRoleByUserID(user.UserID);
                um.RoleId = urTemp == null ? 0 : urTemp.RoleID.Value;

                modelList.Add(um);
            }                     

            PagedList<UserModel> pagedUserList = new PagedList<UserModel>(modelList, id.Value, 10, paging.RowCount.HasValue?paging.RowCount.Value : 0);
            return View(pagedUserList);
        }

        [HttpPost]
        [ActionName("Index")]   
        [MultiButton(Name = "create")]   
        public ActionResult CreateForIndex()
        {
            if (ValidateRole(null) == false) return AccessDenyView();

            return RedirectToAction("Create", "User");
        }

        [HttpPost]
        [ActionName("Index")] 
        [MultiButton(Name = "delete", Argument="ids")]
        public ActionResult DeleteForIndex(string ids)
        {
            if (ValidateRole(null) == false) return AccessDenyView();

            string[] str = ids.Split(',');
            int[] idList = new int[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                idList[i] = Convert.ToInt32(str[i]);
            }
            userService.MarkDelete(idList);
            return RedirectToAction("Index", "User");
        }

        [HttpPost]
        [ActionName("Index")]
        [MultiButton(Name = "edit", Argument = "id")]
        public ActionResult EditForIndex(int id)
        {
            if (ValidateRole(null) == false) return AccessDenyView();

            return RedirectToAction("Edit", "User", new { @id=id});
        }

        public ActionResult Edit(int id)
        {
            if (ValidateRole(null) == false) return AccessDenyView();

            DSHOrder.Entity.User user = userService.GetUserByID(id);

            UserEditModel um = new UserEditModel();
            um.UserName = user.UserName;
            um.UserID = id;
            //um.Password = user.Password;
            //um.ConfirmPassword = user.Password;
            um.Title = user.Title;
            um.CityID = user.CityID;
            um.CustomerID = user.CustomerID;
            um.Sex = user.Sex.HasValue && user.Sex.Value == 1 ? "男" : "女";
            um.Status = user.DeleteInd.HasValue && user.DeleteInd == 1 ? false : true;

            //Department
            List<int> depts = new List<int>();
            foreach (var d in user.UserDepartment)
            {
                depts.Add(d.DepartmentID.Value);
            }
            um.Departments = depts;

            //Role
            Entity.UserRole urTemp = user.UserRole.SingleOrDefault<Entity.UserRole>(p => p.UserID == id);
            um.RoleId = urTemp == null ? 0 : urTemp.RoleID.Value;
            BuildCodeData();

            return View(um);
        }

        [HttpPost]
        public ActionResult Edit(UserEditModel model)
        {
            if (ValidateRole(null) == false) return AccessDenyView();

            BuildCodeData();

            ModelState.Remove("UserName");
            if (ModelState.IsValid)
            {
                //if (String.IsNullOrEmpty(model.UserName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
                //if (String.IsNullOrEmpty(model.Password)) throw new ArgumentException("Value cannot be null or empty.", "password");
                if (model.Departments == null || model.Departments.Count <= 0) throw new ArgumentException("Value cannot be null or empty.", "departments");

                DSHOrder.Entity.User user = userService.GetUserByID(model.UserID);                   
                //user.Password = model.Password;
                user.Title = model.Title;
                user.Sex = model.Sex.Equals("男") ? 1 : 0;
                user.CityID = model.CityID;
                user.CustomerID = model.CustomerID;
                
                //add non-exist
                foreach (int deptId in model.Departments)
                {
                    if (!user.UserDepartment.Any(p => p.DepartmentID == deptId))
                    {
                        user.UserDepartment.Add(new Entity.UserDepartment { DepartmentID = deptId });    
                    }
                }

                //delete exist
                List<int> delsDepts = new List<int>();
                foreach (var item in user.UserDepartment)
                {
                    if (!model.Departments.Contains<int>(item.DepartmentID.Value))
                    {
                        delsDepts.Add(item.UserDepartmentID);
                    }
                }
                foreach (int d in delsDepts)
                {
                    Entity.UserDepartment ud = user.UserDepartment.Single<Entity.UserDepartment>(p => p.UserDepartmentID == d);
                    user.UserDepartment.Remove(ud);
                }

                user.UserRole.SingleOrDefault<Entity.UserRole>(p=>p.UserID == model.UserID).RoleID = model.RoleId;

                DSHOrder.Common.UserManageStatus status = userService.UpdateUser(user);
                if (status == DSHOrder.Common.UserManageStatus.Success)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(status));
                }
            }
            
            return View(model);
        }
                        
        public ActionResult Create()
        {
            if (ValidateRole(null) == false) return AccessDenyView();

            BuildCodeData();
            return View();
        }

        public void BuildCodeData()
        {
            ViewBag.Departments = deptService.GetAllDepartments();
            ViewBag.Roles = roleService.GetAllRoles();

            IList<Entity.City> cityList = cityService.GetAllCities();
            List<SelectListItem> selectedCityList = new List<SelectListItem>();
            //selectedCityList.Add(new SelectListItem() { Text = "城市", Value = "0" });
            foreach (Entity.City item in cityList)
            {
                selectedCityList.Add(new SelectListItem() { Text = item.CityName, Value = item.CityID.ToString() });
            }
            ViewBag.CitySelectList = selectedCityList;

            IList<Entity.Customer> customerList = customerService.GetCustomersByCity();
            List<SelectListItem> selectedCustomerList = new List<SelectListItem>();
            foreach(Entity.Customer c in customerList)
            {
                selectedCustomerList.Add(new SelectListItem(){Text = c.CustomerName, Value = c.CustomerID.ToString()});
            }
            ViewBag.Customers = selectedCustomerList;
        }

        [HttpPost]
        public ActionResult Create(UserModel model)
        {
            if (ValidateRole(null) == false) return AccessDenyView();

            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(model.UserName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
                if (String.IsNullOrEmpty(model.Password)) throw new ArgumentException("Value cannot be null or empty.", "password");
                if (model.Departments == null || model.Departments.Count <= 0) throw new ArgumentException("Value cannot be null or empty.", "departments");

                DSHOrder.Entity.User user = userService.GetUserByName(model.UserName);
                bool isRestore = false;

                if (user != null && user.DeleteInd == 1)
                {
                    user.UserRole.Clear();
                    user.UserDepartment.Clear();
                    user.DeleteInd = 0;
                    isRestore = true;
                }
                else {
                    user = new Entity.User();
                }
                
                user.UserName = model.UserName;
                user.Password = model.Password;
                user.Title = model.Title;
                user.Sex = model.Sex.Equals("男") ? 1 : 0;
                user.CityID = model.CityID;
                user.CustomerID = model.CustomerID;

                foreach (int deptId in model.Departments)
                {
                    user.UserDepartment.Add(new Entity.UserDepartment { DepartmentID = deptId });
                }                         
                
                user.UserRole.Add(new Entity.UserRole { RoleID = model.RoleId });

                DSHOrder.Common.UserManageStatus status = UserManageStatus.Success;
                if (isRestore)
                {
                    status = userService.UpdateUser(user); 
                }
                else
                {
                    status = userService.CreateUser(user);                
                }
                
                if (status == DSHOrder.Common.UserManageStatus.Success)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(status));
                }
            }
            BuildCodeData();
            return View(model);
        }

        private string ErrorCodeToString(DSHOrder.Common.UserManageStatus status)
        {
            switch (status)
            { 
                case DSHOrder.Common.UserManageStatus.Success:
                    return "添加用户成功！";
                case DSHOrder.Common.UserManageStatus.Fail:
                    return "添加用户失败！";
                case DSHOrder.Common.UserManageStatus.DuplicateUser:
                    return "用户已存在！";
                default:
                    return string.Empty;
            }
        }
    }
}
