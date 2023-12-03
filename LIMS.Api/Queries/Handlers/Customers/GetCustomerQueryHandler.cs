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
        private readonly INlboService _nlboService;

        public GetCustomerQueryHandler(ICustomerService customerService, 
            IDolfdService dolfdService,
            ILocalLevelService localLevelService,
            IVhlsecService vhlsecService,
            IMoAMACService moAMACService,
            INlboService nlboService)
        {
            _customerService = customerService;
            _dolfdService = dolfdService;
            _vhlsecService = vhlsecService;
            _localLevelService = localLevelService;
            _moAMACService = moAMACService;
            _nlboService = nlboService;
        }

        public async Task<CustomerDto> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerService.GetCustomerById(request.Id);

            string org = "";
            var roles = customer.GetCustomerRoleName();
            var c = customer.ToModel();
            if (customer.GetCustomerRoleName().ToList().Contains("DolfdAdmin") || customer.GetCustomerRoleName().ToList().Contains("AddAdmin"))
            {
                c.OrgNameNepali = _dolfdService.GetDolfdById(customer.EntityId).Result.NameNepali;
                c.OrgAddressNepali = _dolfdService.GetDolfdById(customer.EntityId).Result.AddressNepali;

            }
            else if (customer.GetCustomerRoleName().ToList().Contains("NlboAdmin"))
            {
                c.OrgNameNepali = _nlboService.GetNlboById(customer.EntityId).Result.NameNepali;
                c.OrgAddressNepali = _nlboService.GetNlboById(customer.EntityId).Result.Address;
            }
            else if (customer.GetCustomerRoleName().ToList().Contains("MolmacAdmin") )
            {
                c.OrgNameNepali = _moAMACService.GetMoAMACById(customer.EntityId).Result.NameNepali;
                c.OrgAddressNepali = _moAMACService.GetMoAMACById(customer.EntityId).Result.NameNepali;
            }
            else
            {
                var vhlsec = await _vhlsecService.GetVhlsecById(customer.EntityId);
                c.OrgNameNepali = vhlsec != null? _vhlsecService.GetVhlsecById(customer.EntityId).Result.NameNepali:"VHLSEC";
                c.OrgAddressNepali = vhlsec!=null?_localLevelService.GetNepaliDistrict(vhlsec.District):"VHLSEC ADDRESS" ;
            }
            return c;
        }
    }
}
