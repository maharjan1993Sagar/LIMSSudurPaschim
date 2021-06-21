//using LIMS.Core;
//using LIMS.Domain.Catalog;
//using LIMS.Domain.Customers;
//using LIMS.Framework.Extensions;
//using LIMS.Services.Customers;
//using LIMS.Services.Helpers;
//using LIMS.Services.Localization;
//using LIMS.Services.Logging;
//using LIMS.Services.Stores;
//using LIMS.Services.Vendors;
//using LIMS.Web.Areas.Admin.Extensions;
//using LIMS.Web.Areas.Admin.Interfaces;
//using LIMS.Web.Areas.Admin.Models.Customers;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace LIMS.Web.Areas.Admin.Services
//{
//    public partial class CustomerTagViewModelService : ICustomerTagViewModelService
//    {
//        private readonly ILocalizationService _localizationService;
//        private readonly ICustomerActivityService _customerActivityService;
      
//        private readonly IStoreService _storeService;
//        private readonly IVendorService _vendorService;
//        private readonly ICustomerTagService _customerTagService;
//        private readonly IDateTimeHelper _dateTimeHelper;

//        public CustomerTagViewModelService(
//           ILocalizationService localizationService,
//           ICustomerActivityService customerActivityService,
         
//           IStoreService storeService,
//           IVendorService vendorService,
//           ICustomerTagService customerTagService,
//           IDateTimeHelper dateTimeHelper)
//        {
//            _localizationService = localizationService;
//            _customerActivityService = customerActivityService;
         
//            _storeService = storeService;
//            _vendorService = vendorService;
//            _customerTagService = customerTagService;
//            _dateTimeHelper = dateTimeHelper;
//        }

//        public virtual CustomerModel PrepareCustomerModelForList(Customer customer)
//        {
//            return new CustomerModel
//            {
//                Id = customer.Id,
//                Email = customer.IsRegistered() ? customer.Email : _localizationService.GetResource("Admin.Customers.Guest"),
//            };
//        }
//        public virtual CustomerTagModel PrepareCustomerTagModel()
//        {
//            var model = new CustomerTagModel();
//            return model;
//        }
//        public virtual async Task<CustomerTag> InsertCustomerTagModel(CustomerTagModel model)
//        {
//            var customertag = model.ToEntity();
//            customertag.Name = customertag.Name.ToLower();
//            await _customerTagService.InsertCustomerTag(customertag);

//            //activity log
//            await _customerActivityService.InsertActivity("AddNewCustomerTag", customertag.Id, _localizationService.GetResource("ActivityLog.AddNewCustomerTag"), customertag.Name);
//            return customertag;
//        }
//        public virtual async Task<CustomerTag> UpdateCustomerTagModel(CustomerTag customerTag, CustomerTagModel model)
//        {
//            customerTag = model.ToEntity(customerTag);
//            customerTag.Name = customerTag.Name.ToLower();

//            await _customerTagService.UpdateCustomerTag(customerTag);

//            //activity log
//            await _customerActivityService.InsertActivity("EditCustomerTage", customerTag.Id, _localizationService.GetResource("ActivityLog.EditCustomerTag"), customerTag.Name);
//            return customerTag;
//        }
//        public virtual async Task DeleteCustomerTag(CustomerTag customerTag)
//        {
//            //activity log
//            await _customerActivityService.InsertActivity("DeleteCustomerTag", customerTag.Id, _localizationService.GetResource("ActivityLog.DeleteCustomerTag"), customerTag.Name);
//            await _customerTagService.DeleteCustomerTag(customerTag);
//        }
//        public virtual async Task<CustomerTagProductModel.AddProductModel> PrepareProductModel(string customerTagId)
//        {
//            var model = new CustomerTagProductModel.AddProductModel();
//            model.CustomerTagId = customerTagId;
//            //categories
//            model.AvailableCategories.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = " " });
//            //var categories = await _categoryService.GetAllCategories(showHidden: true);
//            //foreach (var c in categories)
//            //    model.AvailableCategories.Add(new SelectListItem { Text = _categoryService.GetFormattedBreadCrumb(c, categories), Value = c.Id.ToString() });

//            ////manufacturers
//            //model.AvailableManufacturers.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = " " });
//            //foreach (var m in await _manufacturerService.GetAllManufacturers(showHidden: true))
//            //    model.AvailableManufacturers.Add(new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

//            //stores
//            model.AvailableStores.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = " " });
//            foreach (var s in await _storeService.GetAllStores())
//                model.AvailableStores.Add(new SelectListItem { Text = s.Shortcut, Value = s.Id.ToString() });

//            //vendors
//            model.AvailableVendors.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = " " });
//            foreach (var v in await _vendorService.GetAllVendors(showHidden: true))
//                model.AvailableVendors.Add(new SelectListItem { Text = v.Name, Value = v.Id.ToString() });

//            //product types
//            model.AvailableProductTypes = ProductType.SimpleProduct.ToSelectList().ToList();
//            model.AvailableProductTypes.Insert(0, new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = " " });
//            return model;
//        }
       
//        public virtual async Task InsertProductModel(CustomerTagProductModel.AddProductModel model)
//        {
//            //foreach (string id in model.SelectedProductIds)
//            //{
//            //    var product = await _productService.GetProductById(id);
//            //    if (product != null)
//            //    {
//            //        var customerTagProduct = await _customerTagService.GetCustomerTagProduct(model.CustomerTagId, id);
//            //        if (customerTagProduct == null)
//            //        {
//            //            customerTagProduct = new CustomerTagProduct();
//            //            customerTagProduct.CustomerTagId = model.CustomerTagId;
//            //            customerTagProduct.ProductId = id;
//            //            customerTagProduct.DisplayOrder = 0;
//            //            await _customerTagService.InsertCustomerTagProduct(customerTagProduct);
//            //        }
//            //    }
            
//        }

//    }
//}
