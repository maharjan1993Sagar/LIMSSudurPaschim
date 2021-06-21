using Autofac.Core;
using LIMS.Domain;
using LIMS.Domain.StatisticalData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace LIMS.Services.Statisticaldata
{
    public interface IServiceData
    {
        Task<ServicesData> GetServiceById(string id);

        Task<IPagedList<ServicesData>> GetService(int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<ServicesData>> GetService(string customer, string type,string month, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<ServicesData>> GetServiceByFiscalyear(string customer, string type, string fiscalYear, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<ServicesData>> GetService(List<string> customer, string type,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);

        Task<IPagedList<ServicesData>> GetService(List<string> customer,string type, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteService(ServicesData service);

        Task InsertService(ServicesData service);

        Task UpdateService(ServicesData service);
        Task InsertServiceList(List<ServicesData> Service);
        Task UpdateServiceList(List<ServicesData> Service);
        Task<List<ServicesData>> GetFilteredService(string fiscalyearId, string quarter,string month, string serviceType, string createdby,string district,string locallevel,string ward, string species = "", string vaccineName = "", string tretmentType = "", string animalHealth = "", int pageIndex = 0, int pageSize = int.MaxValue);


    }
}
