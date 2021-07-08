using LIMS.Domain.Breed;
using LIMS.Domain.GeneralCMS;
using LIMS.Web.Areas.Admin.Models.Breed;
using LIMS.Web.Areas.Admin.Models.GeneralCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class EmployeeMappingExtension
    {
        public static EmployeeModel ToModel(this Employee entity)
        {
            return entity.MapTo<Employee, EmployeeModel>();
        }

        public static Employee ToEntity(this EmployeeModel model)
        {
            return model.MapTo<EmployeeModel, Employee>();
        }

        public static Employee ToEntity(this EmployeeModel model, Employee destination)
        {
            return model.MapTo(destination);
        }
    }
}
