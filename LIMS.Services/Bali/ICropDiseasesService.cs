using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public interface ICropDiseasesService
    {
        Task<CropDiseases> GetCropDiseasesById(string id);
        Task<IPagedList<CropDiseases>> GetCropDiseases(string createdby,  string fiscalYearId, string LocalLevel, string district, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<CropDiseases>> GetCropDiseases(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task<IPagedList<CropDiseases>> GetCropDiseases(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task DeleteCropDiseases(CropDiseases CropDiseases);

        Task InsertCropDiseases(CropDiseases CropDiseases);
        Task InsertCropDiseasesList(List<CropDiseases> livestocks);

        Task UpdateCropDiseases(CropDiseases CropDiseases);
        Task UpdateCropDiseasesList(List<CropDiseases> livestocks);
    }
}
