using LIMS.Core;
using LIMS.Domain;
using LIMS.Domain.Breed;
using LIMS.Domain.Data;
using LIMS.Domain.GeneralCMS;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Services.GeneralCMS
{
  public   class EmployeeService:IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IMediator _mediator;
        private readonly IWorkContext _workContext;

       public EmployeeService(IRepository<Employee> employeeRepository, IMediator mediator,IWorkContext workContext)
        {
            _employeeRepository = employeeRepository;
            _mediator = mediator;
            _workContext = workContext;
        }
        public async Task<List<Employee>> GetAll()
        {
            var employee = _employeeRepository.Table;
            return employee.Where(m=>m.IsActive==true).ToList();
        }
        public async Task DeleteEmployee(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException("Employee");
            await _employeeRepository.DeleteAsync(employee);

            //event notification
            await _mediator.EntityDeleted(employee);
        }

        public async Task<IPagedList<Employee>> GetEmployee(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _employeeRepository.Table;
            return await PagedList<Employee>.Create(query, pageIndex, pageSize);
        }  
        public async Task<IPagedList<Employee>> GetEmployeeByUser(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var userId = _workContext.CurrentCustomer.Id;
            var query = _employeeRepository.Collection;
            var filter = Builders<Employee>.Filter.Eq("UserId", userId);
            return await PagedList<Employee>.Create(query,filter, pageIndex, pageSize);
        }
        public async Task<IPagedList<Employee>> GetByUser(string userId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var filter = Builders<Employee>.Filter.Eq("UserId", userId);
            var query = _employeeRepository.Table;
            return await PagedList<Employee>.Create(query, pageIndex, pageSize);
        }

        public Task<Employee> GetEmployeeById(string Id)
        {
            return _employeeRepository.GetByIdAsync(Id);
        }
        public async Task InsertEmployee(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException("Employee");
            await _employeeRepository.InsertAsync(employee);

            //event notification
            await _mediator.EntityInserted(employee);
        }

        public async Task UpdateEmployee(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException("Employee");
            await _employeeRepository.UpdateAsync(employee);

            //event notification
            await _mediator.EntityUpdated(employee);
        }

        public async Task UpdateEmployee(List<Employee> employee)
        {
            if (employee == null)
                throw new ArgumentNullException("Employee");

            await _employeeRepository.UpdateAsync(employee);          
        }

       
    }


}
