using LIMS.Domain;
using LIMS.Domain.AInR;
using LIMS.Domain.Data;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LIMS.Services.Events;
using System.Linq;

namespace LIMS.Services.Ainr
{
    public class AnimalRegistrationService : IAnimalRegistrationService
    {
        private readonly IRepository<AnimalRegistration> _animalRegistrationRepository;
        private readonly IMediator _mediator;
        public AnimalRegistrationService(IRepository<AnimalRegistration> AnimalRegistrationRepository, IMediator mediator)
        {
            _animalRegistrationRepository = AnimalRegistrationRepository;
            _mediator = mediator;
        }

        public async Task DeleteAnimalRegistration(AnimalRegistration animalRegistration)
        {
            if (animalRegistration == null)
                throw new ArgumentNullException("AnimalRegistration");
            await _animalRegistrationRepository.DeleteAsync(animalRegistration);

            //event notification
            await _mediator.EntityDeleted(animalRegistration);
        }
       

        public async Task<IPagedList<AnimalRegistration>> GetAnimalRegistration(string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _animalRegistrationRepository.Table;
            query = query.Where(m => m.IsDeleted == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(f =>
                f.Name.ToLower() == keyword ||
                f.Species.EnglishName.ToLower() == keyword ||
                f.EarTagNo == keyword

                );
            }

            return await PagedList<AnimalRegistration>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<AnimalRegistration>> GetAnimalByUser(string userid, string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _animalRegistrationRepository.Table;
            query = query.Where(m => m.IsDeleted == false);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(f =>
                f.Name.ToLower().Contains(keyword) ||
                f.Species.EnglishName.ToLower().Contains(keyword) ||
                f.EarTagNo.Contains(keyword) ||
                f.Farm.NameEnglish.ToLower().Contains(keyword)

                );
            }
            query = query.Where(m => m.CreatedBy == userid);
            return await PagedList<AnimalRegistration>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<AnimalRegistration>> GetAnimalByLss(List<string> customerid, string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _animalRegistrationRepository.Table.Where(t => customerid.Contains(t.CreatedBy));
            query = query.Where(m => m.IsDeleted == false);

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                query = query.Where(f =>
                f.Name.ToLower().Contains(keyword) ||
                f.Species.EnglishName.ToLower().Contains(keyword) ||
                f.EarTagNo.Contains(keyword) ||
                f.Farm.NameEnglish.ToLower().Contains(keyword)

                );
            }
            return await PagedList<AnimalRegistration>.Create(query, pageIndex, pageSize);
        }
        public  List<AnimalRegistration> GetAllAnimalByLss(List<string> customerid)
        {
            var query = _animalRegistrationRepository.Table.Where(t => customerid.Contains(t.CreatedBy));
            query = query.Where(m => m.IsDeleted == false);

            return query.ToList();
        }

        public async Task<IPagedList<AnimalRegistration>> GetAnimalRegistrationByFarmId(string farmId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _animalRegistrationRepository.Table;
            query = query.Where(d => d.Farm.Id == farmId && d.IsDeleted==false);
            return await PagedList<AnimalRegistration>.Create(query, pageIndex, pageSize);

        }

        public Task<AnimalRegistration> GetAnimalRegistrationById(string id)
        {
            return _animalRegistrationRepository.GetByIdAsync(id);
        }

        public async Task ExitAnimal(string id)
        {
             var animal=await _animalRegistrationRepository.GetByIdAsync(id);
            animal.IsDeleted = true;
           await _animalRegistrationRepository.UpdateAsync(animal);

        }
        public async Task InsertAnimalRegistration(AnimalRegistration animalRegistration)
        {
            if (animalRegistration == null)
                throw new ArgumentNullException("AnimalRegistration");
            animalRegistration.IsDeleted = false;
            await _animalRegistrationRepository.InsertAsync(animalRegistration);

            //event notification
            await _mediator.EntityInserted(animalRegistration);
        }

        public async Task UpdateAnimalRegistration(AnimalRegistration animalRegistration)
        {
            if (animalRegistration == null)
                throw new ArgumentNullException("AnimalRegistration");
            await _animalRegistrationRepository.UpdateAsync(animalRegistration);

            //event notification
            await _mediator.EntityUpdated(animalRegistration);
        }
        public async Task<IPagedList<AnimalRegistration>> GetAnimalRegistrationByCreatedBy(string createdBy, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _animalRegistrationRepository.Table;
            query = query.Where(m => m.IsDeleted == false);

            query = query.Where(d => d.CreatedBy == createdBy);
            return await PagedList<AnimalRegistration>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<AnimalRegistration>> SearchAnimal(string farm,string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _animalRegistrationRepository.Table;
            query = query.Where(m => m.IsDeleted == false);

            query = query.Where(m => m.Farm.Id == farm);
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                query = query.Where(ar =>
                    ar.Name.ToLower().Contains(keyword)
                    ||
                    ar.EarTagNo.ToLower().Contains(keyword)
                );
            }
            return await PagedList<AnimalRegistration>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<AnimalRegistration>> SearchMaleAnimal(string createdBy, string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue)
       {
            var query = _animalRegistrationRepository.Table;
            query = query.Where(m => m.IsDeleted == false);

            query = query.Where(m => m.CreatedBy == createdBy && m.Gender== "Male");
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                query = query.Where(ar =>
                    ar.Name.ToLower().Contains(keyword)
                    ||
                    ar.EarTagNo.ToLower().Contains(keyword)
                );
            }
            return await PagedList<AnimalRegistration>.Create(query, pageIndex, pageSize);
        }

    }
}
