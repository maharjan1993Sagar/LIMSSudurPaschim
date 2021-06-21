using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.Queries.Models.Common
{
    public class GetQueryModels<T> : IRequest<IList<T>> where T : class
    {
        public string Id { get; set; }
        public string FarmId { get; set; }
        public string AnimalId { get; set; }
        //public string Id { get; set; }
        //public string CreatedBy { get; set; }
    }
}
