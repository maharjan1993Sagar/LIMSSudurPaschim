using LIMS.Domain;
using LIMS.Domain.AnimalHealth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.AnimalHealth
{
    public interface ISampleService
    {
        Task<Sample> GetsampleById(string id);

        Task<IPagedList<Sample>> Getsample(string createdby, int pageIndex = 0, int pageSize = int.MaxValue);

        Task Deletesample(Sample sample);
        Task Insertsample(Sample smaple);
        Task Updatesample(Sample sample);
    }
}
