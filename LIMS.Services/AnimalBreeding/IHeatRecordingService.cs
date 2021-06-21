using LIMS.Domain;
using LIMS.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.AnimalBreeding
{
    public interface IHeatRecordingService
    {
        Task<HeatRecording> GetHeatRecordingById(string Id);

        Task<IPagedList<HeatRecording>> GetHeatRecording(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteHeatRecording(HeatRecording heatRecording);

        Task InsertHeatRecording(HeatRecording heatRecording);

        Task UpdateHeatRecording(HeatRecording heatRecording);
        Task<IPagedList<HeatRecording>> GetHeatRecordingByAnimalId(string animalId, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
