using LIMS.Domain;
using LIMS.Domain.Breed;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Breed
{
    public class SubsidyTypeService:ISubsidyTypeService
    {
        private readonly IRepository<SubsidyType> _SubsidyTypeRepository;
        private readonly IMediator _mediator;
        public SubsidyTypeService(IRepository<SubsidyType> SubsidyTypeRepository, IMediator mediator)
        {
            _SubsidyTypeRepository = SubsidyTypeRepository;
            _mediator = mediator;
        }
        public async Task DeleteSubsidyType(SubsidyType SubsidyType)
        {
            if (SubsidyType == null)
                throw new ArgumentNullException("SubsidyType");
            await _SubsidyTypeRepository.DeleteAsync(SubsidyType);

            //event notification
            await _mediator.EntityDeleted(SubsidyType);
        }

        public async Task<IPagedList<SubsidyType>> GetSubsidyType(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _SubsidyTypeRepository.Table;
            return await PagedList<SubsidyType>.Create(query, pageIndex, pageSize);
        }

        public Task<SubsidyType> GetSubsidyTypeById(string id)
        {
            return _SubsidyTypeRepository.GetByIdAsync(id);
        }

        public async Task InsertSubsidyType(SubsidyType SubsidyType)
        {
            if (SubsidyType == null)
                throw new ArgumentNullException("SubsidyType");
            await _SubsidyTypeRepository.InsertAsync(SubsidyType);

            //event notification
            await _mediator.EntityInserted(SubsidyType);
        }

        public async Task UpdateSubsidyType(SubsidyType SubsidyType)
        {
            if (SubsidyType == null)
                throw new ArgumentNullException("SubsidyType");
            await _SubsidyTypeRepository.UpdateAsync(SubsidyType);

            //event notification
            await _mediator.EntityUpdated(SubsidyType);
        }
    }
}
