using Autofac;
using LIMS.Core.Configuration;
using LIMS.Core.Infrastructure;
using LIMS.Core.Infrastructure.DependencyManagement;
using LIMS.Domain.Organizations;
using LIMS.Services.Activities;
using LIMS.Services.Ainr;
using LIMS.Services.AnimalBreeding;
using LIMS.Services.AnimalHealth;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.DynamicMenu;
using LIMS.Services.FeedBack;
using LIMS.Services.LocalStructure;
using LIMS.Services.MedicineInventory;
using LIMS.Services.MoAMAC;
using LIMS.Services.NewsEvent;
using LIMS.Services.Organizations;
using LIMS.Services.OtherOrganizations;
using LIMS.Services.Professionals;
using LIMS.Services.RationBalance;
using LIMS.Services.Recording;
using LIMS.Services.Semen;
using LIMS.Services.Statisticaldata;
using LIMS.Services.User;
using LIMS.Services.Vaccination;
using LIMS.Services.VaccinationInventory;
using LIMS.Web.Areas.Admin.Interfaces;
using LIMS.Web.Areas.Admin.Services;

namespace LIMS.Web.Areas.Admin.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, LIMSConfig config)
        {
            builder.RegisterType<ActivityLogViewModelService>().As<IActivityLogViewModelService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerAttributeViewModelService>().As<ICustomerAttributeViewModelService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerViewModelService>().As<ICustomerViewModelService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerReminderViewModelService>().As<ICustomerReminderViewModelService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerRoleViewModelService>().As<ICustomerRoleViewModelService>().InstancePerLifetimeScope();
            builder.RegisterType<EmailAccountViewModelService>().As<IEmailAccountViewModelService>().InstancePerLifetimeScope();
            builder.RegisterType<KnowledgebaseViewModelService>().As<IKnowledgebaseViewModelService>().InstancePerLifetimeScope();
            builder.RegisterType<LanguageViewModelService>().As<ILanguageViewModelService>().InstancePerLifetimeScope();
            builder.RegisterType<VaccinationTypeService>().As<IVaccinationTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<MoAMACService>().As<IMoAMACService>().InstancePerLifetimeScope();
            builder.RegisterType<ParaProfessionalService>().As<IParaProfessionalService>().InstancePerLifetimeScope();
            builder.RegisterType<VetGraduateService>().As<IVetGraduateService>().InstancePerLifetimeScope();
            builder.RegisterType<BreedService>().As<IBreedService>().InstancePerLifetimeScope();
            builder.RegisterType<SpeciesService>().As<ISpeciesService>().InstancePerLifetimeScope();
            builder.RegisterType<UnitService>().As<IUnitService>().InstancePerLifetimeScope();
            builder.RegisterType<FiscalYearService>().As<IFiscalYearService>().InstancePerLifetimeScope();
            builder.RegisterType<DolfdService>().As<IDolfdService>().InstancePerLifetimeScope();
            builder.RegisterType<VhlsecService>().As<IVhlsecService>().InstancePerLifetimeScope();
            builder.RegisterType<LssService>().As<ILssService>().InstancePerLifetimeScope();
            builder.RegisterType<FarmService>().As<IFarmService>().InstancePerLifetimeScope();
            builder.RegisterType<AnimalRegistrationService>().As<IAnimalRegistrationService>().InstancePerLifetimeScope();
            builder.RegisterType<OwnerService>().As<IOwnerService>().InstancePerLifetimeScope();
            builder.RegisterType<DiseaseService>().As<IDiseaseService>().InstancePerLifetimeScope();
            builder.RegisterType<MilkRecordingService>().As<IMilkRecordingService>().InstancePerLifetimeScope();
            builder.RegisterType<GrowthMonitoringService>().As<IGrowthMonitoringService>().InstancePerLifetimeScope();
            builder.RegisterType<AiService>().As<IAiService>().InstancePerLifetimeScope();
            builder.RegisterType<PregnencyDiagnosisService>().As<IPregnencyDiagnosisService>().InstancePerLifetimeScope();
            builder.RegisterType<PregnencyTerminationService>().As<IPregnencyTerminationService>().InstancePerLifetimeScope();
            builder.RegisterType<HeatRecordingService>().As<IHeatRecordingService>().InstancePerLifetimeScope();
            builder.RegisterType<EarTagService>().As<IEarTagService>().InstancePerLifetimeScope();
            builder.RegisterType<LivestockService>().As<ILivestockService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductionDataService>().As<IProductionionDataService>().InstancePerLifetimeScope();
            builder.RegisterType<ServiceDataService>().As<IServiceData>().InstancePerLifetimeScope();
            builder.RegisterType<NlboService>().As<INlboService>().InstancePerLifetimeScope();
            builder.RegisterType<FeedbackService>().As<IFeedbackService>().InstancePerLifetimeScope();
            builder.RegisterType<ReceivedMedicineService>().As<IReceivedMedicineService>().InstancePerLifetimeScope();
            builder.RegisterType<MedicineProgressService>().As<IMedicineProgressService>().InstancePerLifetimeScope();
            builder.RegisterType<MedicineDemandService>().As<IMedicineDemandService>().InstancePerLifetimeScope();
            builder.RegisterType<NlboUserService>().As<INlboUserService>().InstancePerLifetimeScope();
            builder.RegisterType<OrganizationService>().As<IOrganizationService>().InstancePerLifetimeScope();
            builder.RegisterType<AnimalTypeService>().As<IAnimalTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<ActivityService>().As<IActivityService>().InstancePerLifetimeScope();
            builder.RegisterType<TargetService>().As<ITargetRegisterService>().InstancePerLifetimeScope();
            builder.RegisterType<ActivityProgressService>().As<IActivityProgressService>().InstancePerLifetimeScope();
            builder.RegisterType<VaccinationService>().As<IVaccinationService>().InstancePerLifetimeScope();
            builder.RegisterType<VaccinationUserService>().As<IVaccinationUserService>().InstancePerLifetimeScope();
            builder.RegisterType<LogViewModelService>().As<ILogViewModelService>().InstancePerLifetimeScope();
            builder.RegisterType<ServiceProviderService>().As<IServiceProviderService>().InstancePerLifetimeScope();
            builder.RegisterType <LocalLevelService>().As<ILocalLevelService>().InstancePerLifetimeScope();
            builder.RegisterType<ReceivedVaccinationService>().As<IReceivedVaccinationService>().InstancePerLifetimeScope();
            builder.RegisterType<RationBalanceService>().As<IRationBalanceService>().InstancePerLifetimeScope();
            builder.RegisterType<OtherOrganizationService>().As<IOtherOrganizationService>().InstancePerLifetimeScope();
            builder.RegisterType<PurnaKhopService>().As<IPurnaKhopService>().InstancePerLifetimeScope();
            builder.RegisterType<FarmForVaccinatonService>().As<IFarmForVaccinationService>().InstancePerLifetimeScope();
            builder.RegisterType<OtherOrganizationDetailsService>().As<IOtherOrganizationDetailsService>().InstancePerLifetimeScope();
            builder.RegisterType<FeedIndustryServices>().As<IFeedIndustryServices>().InstancePerLifetimeScope();
            builder.RegisterType<DiaryIndustryService>().As<IDIaryIndustryService>().InstancePerLifetimeScope();
            builder.RegisterType<MeatShopService>().As<IMeatShopService>().InstancePerLifetimeScope();
            builder.RegisterType<MeatProcessingIndustryService>().As<IMeatProcessingIndustryService>().InstancePerLifetimeScope();
            builder.RegisterType<AnimalFeedService>().As<IAnimalFeedService>().InstancePerLifetimeScope();
            builder.RegisterType<FishSrotService>().As<IFishSrotService>().InstancePerLifetimeScope();
            builder.RegisterType<LivestockResearchCenterService>().As<ILivestockResearchCenterService>().InstancePerLifetimeScope();
            builder.RegisterType<FeedFodderResearchCenterService>().As<IFeedFodderResearchCenterService>().InstancePerLifetimeScope();
            builder.RegisterType<NGOService>().As<INGOService>().InstancePerLifetimeScope();
            builder.RegisterType<VetClinicService>().As<IVetClinicService>().InstancePerLifetimeScope();
            builder.RegisterType<TechSchoolService>().As<ITechSchoolService>().InstancePerLifetimeScope();
            builder.RegisterType<CanelClubeService>().As<ICanelClubeService>().InstancePerLifetimeScope();
            builder.RegisterType<SemenDistributionService>().As<ISemenDistributionService>().InstancePerLifetimeScope();
            builder.RegisterType<DistributedVaccinationService>().As<IDistributedVaccinationService>().InstancePerLifetimeScope();
            builder.RegisterType<ExitService>().As<IExitService>().InstancePerLifetimeScope();
            builder.RegisterType<FiscalYearForGraphService>().As<IFiscalYearForGraphService>().InstancePerLifetimeScope();
           builder.RegisterType<SampleService>().As<ISampleService>().InstancePerLifetimeScope();


            builder.RegisterType<NewsEventService>().As<INewsEventService>().InstancePerLifetimeScope();
            builder.RegisterType<MainMenuService>().As<IMainMenuService>().InstancePerLifetimeScope();
            builder.RegisterType<SubMenuService>().As<ISubMenuService>().InstancePerLifetimeScope();
            builder.RegisterType<SubSubMenuService>().As<ISubSubMenuService>().InstancePerLifetimeScope();
            builder.RegisterType<NewsEventService>().As<INewsEventService>().InstancePerLifetimeScope();
         
        }

        public int Order {
            get { return 3; }
        }
    }
}
