using LIMS.Api.DTOs.AINR;
using LIMS.Api.DTOs.GeneralCMS;
using LIMS.Api.Queries.Models.Common;
using LIMS.Domain.Data;
using LIMS.Services.GeneralCMS;
using LIMS.Services.Media;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using LIMS.Api.Extensions;

namespace LIMS.Api.Queries.Handlers.GeneralCMS
{
    public class GetEmployeeQueryHandler: IRequestHandler<GetQueryCMS<EmployeeDto>, IList<EmployeeDto>>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IPictureService _pictureService;
        public GetEmployeeQueryHandler(IEmployeeService employeeService,
                                     IPictureService pictureService)
        {
            _employeeService = employeeService;
            _pictureService = pictureService;
        }
        public async Task<IList<EmployeeDto>> Handle(GetQueryCMS<EmployeeDto> request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.UserId))
            {
                var employee =await _employeeService.GetAll();

                var employeeDto = employee.Where(m => m.UserId == request.UserId).Select(m => m.ToModel()).ToList();

                return employeeDto;

            }
            else
            {
                return new List<EmployeeDto>();
            }
        }
    }
}

