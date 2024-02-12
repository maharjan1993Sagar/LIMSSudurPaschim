using LIMS.Domain;
using LIMS.Domain.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain.StatisticalData;
using LIMS.Services.Events;
using MongoDB.Driver.Linq;

namespace LIMS.Services.Statisticaldata
{
    public class ServiceDataService:IServiceData
    {
        private readonly IRepository<ServicesData> _serviceRepository;
        private readonly IMediator _mediator;
        public ServiceDataService(IRepository<ServicesData> serviceRepository, IMediator mediator)
        {
            _serviceRepository = serviceRepository;
            _mediator = mediator;
        }
        public async Task DeleteService(ServicesData service)
        {
            if (service == null)
                throw new ArgumentNullException("Service");

            await _serviceRepository.DeleteAsync(service);

            //event notification
            await _mediator.EntityDeleted(service);
        }

        public async Task<IPagedList<ServicesData>> GetService(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _serviceRepository.Table;


            return await PagedList<ServicesData>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<ServicesData>> GetService(List<string> customer, string type,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _serviceRepository.Table;
            query = query.Where(m => customer.Contains(m.CreatedBy) && m.ServicesType.ToLower() == type&&m.FiscalYear.Id==fiscalyear);

            return await PagedList<ServicesData>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<ServicesData>> GetService(List<string> customer,string type,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _serviceRepository.Table;
            query = query.Where(m => customer.Contains(m.CreatedBy)&& m.ServicesType.ToLower()==type);

            return await PagedList<ServicesData>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<ServicesData>> GetService(string customer, string type,string month, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _serviceRepository.Table;
            query = query.Where(m => m.CreatedBy==customer && m.ServicesType.ToLower() == type&&m.Month==month);

            return await PagedList<ServicesData>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<ServicesData>> GetServiceByFiscalyear(string customer, string type,string fiscalYear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _serviceRepository.Table;
            query = query.Where(m => m.CreatedBy==customer && m.ServicesType.ToLower() == type&&m.FiscalYear.Id==fiscalYear);

            return await PagedList<ServicesData>.Create(query, pageIndex, pageSize);
        }
        

        public Task<ServicesData> GetServiceById(string Id)
        {
            return _serviceRepository.GetByIdAsync(Id);

        }

        public async Task InsertService(ServicesData service)
        {
            if (service == null)
                throw new ArgumentNullException("Service");

            await _serviceRepository.InsertAsync(service);

            //event notification
            await _mediator.EntityInserted(service);
        }

        public async Task UpdateService(ServicesData service)
        {
            if (service == null)
                throw new ArgumentNullException("Service");

            await _serviceRepository.UpdateAsync(service);

            //event notification
            await _mediator.EntityUpdated(service);
        }
        public async Task InsertServiceList(List<ServicesData> services)
        {
            if (services.Count < 1)
                throw new ArgumentNullException("Service");
            await _serviceRepository.InsertManyAsync(services);


        }
        public async Task UpdateServiceList(List<ServicesData> services)
        {
            if (services.Count < 1)
                throw new ArgumentNullException("Service");
            foreach (var item in services)
            {
                 _serviceRepository.Update(item);
            }


        }
        public async Task<List<ServicesData>> GetFilteredService( string fiscalyearId, string quarter,string month, string serviceType, string createdby, string district, string locallevel,string ward, string species="",string vaccineName="",string tretmentType="",string animalHealth="", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _serviceRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (!string.IsNullOrEmpty(species)&& serviceType.ToLower()=="ai")
            {
               
                query = query.Where(m =>
                  m.Month == quarter &&
                  m.ServicesType == serviceType &&
                  m.FiscalYear.Id == fiscalyearId &&
                  m.Species.Id == species&&
                  m.District==district&&
                  m.LocalLevel==locallevel&&
                  m.Ward==ward&&
                  m.Month==month
                );
            }
            else
            {
                query = query.Where(m =>
                                 m.Month == quarter &&
                                 m.ServicesType == serviceType &&
                                 m.FiscalYear.Id == fiscalyearId &&                                 
                                  m.District == district &&
                                   m.LocalLevel == locallevel&&
                                    m.Ward == ward &&
                                 m.Month == month
                               );
            }

            if (serviceType!=null&&serviceType.ToLower() == "vaccination") {
                query = query.Where(m => m.VaccinationId == vaccineName);
            }
            if (serviceType != null && serviceType.ToLower() == "treatment")
            {
                query = query.Where(m => m.TreatmentType == tretmentType);
            }
            if (serviceType != null && serviceType.ToLower() == "animalhealth")
            {
                query = query.Where(m => m.AnimalHealthService == animalHealth);
            }
            return await PagedList<ServicesData>.Create(query, pageIndex, pageSize);
        }


    }
}
