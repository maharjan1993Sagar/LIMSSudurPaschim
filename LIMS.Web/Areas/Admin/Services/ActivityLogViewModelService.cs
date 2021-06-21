using LIMS.Services.Customers;
using LIMS.Services.Helpers;
using LIMS.Services.Knowledgebase;
using LIMS.Services.Logging;
using LIMS.Web.Areas.Admin.Extensions;
using LIMS.Web.Areas.Admin.Interfaces;
using LIMS.Web.Areas.Admin.Models.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Services
{
    public partial class ActivityLogViewModelService : IActivityLogViewModelService
    {
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ICustomerService _customerService;
       
      //  private readonly IKnowledgebaseService _knowledgebaseService;
      
        public ActivityLogViewModelService(ICustomerActivityService customerActivityService,
            IDateTimeHelper dateTimeHelper, ICustomerService customerService
          )
        {
            _customerActivityService = customerActivityService;
            _dateTimeHelper = dateTimeHelper;
            _customerService = customerService;
          
        
        }
        public virtual async Task<IList<ActivityLogTypeModel>> PrepareActivityLogTypeModels()
        {
            var model = (await _customerActivityService
                .GetAllActivityTypes())
                .Select(x => x.ToModel())
                .ToList();
            return model;
        }
        public virtual async Task SaveTypes(List<string> types)
        {
            var activityTypes = await _customerActivityService.GetAllActivityTypes();
            foreach (var activityType in activityTypes)
            {
                activityType.Enabled = types.Contains(activityType.Id);
                await _customerActivityService.UpdateActivityType(activityType);
            }
        }
        public virtual async Task<ActivityLogSearchModel> PrepareActivityLogSearchModel()
        {
            var activityLogSearchModel = new ActivityLogSearchModel();
            activityLogSearchModel.ActivityLogType.Add(new SelectListItem
            {
                Value = "",
                Text = "All"
            });
            foreach (var at in await _customerActivityService.GetAllActivityTypes())
            {
                activityLogSearchModel.ActivityLogType.Add(new SelectListItem
                {
                    Value = at.Id.ToString(),
                    Text = at.Name
                });
            }
            return activityLogSearchModel;
        }

        public virtual async Task<(IEnumerable<ActivityLogModel> activityLogs, int totalCount)> PrepareActivityLogModel(ActivityLogSearchModel model, int pageIndex, int pageSize)
        {
            DateTime? startDateValue = (model.CreatedOnFrom == null) ? null
                : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.CreatedOnFrom.Value, _dateTimeHelper.CurrentTimeZone);

            DateTime? endDateValue = (model.CreatedOnTo == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.CreatedOnTo.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);

            var activityLog = await _customerActivityService.GetAllActivities(model.Comment, startDateValue, endDateValue, null, model.ActivityLogTypeId, model.IpAddress, pageIndex - 1, pageSize);
            var activityLogModel = new List<ActivityLogModel>();
            foreach (var item in activityLog)
            {
                var customer = await _customerService.GetCustomerById(item.CustomerId);
                var cas = await _customerActivityService.GetActivityTypeById(item.ActivityLogTypeId);

                var m = item.ToModel();
                m.CreatedOn = _dateTimeHelper.ConvertToUserTime(item.CreatedOnUtc, DateTimeKind.Utc);
                m.ActivityLogTypeName = cas?.Name;
                m.CustomerEmail = customer != null ? customer.Email : "NULL";
                activityLogModel.Add(m);
            }
            return (activityLogModel, activityLog.TotalCount);
        }

        public virtual async Task<(IEnumerable<ActivityStatsModel> activityStats, int totalCount)> PrepareActivityStatModel(ActivityLogSearchModel model, int pageIndex, int pageSize)
        {
            DateTime? startDateValue = (model.CreatedOnFrom == null) ? null
                : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.CreatedOnFrom.Value, _dateTimeHelper.CurrentTimeZone);

            DateTime? endDateValue = (model.CreatedOnTo == null) ? null
                : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.CreatedOnTo.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);

            var activityStat = await _customerActivityService.GetStatsActivities(startDateValue, endDateValue, model.ActivityLogTypeId, pageIndex - 1, pageSize);
            var activityStatModel = new List<ActivityStatsModel>();
            foreach (var x in activityStat)
            {
                var activityLogType = await _customerActivityService.GetActivityTypeById(x.ActivityLogTypeId);
                string _name = "-empty-";
                if (activityLogType != null)
                {
                    IList<string> systemKeywordsCategory = new List<string>();
                    systemKeywordsCategory.Add("PublicStore.ViewCategory");
                    systemKeywordsCategory.Add("EditCategory");
                    systemKeywordsCategory.Add("AddNewCategory");

                   

                    IList<string> systemKeywordsManufacturer = new List<string>();
                    systemKeywordsManufacturer.Add("PublicStore.ViewManufacturer");
                    systemKeywordsManufacturer.Add("EditManufacturer");
                    systemKeywordsManufacturer.Add("AddNewManufacturer");

                  

                    IList<string> systemKeywordsProduct = new List<string>();
                    systemKeywordsProduct.Add("PublicStore.ViewProduct");
                    systemKeywordsProduct.Add("EditProduct");
                    systemKeywordsProduct.Add("AddNewProduct");

                    IList<string> systemKeywordsUrl = new List<string>();
                    systemKeywordsUrl.Add("PublicStore.Url");
                    if (systemKeywordsUrl.Contains(activityLogType.SystemKeyword))
                    {
                        _name = x.EntityKeyId;
                    }

                   

                   
                }

                var m = x.ToModel();
                m.ActivityLogTypeName = activityLogType != null ? activityLogType.Name : "-empty-";
                m.Name = _name;
                activityStatModel.Add(m);
            }
            return (activityStatModel, activityStat.TotalCount);
        }
    }
}
