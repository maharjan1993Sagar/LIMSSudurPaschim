using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.Organizations;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Organizations
{
    public class VetClinicService:IVetClinicService
    {
        private readonly IRepository<VetClinic> _vetClinicRepository;
        private readonly IMediator _mediator;
        public VetClinicService(IRepository<VetClinic> vetClinicRepository, IMediator mediator)
        {
            _vetClinicRepository = vetClinicRepository;
            _mediator = mediator;
        }
        public async Task DeleteVetClinic(VetClinic VetClinic)
        {
            if (VetClinic == null)
                throw new ArgumentNullException("VetClinic");
            await _vetClinicRepository.DeleteAsync(VetClinic);

            //event notification
            await _mediator.EntityDeleted(VetClinic);
        }

        public async Task<IPagedList<VetClinic>> GetVetClinic(string createdby,string fiscalYear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _vetClinicRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (!string.IsNullOrEmpty(fiscalYear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYear);

            }
            return await PagedList<VetClinic>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<VetClinic>> GetVetClinic(List<string> createdby, string fiscalYear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _vetClinicRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy) );
            if (!string.IsNullOrEmpty(fiscalYear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYear);

            }
            return await PagedList<VetClinic>.Create(query, pageIndex, pageSize);
        }

        public Task<VetClinic> GetVetClinicById(string id)
        {
            return _vetClinicRepository.GetByIdAsync(id);
        }

        public async Task InsertVetClinic(VetClinic VetClinic)
        {
            if (VetClinic == null)
                throw new ArgumentNullException("VetClinic");
            await _vetClinicRepository.InsertAsync(VetClinic);

            //event notification
            await _mediator.EntityInserted(VetClinic);
        }
        public async Task InsertVetClinicList(List<VetClinic> VetClinic)
        {
            if (VetClinic == null)
                throw new ArgumentNullException("VetClinic");
            await _vetClinicRepository.InsertManyAsync(VetClinic);

        }

        public async Task UpdateVetClinic(VetClinic VetClinic)
        {
            if (VetClinic == null)
                throw new ArgumentNullException("VetClinic");
            await _vetClinicRepository.UpdateAsync(VetClinic);

            //event notification
            await _mediator.EntityUpdated(VetClinic);
        }
    }
}
