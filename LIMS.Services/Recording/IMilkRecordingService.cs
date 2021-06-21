using LIMS.Domain;
using LIMS.Domain.Recording;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Recording
{
    public  interface IMilkRecordingService
    {
        Task<MilkRecording> GetMilkRecordingById(string id);

        Task<IPagedList<MilkRecording>> GetMilkRecording(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteMilkRecording(MilkRecording MilkRecording);

        Task InsertMilkRecording(MilkRecording MilkRecording);

        Task UpdateMilkRecording(MilkRecording MilkRecording);
    }
}
