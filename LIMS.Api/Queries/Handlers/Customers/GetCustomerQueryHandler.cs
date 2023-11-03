using LIMS.Api.DTOs.Customers;
using LIMS.Api.Extensions;
using LIMS.Api.Queries.Models.Customers;
using LIMS.Services.Customers;
using LIMS.Services.LocalStructure;
using LIMS.Services.MoAMAC;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Queries.Handlers.Customers
{
    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerDto>
    {
        private readonly ICustomerService _customerService;
        private readonly IDolfdService _dolfdService;
        private readonly IVhlsecService _vhlsecService;
        private readonly ILocalLevelService _localLevelService;
        private readonly IMoAMACService _moAMACService;

        public GetCustomerQueryHandler(ICustomerService customerService, 
            IDolfdService dolfdService,
            ILocalLevelService localLevelService,
            IVhlsecService vhlsecService,
            IMoAMACService moAMACService)
        {
            _customerService = customerService;
            _dolfdService = dolfdService;
            _vhlsecService = vhlsecService;
            _localLevelService = localLevelService;
            _moAMACService = moAMACService;
        }

        public async Task<CustomerDto> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerService.GetCustomerById(request.Id);
           
            string org = "";
            var roles = customer.GetCustomerRoleName();
            var c = customer.ToModel();
           if(customer.GetCustomerRoleName().ToList().Contains("DolfdAdmin")|| customer.GetCustomerRoleName().ToList().Contains("AddAdmin"))
            {
                c.OrgNameNepali=_dolfdService.GetDolfdById(customer.EntityId).Result.NameNepali;
                c.OrgAddressNepali = _dolfdService.GetDolfdById(customer.EntityId).Result.AddressNepali;

            }
            if (customer.GetCustomerRoleName().ToList().Contains("MolmacAdmin") )
            {
                c.OrgNameNepali = _moAMACService.GetMoAMACById(customer.EntityId).Result.NameNepali;
                c.OrgAddressNepali = _moAMACService.GetMoAMACById(customer.EntityId).Result.NameNepali;

            }
            else
            {
                c.OrgNameNepali = _vhlsecService.GetVhlsecById(customer.EntityId).Result.NameNepali;
                var vhlsec = await _vhlsecService.GetVhlsecById(customer.EntityId);
                c.OrgAddressNepali =_localLevelService.GetNepaliDistrict(vhlsec.District) ;
            }
            return c;
        }
    }
}
