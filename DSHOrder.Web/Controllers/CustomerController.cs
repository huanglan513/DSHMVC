using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DSHOrder.Service.Interface;
using DSHOrder.Entity;
using DSHOrder.Service;
using DSHOrder.Common;
using Webdiyer.WebControls.Mvc;
using DSHOrder.Web.Models;

namespace DSHOrder.Web.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly ICustomerService customerService;
        private readonly ICityService cityService;
        private readonly ICodeTableService codeTableService;
        private readonly IIndustryService industryService;


        public CustomerController(ICustomerService customerService, ICityService cityService, ICodeTableService codeTableService,IIndustryService industryService)
        {
            this.customerService = customerService;
            this.cityService = cityService;
            this.codeTableService = codeTableService;
            this.industryService = industryService;
        }

        public ActionResult Index(int? id, string customerNameSearch, string cityIDSearch, string isCertifiedSearch, int ViewType = 0)
        {
            if (ValidateRole(null) == false) return AccessDenyView();
            
            CustomerSearchModel model = new CustomerSearchModel();
            string query = Request.UrlReferrer==null ? "" : Request.UrlReferrer.Query;
            model.CustomerNameSearch = customerNameSearch == null ? "" : customerNameSearch;
            model.CityIDSearch = string.IsNullOrEmpty(cityIDSearch) ? 0 : int.Parse(cityIDSearch);
            model.IsCertifiedSearch = string.IsNullOrEmpty(isCertifiedSearch) ? 100 : int.Parse(isCertifiedSearch);
            model.ViewType = ViewType;
            //if (Request.Url.Query.IndexOf("ViewType") == -1 && query.IndexOf("ViewType") > 0)   //当切换到第二个Tab时，点击查询会不知道选择哪个Tab，此处为了记住选择的Tab
            //{
            //    model.ViewType = int.Parse(query.Substring(query.LastIndexOf("=") + 1));
            //}
            //else
            //{
                
         //   }

          
            ViewData["citySelectList"] = LoadCityList(model.CityIDSearch,true);
            ViewData["certifiedList"] = LoadCertifiedList();

            Pagination paging = new Pagination();
            paging.PageSize = 10;
            paging.CurrentPageIndex = id ?? 1;

            IList<CustomerExtend> list = customerService.GetCustomersByNameAndCity(paging, model.CustomerNameSearch, model.CityIDSearch,model.ViewType,model.IsCertifiedSearch);
            model.CustomerList = new PagedList<CustomerExtend>(list, id ?? 1, 10, paging.RowCount.Value);
            return View(model);
        }

        private List<SelectListItem> LoadCityList(int selectedValue,bool existAll)
        {
            IList<City> cityList = cityService.GetAllCities();
            List<SelectListItem> selectedCityList = new List<SelectListItem>();
            if (existAll)
            {
                selectedCityList.Add(new SelectListItem() { Text = "城市", Value = "0" });
            }
            foreach (City item in cityList)
            {
                if (item.CityID == selectedValue)
                {
                    selectedCityList.Add(new SelectListItem() { Text = item.CityName, Value = item.CityID.ToString(), Selected = true });
                }
                else
                {
                    selectedCityList.Add(new SelectListItem() { Text = item.CityName, Value = item.CityID.ToString() });
                }
            }
            return selectedCityList;
        }

        private List<SelectListItem> LoadCertifiedList()
        {
            List<SelectListItem> selectedCertifiedList = new List<SelectListItem>();
            selectedCertifiedList.Add(new SelectListItem() { Text = "全部", Value = "100", Selected = true });
            selectedCertifiedList.Add(new SelectListItem() { Text = "未认证", Value = "0" });
            selectedCertifiedList.Add(new SelectListItem() { Text = "已认证", Value = "1" });
            return selectedCertifiedList;
        }

        public ActionResult AccessDenyView()
        {
            return RedirectToAction("Security", "Home", new { pageUrl = this.Request.RawUrl });
        }

        public ActionResult Details(int id,int? pageIndex)
        {
            CustomerDetailModel model = new CustomerDetailModel();

            Pagination paging = new Pagination();
            paging.PageSize = 10;
            paging.CurrentPageIndex = pageIndex ?? 1;

            Customer customer = customerService.GetCustomerByID(id);
            List<CooperationRecord> history = customerService.GetCooperationHistory(id, paging);
            model.CustomerInfo = customer;
            model.CooperationHistory = new PagedList<CooperationRecord>(history, pageIndex ?? 1, 10, paging.RowCount.Value);

            CodeTable ct=codeTableService.GetCodeTableByID(customer.DefaultPaymentType.Value);
            if (ct != null)
            {
                ViewData["paymentType"] = ct.CodeDesc;
            }
            SubIndustry subIndustry=customer.SubIndustry;
            if (subIndustry != null)
            {
                ViewData["SubIndustryName"] = subIndustry.SubIndustryName;
                ViewData["IndustryName"] = subIndustry.Industry.IndustryName;
            }
            return View(model);
        }

        //
        // GET: /Customer/Create

        public ActionResult Create(int id, string name)
        {
            if (id > 0)
            {
                Customer customer = customerService.GetCustomerByID(id);
                SetInitData(customer);
                return View(customer);
            }
            else
            {
                SetInitData(null);  
                return View();
            }
           
        }
   
        //
        // POST: /Customer/Create

        [HttpPost]
        public ActionResult Create(Customer model,FormCollection collection)
        {
            try
            {
                if (!string.IsNullOrEmpty(collection["SubIndustryList"]))
                {
                    model.SubIndustryID = int.Parse(collection["SubIndustryList"]);
                }
                model.BusinessLisenceImg = collection["txtImgBusinessLisenceImg"];
                model.TaxRegisterCertificateImg = collection["txtImgTaxRegisterCertificateImg"];
                model.HealthPermitImg = collection["txtImgHealthPermitImg"];
                model.LisenceImg = collection["txtImgLisenceImg"];
                model.OtherProfQualificationImg = collection["txtImgOtherProfQualificationImg"];
                model.PowerofAttorneyImg = collection["txtImgPowerofAttorneyImg"];
                model.ExternalEnvImg = collection["txtImgExternalEnvImg"];
                model.InternalEnvImg = collection["txtImgInternalEnvImg"];
                               
                if (ModelState.IsValid)
                {
                    if (model.CustomerID > 0)
                    {
                        model.LastModifyBy = this.User.Identity.Name;
                        model.LastModifyTime = DateTime.Now;
                    }
                    else
                    {
                        model.CreateBy = this.User.Identity.Name;
                        model.CreateTime = DateTime.Now;
                    }

                    customerService.Update(model);
                    if (!string.IsNullOrEmpty(Request.QueryString["name"]) && Request.QueryString["name"].IndexOf("G") > -1)
                    {
                        return RedirectToAction("Create", "GroupBy", new { id = Request.QueryString["name"].Substring(1) });
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    SetInitData(null);
                    return View(model);
                }
            }
            catch
            {
                SetInitData(null);
                return View();
            }
        }


        //
        // GET: /Customer/Edit/5
 
        public ActionResult Edit(int id)
        {
            Customer customer=customerService.GetCustomerByID(id);
            SetInitData(customer);
            return View(customer);
        }

        //
        // POST: /Customer/Edit/5

        [HttpPost]
        public ActionResult Edit(Customer model, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.LastModifyBy = this.User.Identity.Name;
                    model.LastModifyTime = DateTime.Now;
                    customerService.Update(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    Customer customer = customerService.GetCustomerByID(model.CustomerID);
                    SetInitData(customer);
                    return View(customer);
                }
            }
            catch
            {
                Customer customer = customerService.GetCustomerByID(model.CustomerID);
                SetInitData(customer);
                return View(customer);
            }
        }

        //
        // GET: /Customer/Delete/5

        public ActionResult Delete()
        {
            return View();
        }

        //
        // POST: /Customer/Delete/5

        [HttpPost]
        public int Delete(string selectedIds)
        {
             string[] idArray = selectedIds.Split(',');
            int[] idIntArray = new int[idArray.Length];
            for(int i=0;i<idArray.Length;i++)
            {
                idIntArray[i] = int.Parse(idArray[i]);
            }
            var r = customerService.MarkDeleted(idIntArray);
            return r;
            // return RedirectToAction("Index");

        }

        public bool ExistCustomerName(string customerName, int customerId)
        {
            ICustomerService customerService = new CustomerService();
            return customerService.ExistCustomer(customerName.Trim(),customerId);
        }

        private void SetInitData(Customer customer)
        {
            if (customer == null)
            {
                ViewBag.Title = "新增商家信息";
            }
            else
            {
                ViewBag.Title = "修改商家信息";
            }
            IList<City> cityList = cityService.GetAllCities();
            if (customer!=null && customer.CityID.HasValue)
            {
                ViewData["citySelectList"] = new SelectList(cityList, "CityID", "CityName", customer.CityID);
            }
            else
            {
                ViewData["citySelectList"] = new SelectList(cityList, "CityID", "CityName");
            }

            if (cityList.Count > 0)
            {
                City city;
                if (customer == null)
                {
                    city = cityList[0];
                    ViewData["districtSelectList"] = new SelectList(city.District, "DistrictID", "DistrictName");
                }
                else
                {
                    city = cityList.First(p => p.CityID == customer.CityID);
                    ViewData["districtSelectList"] = new SelectList(city.District, "DistrictID", "DistrictName", customer.DistrictID);
                }
            }
            else
            {
                ViewData["districtSelectList"] = null;
            }

            IList<Industry> industryList = industryService.GetAllIndustries();
            ViewData["IndustryList"] = new SelectList(industryList, "IndustryID", "IndustryName");

            if (customer!=null && customer.SubIndustryID.HasValue)
            {
                IList<SubIndustry> subIndustryList = GetSubIndustryListBySubIndustryId(customer.SubIndustryID.Value);
                if (subIndustryList.Count > 0)
                {
                    ViewData["IndustryList"] = new SelectList(industryList, "IndustryID", "IndustryName", subIndustryList[0].IndustryID);
                }
                ViewData["SubIndustryList"] = new SelectList(subIndustryList, "SubIndustryID", "SubIndustryName");
            }
            else if(industryList.Count>0)
            {
                IList<SubIndustry> subIndustryList = GetSubIndustryList(industryList[0].IndustryID);
                ViewData["SubIndustryList"] = new SelectList(subIndustryList, "SubIndustryID", "SubIndustryName");
            }

            IList<CodeTable> paymentTypeList = codeTableService.GetCodeTablesByType((int)ComUtil.CodeType.PaymentType);
            if (customer == null)
            {
                ViewData["paymentTypeSelectList"] = new SelectList(paymentTypeList, "CodeID", "CodeDesc");
            }
            else
            {
                ViewData["paymentTypeSelectList"] = new SelectList(paymentTypeList, "CodeID", "CodeDesc", customer.DefaultPaymentType);
            }
            ViewData["MonthPayment"] = paymentTypeList.FirstOrDefault(p => p.CodeDesc == "月结").CodeID;

             List<SelectListItem> MonthDayList=new List<SelectListItem>();
             for (int i = 1; i < 30; i++)
             {
                 MonthDayList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
             }
             MonthDayList.Add(new SelectListItem() { Text = "月末最后一天", Value = "-1" });

             ViewData["MonthDayList"] = MonthDayList;
           
        }

        public JsonResult GetDistrict(int cityID)
        {
            IDistrictService service = new DistrictService();
            IList<District> list = service.GetDistrictByCity(cityID);
            if (list.Count > 0)
            {
                return Json(new SelectList(list, "DistrictID", "DistrictName"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        public IList<SubIndustry> GetSubIndustryList(int industryID)
        {
            ISubIndustryService service = new SubIndustryService();
            IList<SubIndustry> list = service.GetSubIndustriesByIndustryID(industryID);
            return list;
        }

        public JsonResult GetSubIndustry(int industryID)
        {
            IList<SubIndustry> list = GetSubIndustryList(industryID);
            return Json(new SelectList(list, "SubIndustryID", "SubIndustryName"), JsonRequestBehavior.AllowGet);

        }

        public IList<SubIndustry> GetSubIndustryListBySubIndustryId(int subIndustryID)
        {
            ISubIndustryService service = new SubIndustryService();
            return service.GetSubIndustriesBySubIndustryID(subIndustryID);

         }

     }
}
