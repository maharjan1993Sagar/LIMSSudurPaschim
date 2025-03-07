﻿using LIMS.Domain.Security;
using LIMS.Core.Infrastructure;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;

namespace LIMS.Framework.Security.Honeypot
{
    public static class HtmlExtensions
    {
        public static IHtmlContent GenerateHoneypotInput(this IHtmlHelper helper)
        {
            var sb = new StringBuilder();

            sb.AppendFormat("<div style=\"display:none;\">");
            sb.Append(Environment.NewLine);

            var securitySettings = helper.ViewContext.HttpContext.RequestServices.GetRequiredService<SecuritySettings>();
            sb.AppendFormat("<input id=\"{0}\" name=\"{0}\" type=\"text\">", securitySettings.HoneypotInputName);

            sb.Append(Environment.NewLine);
            sb.Append("</div>");

            return new HtmlString(sb.ToString());
        }
    }
}