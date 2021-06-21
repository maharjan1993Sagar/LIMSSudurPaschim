using AutoMapper;
using LIMS.Api.DTOs;
using LIMS.Api.DTOs.AINR;
using LIMS.Api.DTOs.AnimalBreeding;
using LIMS.Api.DTOs.AnimalHealth;
using LIMS.Api.DTOs.Common;
using LIMS.Api.DTOs.PerformnceRecording;
using LIMS.Api.DTOs.RationBalance;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.AInR;
using LIMS.Domain.AnimalHealth;
using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Breed;
using LIMS.Domain.RationBalance;
using LIMS.Domain.Recording;
using LIMS.Domain.Services;
using LIMS.Domain.Vaccination;

namespace LIMS.Api.Infrastructure.Mapper.Profiles
{
    public class BreedProfile : Profile, IMapperProfile
    {
        public BreedProfile()
        {
            CreateMap<BreedDto, BreedReg>().ReverseMap();
            CreateMap<FarmDto, Farm>().ForMember(m => m.Provience, opt => opt.MapFrom(src => src.Province)).ReverseMap();
            CreateMap<AnimalRegistrationDto, AnimalRegistration>().ReverseMap();
            CreateMap<VaccinationType, VaccineDto>().ForMember(m => m.Name, opt => opt.MapFrom(m => m.MedicalName));
            CreateMap<Disease, DiseaseDto>().ReverseMap();
            CreateMap<FiscalYearDto, FiscalYear>().ReverseMap();
            CreateMap<VaccinationDto, AnimalVaccination>()
                .ForMember(m => m.AnimalRegistration, mo => mo.Ignore())
                .ForMember(m => m.Farm, mo => mo.Ignore())
                .ForMember(m => m.VaccinationType, mo => mo.Ignore())
                .ForMember(m => m.FiscalYear, mo => mo.Ignore())
                .ForMember(m => m.Disease, mo => mo.Ignore())
                .ForMember(m => m.GenericAttributes, mo => mo.Ignore());
            CreateMap<AnimalVaccination, VaccinationDto>();
            CreateMap<FeedLibrary, RationBalanceDto>().ReverseMap();
            CreateMap<MilkRecording, MilkRecordingDto>().ReverseMap();
            CreateMap<GrowthMonitoring, GrowthMonitoringDto>().ReverseMap();
            CreateMap<AIService, AIDto>().ReverseMap();
            CreateMap<AnimalFeed, AnimalFeedDto>().ReverseMap();
            CreateMap<PregnencyDiagnosis, PregnencyDiagnosisDto>().ReverseMap();
            CreateMap<TreatMent, TreatMentDto>().ReverseMap();
            CreateMap<HeatRecording, HeatRecordingDto>().ReverseMap();
            CreateMap<PregnencyTermination, PregnencyTerminationDto>().ReverseMap();
            CreateMap<Exit, ExitDto>().ReverseMap();

        }

        public int Order => 1;
    }
}
