using LIMS.Domain.Common;
using LIMS.Domain.Customers;
using LIMS.Domain.Directory;
using LIMS.Domain.Localization;
using LIMS.Domain.Stores;
using LIMS.Web.Models.Common;
using MediatR;
using System;
using System.Collections.Generic;
namespace LIMS.Web.Features.Models.Common
{
    public class GetAddressModel : IRequest<AddressModel>
    {
        public AddressModel Model { get; set; }
        public Address Address { get; set; }
        public bool ExcludeProperties { get; set; }
        public Func<IList<Country>> LoadCountries { get; set; } = null;
        public bool PrePopulateWithCustomerFields { get; set; } = false;
        public Customer Customer { get; set; } = null;
        public Language Language { get; set; }
        public Store Store { get; set; }
        public string OverrideAttributesXml { get; set; } = "";
    }
}
