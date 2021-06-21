using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LIMS.Framework.Events
{
    public class ActionExecutingContextNotification : INotification
    {
        public ActionExecutingContext Context { get; private set; }

        public bool Before { get; private set; }

        public ActionExecutingContextNotification(ActionExecutingContext context, bool before)
        {
            Context = context;
            Before = before;
        }
    }
}
