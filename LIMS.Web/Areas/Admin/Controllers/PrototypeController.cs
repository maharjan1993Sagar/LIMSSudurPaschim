using LIMS.Domain.Breed;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Localization;
using LIMS.Services.MoAMAC;
using LIMS.Services.Professionals;
using LIMS.Services.Security;
using LIMS.Services.Vaccination;
using LIMS.Web.Areas.Admin.Models.AInR;
using LIMS.Web.Areas.Admin.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class PrototypeController : BaseAdminController
    {
        private readonly ILocalizationService _localizationService;
        private readonly IBreedService _breedService;
        private readonly ISpeciesService _speciesService;
        private readonly IVaccinationTypeService _vaccinationType;
        private readonly IParaProfessionalService _paraProfessionalService;
        private readonly IVetGraduateService _vetGraduateService;
        private readonly IMoAMACService _moAMACService;
        private readonly IFiscalYearService _fiscalYearService;
        public PrototypeController(ILocalizationService localizationService,
            IBreedService breedService, ISpeciesService speciesService,
            IVaccinationTypeService vaccinationType, IParaProfessionalService paraProfessionalService,
            IVetGraduateService vetGraduateService, IMoAMACService moAMACService, IFiscalYearService fiscalYearService)
        {
            _localizationService = localizationService;
            _breedService = breedService;
            _speciesService = speciesService;
            _vaccinationType = vaccinationType;
            _vetGraduateService = vetGraduateService;
            _paraProfessionalService = paraProfessionalService;
            _moAMACService = moAMACService;
            _fiscalYearService = fiscalYearService;
        }

        public IActionResult SpeciesCreate()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> GetBreed(string id)
        {
            return Ok(await _breedService.GetBreedBySpeciesId(id));
        }
        [AllowAnonymous]
        public async Task<IActionResult> GetSpecies()
        {
            return Ok(await _speciesService.GetSpecies());
        }
        [AllowAnonymous]
        public async Task<IActionResult> GetSpeciesVaccination(string VaccinationId)
        {
            var vaccination = await _vaccinationType.GetVaccinationTypeById(VaccinationId);
            var species = new List<Species>();
            foreach(var item in vaccination.Species)
            {
                species.Add(await _speciesService.GetSpeciesById(item));
            }
            return Ok(species);
        }
        [AllowAnonymous]
        public async Task<IActionResult> GetVetGraduate(string type)
        {
            if (type == "Vet-graduate")
            {
                var vet = new SelectList(await _vetGraduateService.GetVetGraduate(), "Id", "NameEnglish").ToList();
                return Ok(vet);
            }
            else
            {
                var para = new SelectList(await _paraProfessionalService.GetParaProfessionals(), "Id", "NameEnglish").ToList();
                return Ok(para);
            }
        }

        public IActionResult FarmCreate()
        {
            var category = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Farmer",
                    Value="farmer"
                },
                new SelectListItem{
                    Text="Farm",
                    Value="farm"
                }
            };
            category.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            var education = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Bachelor",
                    Value="Bachelor"
                },
                new SelectListItem{
                    Text="+2",
                    Value="+2"
                },
                new SelectListItem{
                    Text="Secondary level",
                    Value="Secondary level"
                },
                new SelectListItem{
                    Text="Below class 8",
                    Value="Below class 8"
                }
            };
            education.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            var farmType = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Public",
                    Value="Public"
                },
                new SelectListItem{
                    Text="Private",
                    Value="Private"
                },
                new SelectListItem{
                    Text="Semi-Public",
                    Value="SemiPublic"
                },
            };
            farmType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            var ethnicGroup = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Dalit",
                    Value="Dalit"
                },
                new SelectListItem{
                    Text="JanaJati",
                    Value="JanaJati"
                },
                new SelectListItem{
                    Text="Aanya",
                    Value="Aanya"
                },
            };
            ethnicGroup.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            var province = GetProvinceList();
            province.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.Provience = province;
            ViewBag.Education = education;
            ViewBag.FarmType = farmType;
            ViewBag.Category = category;
            ViewBag.EthinicGroup = ethnicGroup;

            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.AnimalRegistration)]
        public IActionResult OwnerCreate()
        {
            var ownerType = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Owner",
                    Value="Owner"
                },
                new SelectListItem{
                    Text="Keeper",
                    Value="Keeper"
                },
            };
            ownerType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            var province = GetProvinceList();
            province.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            var farmId = new List<SelectListItem>() {
                new SelectListItem{
                    Text="KirshiFarm Pokhara",
                    Value="KirshiFarm Pokhara"
                },
                new SelectListItem{
                    Text="Nepal Krishi farm",
                    Value="Nepal Krishi farm"
                },
            };
            farmId.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            var ethnicGroup = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Dalit",
                    Value="Dalit"
                },
                new SelectListItem{
                    Text="JanaJati",
                    Value="JanaJati"
                },
                new SelectListItem{
                    Text="Anya",
                    Value="Anya"
                },
            };
            ethnicGroup.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.Provience = province;
            ViewBag.Type = ownerType;
            ViewBag.EthinicGroup = ethnicGroup;
            ViewBag.FarmId = farmId;
            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.AnimalRegistration)]
        public async Task<IActionResult> AnimalRegistration()
        {
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;

            var breed = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Jarshi",
                    Value="Jarshi"
                },
                new SelectListItem{
                    Text="JamunaPari",
                    Value="JamunaPari"
                }
            };
            breed.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.SpeciesId = species;
            ViewBag.BreedId = breed;
            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.AnimalRegistration)]
        public IActionResult FarmList(DataSourceRequest command)
        {
            var farmModel = new List<FarmModel>(
                new List<FarmModel>
                {
                    new FarmModel{ NameEnglish="Krishna farm", Category="Farm",Phone="9876543210"},
                    new FarmModel{ NameEnglish="Rajan farm", Category="Farm",Phone="987654320"},
                    new FarmModel{ NameEnglish="Raj Rai", Category="Farmer",Phone="987654320"},
                    new FarmModel{ NameEnglish="Raja Sherpa", Category="Farmer",Phone="987654320"}
                });

            var gridModel = new DataSourceResult {
                Data = farmModel,
                Total = farmModel.Count()
            };
            return Json(gridModel);
        }

        public async Task<IActionResult> Services()
        {
            return View();
        }

        public IActionResult VaccinationList()
        {
            var vaccination = new List<VaccinationServiceModel>(
                  new List<VaccinationServiceModel>
                  {
                      new VaccinationServiceModel{
                          VaccinationDate=Convert.ToDateTime("2017/10/18"), VaccinationForDisease="Rbs",VaccinationSubType="cde",
                           AnimalRegistration=new AnimalRegistrationModel
                        {
                            Name="Abc",
                            EarTagNo="123456789",
                            FarmModel=new FarmModel {
                                NameEnglish="Krishna farm"
                            },
                        },

                          },


                  });

            var gridModel = new DataSourceResult {
                Data = vaccination,
                Total = vaccination.Count()
            };
            return Json(gridModel);



        }



        public async Task<IActionResult> Vaccination()
        {
            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.AnimalHealth)]
        public async Task<IActionResult> VaccinationTab()
        {
            var model = new AnimalRegistrationModel();
            var vaccination = new List<SelectListItem>();
            if (await _vaccinationType.GetVaccination() != null)
            {
                vaccination = new SelectList(await _vaccinationType.FiletrVaccinationType("Vaccine"), "Id", "MedicalName").ToList();
                vaccination.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            }
            else
                vaccination.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.vaccinationId = vaccination;

            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();

            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYear = fiscalyear;

            var disease = new List<SelectListItem>() {
                new SelectListItem{Text="rabies", Value="rabies" },
            };
            disease.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Disease = disease;

            var labName = new List<SelectListItem> {
                new SelectListItem { Text = "Karnali lab", Value = "Karnali lab" },
                new SelectListItem { Text = "Test Lab2", Value = "TestLab2" },
            };
            labName.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LabName = labName;

            var sampleType = new List<SelectListItem> {
                new SelectListItem { Text = "Urine", Value = "Urine" },
                new SelectListItem { Text = "Milk", Value = "Milk" },
            };
            sampleType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SampleType = sampleType;

            return View(model);
        }


        public async Task<IActionResult> ServiceTab()
        {
            var model = new AnimalRegistrationModel();

            var serviceType = new List<SelectListItem>() {
                new SelectListItem{Text="Natural",Value="Natural"},
                new SelectListItem{Text="Others", Value="Others" }
            };
            serviceType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();

            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYear = fiscalyear;
            ViewBag.FiscalYear = fiscalyear;
            ViewBag.ServiceType = serviceType;

            var terminationType = new List<SelectListItem>() {
                new SelectListItem{Text="False diagnosis", Value="False diagnosis" },
                new SelectListItem{Text="Missed calving ", Value="Missed calving" },
                new SelectListItem{Text="Others", Value="Others" },
            };
            terminationType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.TerminationType = terminationType;

            var calvingType = new List<SelectListItem>() {
                new SelectListItem{Text="Single male", Value="Single male" },
                new SelectListItem{Text="Single female ", Value="Single female" },
                new SelectListItem{Text="Twin female", Value="Twin female" },
                new SelectListItem{Text="Twin male", Value="Twin male" },
                new SelectListItem{Text="Twin male-female", Value="Twin male-female" }
            };
            calvingType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.CalvingType = calvingType;
            var birthType = new List<SelectListItem>() {
                new SelectListItem{Text="Normal", Value="Normal" },
                new SelectListItem{Text="pre-mature", Value="pre-mature" },
                new SelectListItem{Text="Abortion", Value="Abortion" },
                };
            birthType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.BirthType = birthType;
            var easeBirth = new List<SelectListItem>() {
                new SelectListItem{Text="Easy", Value="Easy" },
                new SelectListItem{Text="Considerable assistant", Value="Considerable assistant" },
                new SelectListItem{Text="vet. Assistantincl. Caesarien", Value="vet. Assistantincl. Caesarien" },
                };
            easeBirth.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.EaseBirth = easeBirth;
            var fateOfCalf = new List<SelectListItem>() {
                new SelectListItem{Text="Alive", Value="Alive" },
                new SelectListItem{Text="Dead", Value="Dead" },
                };
            fateOfCalf.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FateOfCalf = fateOfCalf;
            var method = new List<SelectListItem>() {
                new SelectListItem{Text="Rector Palpation", Value="Rector Palpation" },
                new SelectListItem{Text="Usg ", Value="Usg" },
            };
            method.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Method = method;

            var state = new List<SelectListItem>() {
                new SelectListItem{Text="Normal", Value="Normal" },
                new SelectListItem{Text="Force ", Value="Force" },
            };
            state.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.State = state;
            var typeOfAi = new List<SelectListItem> {
                new SelectListItem { Text = "First", Value = "First" },
                new SelectListItem { Text = "Repeat", Value = "Repeat" },
                 new SelectListItem { Text = "Double", Value = "Double" },

            };
            typeOfAi.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.TypeOfAi = typeOfAi;

            ViewBag.State = state;
            var reasonForExit = new List<SelectListItem> {
                new SelectListItem { Text = "Culled", Value = "Culled" },
                new SelectListItem { Text = "Old age", Value = "Old age" },
                new SelectListItem { Text = "Sick", Value = "Sick" },
                new SelectListItem { Text = "Sick", Value = "Sick" },
                new SelectListItem { Text = "Died", Value = "Died" },
                new SelectListItem { Text = "Sold for use", Value = "Sold for use" },
                new SelectListItem { Text = "Sold for slaughter", Value = "Sold for slaughter" },
                new SelectListItem { Text = "Others", Value = "Others" },

            };
            reasonForExit.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));


            ViewBag.ReasonForExit = reasonForExit;
            return View(model);
        }


        public IActionResult ServicesList(DataSourceRequest command)
        {
            var animalRegistrationModels = new List<AnimalRegistrationModel>(
                new List<AnimalRegistrationModel>
                {
                    new AnimalRegistrationModel{
                        Name="test",
                        EarTagNo="123456789",
                        FarmModel=new FarmModel
                        {
                            NameEnglish="Krishna farm",
                            Category="Farm",
                            Phone="9876543210"
                        },
                    },
                    new AnimalRegistrationModel{
                        Name="text1",
                        EarTagNo="123450789",
                        FarmModel=new FarmModel
                        {
                            NameEnglish="Krishna farm",
                            Category="Farm",
                            Phone="9876543212"
                        },
                    }
                    ,
                    new AnimalRegistrationModel{
                        Name="text2",
                        EarTagNo="",
                        FarmModel=new FarmModel
                        {
                            NameEnglish="Krishna farm",
                            Category="Farm",
                            Phone="9876543214"
                        },
                    },
                    new AnimalRegistrationModel
                    {
                        Name="test3",
                        EarTagNo="",
                        FarmModel=new FarmModel
                        {
                            NameEnglish="Rajan farm",
                            Category="Farm",
                            Phone="9876543216"
                        },
                    },
                    new AnimalRegistrationModel
                    {
                        Name="text4",
                        EarTagNo="",
                        FarmModel=new FarmModel
                        {
                            NameEnglish="Ram farm",
                            Category="Farm",
                            Phone="9876543218"
                        },
                    },
                    new AnimalRegistrationModel
                    {
                        Name="text5",
                        EarTagNo="",
                        FarmModel=new FarmModel
                        {
                            NameEnglish="hari farm",
                            Category="Farm",
                            Phone="9876543219"
                        },
                    },
                });

            var gridModel = new DataSourceResult {
                Data = animalRegistrationModels,
                Total = animalRegistrationModels.Count()
            };
            return Json(gridModel);
        }

        public IActionResult ServiceProvider()
        {
            var province = GetProvinceList();
            province.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            var serviceProviderType = new List<SelectListItem> {
                new SelectListItem { Text = "Vet-graduate", Value = "Vet-graduate" },
                new SelectListItem { Text = "Para-professional", Value = "Para-professional" },
            };
            serviceProviderType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.ServiceProviderType = serviceProviderType;
            ViewBag.Provience = province;
            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.AnimalRegistration)]
        public ActionResult AddWorker()
        {
            var workerType = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Owner",
                    Value="Owner"
                },
                new SelectListItem{
                    Text="Keeper",
                    Value="Keeper"
                },
            };
            workerType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            var province = GetProvinceList();
            province.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            var farmId = new List<SelectListItem>() {
                new SelectListItem{
                    Text="KirshiFarm Pokhara",
                    Value="KirshiFarm Pokhara"
                },
                new SelectListItem{
                    Text="Nepal Krishi farm",
                    Value="Nepal Krishi farm"
                },
            };
            farmId.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            var ethnicGroup = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Dalit",
                    Value="Dalit"
                },
                new SelectListItem{
                    Text="JanaJati",
                    Value="JanaJati"
                },
                new SelectListItem{
                    Text="Aanya",
                    Value="Aanya"
                },
            };
            ethnicGroup.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.Provience = province;
            ViewBag.Type = workerType;
            ViewBag.EthinicGroup = ethnicGroup;
            ViewBag.FarmId = farmId;

            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.AnimalRegistration)]
        public async Task<ActionResult> AddAnimal()
        {
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;

            var breed = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Jarshi",
                    Value="Jarshi"
                },
                new SelectListItem{
                    Text="JamunaPari",
                    Value="JamunaPari"
                }
            };
            breed.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.SpeciesId = species;
            ViewBag.BreedId = breed;

            return View();
        }


        public async Task<ActionResult> AddDolfd()
        {
            var moamac = new SelectList(await _moAMACService.GetMoAMAC(), "Id", "NameEnglish").ToList();
            moamac.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.MoamacId = moamac;

            var province = GetProvinceList();
            province.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.province = province;

            return View();
        }

        public async Task<ActionResult> AddVhlsec()
        {
            var province = GetProvinceList();
            province.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            var dolfdId = new List<SelectListItem> {
                new SelectListItem {
                    Text = "Dolfd",
                    Value = "Dolfd1"
                },
            };

            dolfdId.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.province = province;
            ViewBag.DolfdId = dolfdId;

            return View();
        }

        public async Task<ActionResult> AddLss()
        {
            var province = GetProvinceList();
            province.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.province = province;

            var vhlsecId = new List<SelectListItem> {
                new SelectListItem { Text = "Vhlsec1", Value = "Vlsec1" },
            };
            vhlsecId.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.VhlsecId = vhlsecId;

            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.PerformanceRecording)]
        public IActionResult MilkRecording()
        {
            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.PerformanceRecording)]
        public IActionResult MilkTab()
        {
            var model = new AnimalRegistrationModel();
            var recordingPeriod = new List<SelectListItem> {
                new SelectListItem { Text = "Morning", Value = "Morning" },
                new SelectListItem { Text = "Afternoon", Value = "Afternoon" },
                new SelectListItem { Text = "Evening", Value = "Evening" },
            };
            recordingPeriod.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.RecordingPeriod = recordingPeriod;

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.PerformanceRecording)]
        public IActionResult GrowthRecordingForm()
        {
            return View();
        }

        public IActionResult SampleEntry()
        {
            var labName = new List<SelectListItem> {
                new SelectListItem {
                    Text = "Karnali lab",
                    Value = "Karnali lab"
                },
                new SelectListItem {
                    Text = "Test Lab2",
                    Value = "TestLab2"
                },
            };
            labName.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LabName = labName;

            var sampleType = new List<SelectListItem> {
                new SelectListItem {
                    Text = "Urine",
                    Value = "Urine"
                },
                new SelectListItem {
                    Text = "Milk",
                    Value = "Milk"
                },
            };
            sampleType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SampleType = sampleType;

            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.AnimalHealth)]
        public IActionResult Treatment()
        {
            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.AnimalRegistration)]
        public IActionResult AnimalMovement()
        {
            return View();
        }

        [PermissionAuthorizeAction(PermissionActionName.AnimalRegistration)]
        public IActionResult MovementTab()
        {
            var registration = new AnimalRegistrationModel();

            var movementType = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Sold",
                    Value="Sold"
                },
                new SelectListItem{
                    Text="Others",
                    Value="Othes"
                }
            };
            movementType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.MovementType = movementType;

            var farm = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Abc krishi farm",
                    Value="Abc"
                },
                new SelectListItem{
                    Text="Cde Krishi Farm",
                    Value="Cde"
                }
            };
            farm.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Farm = farm;

            return View(registration);
        }

        private List<SelectListItem> GetProvinceList()
        {
            return new List<SelectListItem> {

                new SelectListItem { Text = _localizationService.GetResource("Common.Province.Four"), Value = "Province 4", Selected = true },

            };
        }

    }
}
