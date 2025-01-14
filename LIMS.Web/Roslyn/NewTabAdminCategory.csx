﻿#! "netcoreapp3.1"
#r "LIMS.Core"
#r "LIMS.Framework"
#r "LIMS.Services"
#r "LIMS.Web"

using LIMS.Framework.Events;
using LIMS.Services.Events;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

/* Sample code to create new tab on category edit page */
public class AdminTabEvent : INotificationHandler<AdminTabStripCreated>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AdminTabEvent(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public Task Handle(AdminTabStripCreated eventMessage, CancellationToken cancellationToken)
    {
        if (eventMessage.TabStripName == "category-edit")
        {
            var categoryId = Convert.ToString(_httpContextAccessor.HttpContext.GetRouteValue("ID"));
            eventMessage.BlocksToRender.Add(("test new tab", new HtmlString($"<div>TEST{categoryId}</div>")));
        }
        return Task.CompletedTask;
    }
}

