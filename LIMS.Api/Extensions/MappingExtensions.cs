using LIMS.Api.DTOs.Common;
using LIMS.Api.DTOs.Customers;
using LIMS.Domain.Common;
using LIMS.Domain.Customers;
using LIMS.Domain.Media;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Api.DTOs.AINR;
using LIMS.Domain.Breed;
using LIMS.Domain.AInR;
using LIMS.Api.DTOs.FeedBack;
using LIMS.Domain.Feedback;
using LIMS.Api.DTOs.AnimalHealth;
using LIMS.Domain.Vaccination;
using LIMS.Domain.Services;
using LIMS.Api.DTOs;
using LIMS.Domain.RationBalance;
using LIMS.Api.DTOs.PerformnceRecording;
using LIMS.Domain.Recording;
using LIMS.Api.DTOs.RationBalance;
using LIMS.Api.DTOs.AnimalBreeding;
using LIMS.Domain.AnimalHealth;
using LIMS.Api.DTOs.GeneralCMS;
using LIMS.Domain.GeneralCMS;
using LIMS.Domain.DynamicMenu;
using LIMS.Domain.NewsEvent;

namespace LIMS.Api.Extensions
{
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

        #region Customer Role
        public static CustomerRoleDto ToModel(this CustomerRole entity)
        {
            return entity.MapTo<CustomerRole, CustomerRoleDto>();
        }

        public static CustomerRole ToEntity(this CustomerRoleDto model)
        {
            return model.MapTo<CustomerRoleDto, CustomerRole>();
        }

        public static CustomerRole ToEntity(this CustomerRoleDto model, CustomerRole destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Customer

        public static CustomerDto ToModel(this Customer entity)
        {
            return entity.MapTo<Customer, CustomerDto>();
        }

        public static Customer ToEntity(this CustomerDto model)
        {
            return model.MapTo<CustomerDto, Customer>();
        }

        public static Customer ToEntity(this CustomerDto model, Customer destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Customer address
        public static AddressDto ToModel(this Address entity)
        {
            return entity.MapTo<Address, AddressDto>();
        }

        public static Address ToEntity(this AddressDto model)
        {
            return model.MapTo<AddressDto, Address>();
        }
        public static Address ToEntity(this AddressDto model, Address destination)
        {
            return model.MapTo(destination);
        }


        #endregion

        #region Picture

        public static PictureDto ToModel(this Picture entity)
        {
            return entity.MapTo<Picture, PictureDto>();
        }

        #endregion

        #region Species
        public static SpeciesDto ToModel(this Species entity)
        {
            return entity.MapTo<Species, SpeciesDto>();
        }
        #endregion species

        #region Breed
        public static BreedDto ToModel(this BreedReg entity)
        {
            return entity.MapTo<BreedReg, BreedDto>();
        }
        #endregion

        #region RationBalance
        public static RationBalanceDto ToModel(this FeedLibrary entity)
        {
            return entity.MapTo<FeedLibrary, RationBalanceDto>();
        }
        #endregion

        #region FeedLibrary
        public static AnimalFeedDto ToModel(this AnimalFeed entity)
        {
            return entity.MapTo<AnimalFeed, AnimalFeedDto>();
        }
        #endregion 
        #region Vaccinationtype
        public static VaccineDto ToModel(this VaccinationType entity)
        {
            return entity.MapTo<VaccinationType, VaccineDto>();
        }
        #endregion

        #region Vaccination
        public static VaccinationDto ToModel(this AnimalVaccination entity)
        {
            return entity.MapTo<AnimalVaccination, VaccinationDto>();
        }
        public static AnimalVaccination ToEntity(this VaccinationDto model)
        {
            return model.MapTo<VaccinationDto, AnimalVaccination>();
        }
        public static AnimalVaccination ToEntity(this VaccinationDto model,AnimalVaccination destination)
        {
            return model.MapTo(destination);
        }
        #endregion 

        #region Farm
        public static FarmDto ToModel(this Farm entity)
        {
            return entity.MapTo<Farm, FarmDto>();
        }
        public static Farm ToEntity(this FarmDto model)
        {
            return model.MapTo<FarmDto, Farm>();
        }
        public static Farm ToEntity(this FarmDto model, Farm destination)
        {
            return model.MapTo(destination);
        }
        #endregion Farm

        #region AnimalRegistration
        public static AnimalRegistrationDto ToModel(this AnimalRegistration entity)
        {
            return entity.MapTo<AnimalRegistration, AnimalRegistrationDto>();
        }
        public static AnimalRegistration ToEntity(this AnimalRegistrationDto model)
        {
            return model.MapTo<AnimalRegistrationDto, AnimalRegistration>();
        }
        public static AnimalRegistration ToEntity(this AnimalRegistrationDto model, AnimalRegistration destination)
        {
            return model.MapTo(destination);
        }
        #endregion AnimalRegistration
        #region MilkRecording
        public static MilkRecordingDto ToModel(this MilkRecording entity)
        {
            return entity.MapTo<MilkRecording, MilkRecordingDto>();
        }
        public static MilkRecording ToEntity(this MilkRecordingDto model)
        {
            return model.MapTo<MilkRecordingDto, MilkRecording>();
        }
        public static MilkRecording ToEntity(this MilkRecordingDto model, MilkRecording destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region GrowthRecording
        public static GrowthMonitoringDto ToModel(this GrowthMonitoring entity)
        {
            return entity.MapTo<GrowthMonitoring, GrowthMonitoringDto>();
        }
        public static GrowthMonitoring ToEntity(this GrowthMonitoringDto model)
        {
            return model.MapTo<GrowthMonitoringDto, GrowthMonitoring>();
        }
        public static GrowthMonitoring ToEntity(this GrowthMonitoringDto model, GrowthMonitoring destination)
        {
            return model.MapTo(destination);
        }
        #endregion 
        #region Feedback
        public static FeedbackDto ToModel(this Feedback entity)
        {
            return entity.MapTo<Feedback, FeedbackDto>();
        }
        public static Feedback ToEntity(this FeedbackDto model)
        {
            return model.MapTo<FeedbackDto, Feedback>();
        }
        public static Feedback ToEntity(this FeedbackDto model, Feedback destination)
        {
            return model.MapTo(destination);
        }
        #endregion Feedback
        #region AI
        public static AIDto ToModel(this AIService entity)
        {
            return entity.MapTo<AIService, AIDto>();
        }
        public static AIService ToEntity(this AIDto model)
        {
            return model.MapTo<AIDto, AIService>();
        }
        public static AIService ToEntity(this AIDto model, AIService destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region PregnencyDiagnosis
        public static PregnencyDiagnosisDto ToModel(this PregnencyDiagnosis entity)
        {
            return entity.MapTo<PregnencyDiagnosis, PregnencyDiagnosisDto>();
        }
        public static PregnencyDiagnosis ToEntity(this PregnencyDiagnosisDto model)
        {
            return model.MapTo<PregnencyDiagnosisDto, PregnencyDiagnosis>();
        }
        public static PregnencyDiagnosis ToEntity(this PregnencyDiagnosisDto model, PregnencyDiagnosis destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region Treatment
        public static TreatMentDto ToModel(this TreatMent entity)
        {
            return entity.MapTo<TreatMent, TreatMentDto>();
        }
        public static TreatMent ToEntity(this TreatMentDto model)
        {
            return model.MapTo<TreatMentDto, TreatMent>();
        }
        public static TreatMent ToEntity(this TreatMentDto model, TreatMent destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region Heatrecording
        public static HeatRecordingDto ToModel(this HeatRecording entity)
        {
            return entity.MapTo<HeatRecording, HeatRecordingDto>();
        }
        public static HeatRecording ToEntity(this HeatRecordingDto model)
        {
            return model.MapTo<HeatRecordingDto, HeatRecording>();
        }
        public static HeatRecording ToEntity(this HeatRecordingDto model, HeatRecording destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region PregnencyTermination
        public static PregnencyTerminationDto ToModel(this PregnencyTermination entity)
        {
            return entity.MapTo<PregnencyTermination, PregnencyTerminationDto>();
        }
        public static PregnencyTermination ToEntity(this PregnencyTerminationDto model)
        {
            return model.MapTo<PregnencyTerminationDto, PregnencyTermination>();
        }
        public static PregnencyTermination ToEntity(this PregnencyDiagnosisDto model, PregnencyTermination destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region exit
        public static ExitDto ToModel(this Exit entity)
        {
            return entity.MapTo<Exit, ExitDto>();
        }
        public static Exit ToEntity(this ExitDto model)
        {
            return model.MapTo<ExitDto, Exit>();
        }
        #endregion

        #region GeneralCMS
        public static BannerDto ToModel(this Banner entity)
        {
            return entity.MapTo<Banner, BannerDto>();
        }
        public static Banner ToEntity(this BannerDto model)
        {
            return model.MapTo<BannerDto, Banner>();
        }
        
        public static ContactUsDto ToModel(this ContactUs entity)
        {
            return entity.MapTo<ContactUs, ContactUsDto>();
        }
        public static ContactUs ToEntity(this ContactUsDto model)
        {
            return model.MapTo<ContactUsDto, ContactUs>();
        }
         
        public static EmployeeDto ToModel(this Employee entity)
        {
            return entity.MapTo<Employee, EmployeeDto>();
        }
        public static Employee ToEntity(this EmployeeDto model)
        {
            return model.MapTo<EmployeeDto, Employee>();
        } 
        public static GalleryDto ToModel(this Gallery entity)
        {
            return entity.MapTo<Gallery, GalleryDto>();
        }
        public static Gallery ToEntity(this GalleryDto model)
        {
            return model.MapTo<GalleryDto, Gallery>();
        }
        public static ImportantLinksDto ToModel(this ImportantLinks entity)
        {
            return entity.MapTo<ImportantLinks, ImportantLinksDto>();
        }
        public static ImportantLinks ToEntity(this ImportantLinksDto model)
        {
            return model.MapTo<ImportantLinksDto, ImportantLinks>();
        }
        public static MainMenuDto ToModel(this MainMenu entity)
        {
            return entity.MapTo<MainMenu, MainMenuDto>();
        }
        public static MainMenu ToEntity(this MainMenuDto model)
        {
            return model.MapTo<MainMenuDto, MainMenu>();
        }
        public static SubMenuDto ToModel(this SubMenu entity)
        {
            return entity.MapTo<SubMenu, SubMenuDto>();
        }
        public static SubMenu ToEntity(this SubMenuDto model)
        {
            return model.MapTo<SubMenuDto, SubMenu>();
        }
        public static SubSubMenuDto ToModel(this SubSubMenu entity)
        {
            return entity.MapTo<SubSubMenu, SubSubMenuDto>();
        }
        public static SubSubMenu ToEntity(this SubSubMenuDto model)
        {
            return model.MapTo<SubSubMenuDto, SubSubMenu>();
        }
        public static NewsEventFileDto ToModel(this NewsEventFile entity)
        {
            return entity.MapTo<NewsEventFile, NewsEventFileDto>();
        }
        public static NewsEventFile ToEntity(this NewsEventFileDto model)
        {
            return model.MapTo<NewsEventFileDto, NewsEventFile>();
        }
        public static NewsEventTenderDto ToModel(this NewsEventTender entity)
        {
            return entity.MapTo<NewsEventTender, NewsEventTenderDto>();
        }
        public static NewsEventTender ToEntity(this NewsEventTenderDto model)
        {
            return model.MapTo<NewsEventTenderDto, NewsEventTender>();
        }
        public static PageContentDto ToModel(this PageContent entity)
        {
            return entity.MapTo<PageContent, PageContentDto>();
        }
        public static PageContent ToEntity(this PageContentDto model)
        {
            return model.MapTo<PageContentDto, PageContent>();
        }

        #endregion
    }
}
