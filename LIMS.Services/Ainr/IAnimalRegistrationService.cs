using LIMS.Domain;
using LIMS.Domain.AInR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Ainr
{
    public interface IAnimalRegistrationService
    {
        Task<AnimalRegistration> GetAnimalRegistrationById(string id);

        Task<IPagedList<AnimalRegistration>> GetAnimalRegistration(string keyword = "",int pageIndex = 0, int pageSize = int.MaxValue);
        Task DeleteAnimalRegistration(AnimalRegistration animalRegistration);

        Task InsertAnimalRegistration(AnimalRegistration animalRegistration);

        Task UpdateAnimalRegistration(AnimalRegistration animalRegistration);
        Task<IPagedList<AnimalRegistration>> GetAnimalRegistrationByFarmId(string farmId, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<AnimalRegistration>> GetAnimalRegistrationByCreatedBy(string createdBy,int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<AnimalRegistration>> GetAnimalByUser(string userid, string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<AnimalRegistration>> GetAnimalByLss(List<string> customerid, string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue);
        List<AnimalRegistration> GetAllAnimalByLss(List<string> customerid);

        Task<IPagedList<AnimalRegistration>> SearchAnimal(string farm,string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<AnimalRegistration>> SearchMaleAnimal(string createdBy, string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue);

        Task ExitAnimal(string id);

    }
}
