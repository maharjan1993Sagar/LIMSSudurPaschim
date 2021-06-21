using LIMS.Api.DTOs.AnimalBreeding;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.Queries.Models.Common
{
    public class ExitQueryHandler<T>:IRequest<ExitDto>
    {
        public ExitDto Model { get; set; }
    }
}
