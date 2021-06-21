﻿using LIMS.Core;
using LIMS.Framework.Components;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LIMS.Web.ViewComponents
{
    public class TopMenuViewComponent : BaseViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;

        public TopMenuViewComponent(IMediator mediator,
            IWorkContext workContext,
            IStoreContext storeContext)
        {
            _mediator = mediator;
            _workContext = workContext;
            _storeContext = storeContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var model = await _mediator.Send(new GetTopMenu() {
            //    Customer = _workContext.CurrentCustomer,
            //    Language = _workContext.WorkingLanguage,
            //    Store = _storeContext.CurrentStore
            //});

            return View();
        }
    }
}