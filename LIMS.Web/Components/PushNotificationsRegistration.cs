﻿using LIMS.Core;
using LIMS.Domain.Customers;
using LIMS.Domain.PushNotifications;
using LIMS.Framework.Components;
using LIMS.Web.Models.PushNotifications;
using Microsoft.AspNetCore.Mvc;

namespace LIMS.Web.Components
{
    public class PushNotificationsRegistration : BaseViewComponent
    {
        private readonly IWorkContext _workContext;
        private readonly PushNotificationsSettings _pushNotificationsSettings;

        public PushNotificationsRegistration(IWorkContext workContext, PushNotificationsSettings pushNotificationsSettings)
        {
            _workContext = workContext;
            _pushNotificationsSettings = pushNotificationsSettings;
        }

        public IViewComponentResult Invoke()
        {
            var model = new PublicInfoModel();
            model.PublicApiKey = _pushNotificationsSettings.PublicApiKey;
            model.SenderId = _pushNotificationsSettings.SenderId;
            model.AuthDomain = _pushNotificationsSettings.AuthDomain;
            model.ProjectId = _pushNotificationsSettings.ProjectId;
            model.StorageBucket = _pushNotificationsSettings.StorageBucket;
            model.DatabaseUrl = _pushNotificationsSettings.DatabaseUrl;
            model.AppId = _pushNotificationsSettings.AppId;
            if (_pushNotificationsSettings.Enabled)
            {
                if (!_pushNotificationsSettings.AllowGuestNotifications && _workContext.CurrentCustomer.IsGuest())
                    return Content("");

                return View(model);
            }

            return Content("");
        }
    }
}
