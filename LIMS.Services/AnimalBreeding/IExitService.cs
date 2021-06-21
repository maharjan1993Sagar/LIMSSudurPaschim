using LIMS.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.AnimalBreeding
{
    public interface IExitService
    {
        Task ExitAnimal(Exit exit);
    }
}
