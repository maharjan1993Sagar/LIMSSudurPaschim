using LIMS.Api.DTOs.AINR;
using LIMS.Api.DTOs.AnimalBreeding;
using LIMS.Api.Extensions;
using LIMS.Api.Queries.Models.Common;
using LIMS.Core;
using LIMS.Domain.Services;
using LIMS.Services.Ainr;
using LIMS.Services.AnimalBreeding;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Queries.Handlers.Ainr
{
    public class ExitAnimalQueryHandler: IRequestHandler<ExitQueryHandler<ExitDto>, ExitDto>
    {
        private readonly IAnimalRegistrationService _animalRegistrationService;
        private readonly IExitService _exitService;
        private readonly IWorkContext _workContext;
        public ExitAnimalQueryHandler(IAnimalRegistrationService animalRegistrationService, 
            IWorkContext workContext,
            ExitService exitService)
        {
            _animalRegistrationService = animalRegistrationService;
            _workContext = workContext;
            _exitService = exitService;
        }
        public async Task<ExitDto> Handle(ExitQueryHandler<ExitDto> request, CancellationToken cancellationToken)
        {
            var exit = new Exit();
            exit.AnimalRegistrationId = request.Model.AnimalRegistrationId;
            exit.Comments = request.Model.Comments;
            exit.DateOfExit = request.Model.DateOfExit;
            exit.ReasonForExit = request.Model.ReasonForExit;
            exit.source = _workContext.CurrentCustomer.OrgName + "From mobile";
            exit.CreatedBy = _workContext.CurrentCustomer.Id;
            await _exitService.ExitAnimal(exit);
            await _animalRegistrationService.ExitAnimal(request.Model.AnimalRegistrationId);
            return exit.ToModel();

        }

        }
    }
