using LIMS.Domain.Data;
using LIMS.Domain.Services;
using LIMS.Services.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.AnimalBreeding
{
    public class ExitService : IExitService
    {
        private readonly IRepository<Exit> _exitRepository;
        private readonly IMediator _mediator;
        public ExitService(IRepository<Exit> exitRepository, IMediator mediator)
        {
            _exitRepository = exitRepository;
            _mediator = mediator;
        }
        public async Task ExitAnimal(Exit exit)
        {
            if (exit == null)
                throw new ArgumentNullException("AI");
            await _exitRepository.InsertAsync(exit);

            //event notification
            await _mediator.EntityInserted(exit);
        }
    }
}
