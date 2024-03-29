﻿using LIMS.Core.Extensions;
using LIMS.Core.Infrastructure;
using LIMS.Core.Plugins;
using LIMS.Core.Routing;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;

namespace LIMS.Framework.Mvc.Routing
{
    /// <summary>
    /// Represents implementation of route publisher
    /// </summary>
    public class RoutePublisher : IRoutePublisher
    {
        #region Fields

        protected readonly ITypeFinder typeFinder;

        #endregion

        #region Ctor

        public RoutePublisher(ITypeFinder typeFinder)
        {
            this.typeFinder = typeFinder;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="routeBuilder">Route builder</param>
        public virtual void RegisterRoutes(IEndpointRouteBuilder routeBuilder)
        {
            //find route providers provided by other assemblies
            var routeProviders = typeFinder.FindClassesOfType<IRouteProvider>();

            //create and sort instances of route providers
            var instances = routeProviders
                .Where(routeProvider => PluginManager.FindPlugin(routeProvider).Return(plugin => plugin.Installed, true)) //ignore not installed plugins
                .Select(routeProvider => (IRouteProvider)Activator.CreateInstance(routeProvider))
                .OrderByDescending(routeProvider => routeProvider.Priority);

            //register all provided routes
            foreach (var routeProvider in instances)
                routeProvider.RegisterRoutes(routeBuilder);
        }

        #endregion
    }
}
