using LIMS.Domain;
using LIMS.Domain.Breed;
using LIMS.Domain.Data;
using LIMS.Domain.Users;
using LIMS.Services.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.User
{
    public class NlboUserService:INlboUserService
    {
        private readonly IRepository<NlboUser> _nlboUserRepository;
        private readonly IMediator _mediator;
        public NlboUserService(IRepository<NlboUser> nlboUserRepository, IMediator mediator)
        {
            _nlboUserRepository = nlboUserRepository;
            _mediator = mediator;
        }
        public async Task DeleteNlboUser(NlboUser nlboUser)
        {
            if (nlboUser == null)
                throw new ArgumentNullException("NlboUser");
            await _nlboUserRepository.DeleteAsync(nlboUser);

            //event notification
            await _mediator.EntityDeleted(nlboUser);
        }

        public async Task<IPagedList<NlboUser>> GetNlboUser(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _nlboUserRepository.Table;
            return await PagedList<NlboUser>.Create(query, pageIndex, pageSize);
        }

        public Task<NlboUser> GetNlboUserById(string id)
        {
            return _nlboUserRepository.GetByIdAsync(id);
        }

        public async Task InsertNlboUser(NlboUser nlboUser)
        {
            if (nlboUser == null)
                throw new ArgumentNullException("nlboUser");
            await _nlboUserRepository.InsertAsync(nlboUser);

            //event notification
            await _mediator.EntityInserted(nlboUser);
        }

        public async Task UpdateNlboUser(NlboUser nlboUser)
        {
            if (nlboUser == null)
                throw new ArgumentNullException("nlboUser");
            await _nlboUserRepository.UpdateAsync(nlboUser);

            //event notification
            await _mediator.EntityUpdated(nlboUser);
        }

    }
}
