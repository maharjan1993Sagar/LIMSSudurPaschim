﻿using LIMS.Web.Models.Common;
using MediatR;
using System.Collections.Generic;

namespace LIMS.Web.Features.Models.Common
{
    public class GetStatesProvince : IRequest<IList<StateProvinceModel>>
    {
        public string CountryId { get; set; }
        public bool AddSelectStateItem { get; set; }
    }
}
