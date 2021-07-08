using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain;
using LIMS.Domain.Breed;
using LIMS.Domain.Data;
using LIMS.Domain.GeneralCMS;

namespace LIMS.Services.GeneralCMS
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAll();
        Task<IPagedList<Employee>> GetEmployee(int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<Employee>> GetEmployeeByUser(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteEmployee(Employee employee);

        Task InsertEmployee(Employee employee);

        Task UpdateEmployee(Employee employee);
        Task<Employee> GetEmployeeById(string id);
    }
}
