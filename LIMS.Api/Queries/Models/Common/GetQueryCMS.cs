using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.Queries.Models.Common
{
    public class GetQueryCMS<T> : IRequest<IList<T>> where T : class
    {
        public string  UserId { get; set; }

    }
}
