﻿using LIMS.Core.Routing;
using LIMS.Web.Areas.Admin.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace LIMS.Web.Areas.Admin.Infrastructure
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IEndpointRouteBuilder routeBuilder)
        {
            //admin index
            routeBuilder.MapControllerRoute("AdminIndex", $"admin/", new { controller = "Dashboard", action = "Cdms", area = Constants.AreaAdmin });

            //admin login
            routeBuilder.MapControllerRoute("AdminLogin", $"admin/login/", new { controller = "Login", action = "Index", area = Constants.AreaAdmin });

        }
        public int Priority => 0;

    }
}
