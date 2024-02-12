using LIMS.Core;
using LIMS.Framework.Components;
using LIMS.Services.Ainr;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MoAMAC;
using LIMS.Services.Statisticaldata;
using LIMS.Services.Vaccination;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Components
{
    public class LssTreatmentReportViewComponent:BaseViewComponent
    {

        #region fields
        private readonly ILocalizationService _localizationService;
        private readonly ISpeciesService _speciesService;
        private readonly IUnitService _unitService;
        private readonly IFiscalYearService _fiscalYearService;
        public readonly IBreedService _breedService;
        public readonly IWorkContext _workContext;
        public readonly IServiceData _serviceData;
        public readonly ILssService _lssService;
        public readonly ICustomerService _customerService;
        public readonly IFarmService _farmService;
        public readonly IVaccinationTypeService _vaccinationService;

        #endregion fields
        #region ctor
        public LssTreatmentReportViewComponent(ILocalizationService localizationService,
            ISpeciesService speciesService,
             IUnitService unitService,
              IFiscalYearService fiscalYearService,
              IBreedService breedService,
              IWorkContext workContext,
              IServiceData serviceData,
              ILssService lssService,
              ICustomerService customerService,
              IFarmService farmService,
               IVaccinationTypeService vaccinationService
            )
        {
            _localizationService = localizationService;
            _speciesService = speciesService;
            _unitService = unitService;
            _fiscalYearService = fiscalYearService;
            _breedService = breedService;
            _workContext = workContext;
            _serviceData = serviceData;
            _lssService = lssService;
            _customerService = customerService;
            _farmService = farmService;
            _vaccinationService = vaccinationService;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync(string fiscalyear,string month)
        {
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            if (roles.Contains(RoleHelper.LssUser) || roles.Contains(RoleHelper.LssAdmin))
            {

                string createdby = null;
                //if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
                //{
                //    createdby = _workContext.CurrentCustomer.Id;
                //}
                //else
                //{
                //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
                //    var admin = await _customerService.GetCustomerByEmail(adminemail);
                //    createdby = admin.Id;
                //}


                var reportmodel = new List<MonthlyProgressReportModel>();
                var monthHelper = new MonthHelper();
                var months = monthHelper.GetMonths();
                SelectListItem listindex = months.Where(m => m.Value == month).Single<SelectListItem>();
                int index = months.IndexOf(listindex);
                index = index + 1;
                string previousMonth = null;
                if (index != 1)
                {
                    previousMonth = months.ElementAt(index - 2).Value.ToString();
                }
                else
                {
                    previousMonth = "no";
                }

                var deseasePrevention = await _serviceData.GetService(createdby, "animal health", month);
                var previousmonthDesease = await _serviceData.GetService(createdby, "animal health", previousMonth);
                var fiscalyearDisease = await _serviceData.GetService(createdby, "animal health", fiscalyear);
                int progressLastMonth = previousmonthDesease.Where(m => m.AnimalHealthService == "Disease preventation").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int progressThisMonth = deseasePrevention.Where(m => m.AnimalHealthService == "Disease preventation").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int progressThisFiscalMonth = fiscalyearDisease.Where(m => m.AnimalHealthService == "Disease preventation").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));

                List<int> diseaseData = new List<int> { progressLastMonth, progressThisMonth, progressThisFiscalMonth };
                var row = new RowDataModel();
                row.Data = diseaseData;
                row.Title = "Disease preventation";
                reportmodel.Add(new MonthlyProgressReportModel {
                    Topic = "Health Service",
                    Unit = (deseasePrevention.Count() > 0 && deseasePrevention.Where(m => m.AnimalHealthService == "Disease preventation") != null) ? deseasePrevention.Where(m => m.AnimalHealthService == "Disease preventation").FirstOrDefault().Unit.UnitNameEnglish : "",
                    Rows = row
                }
                );
                int FecalExaminationLastMonth = previousmonthDesease.Where(m => m.AnimalHealthService == "Fecal Examination").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int FecalExaminationThisMonth = deseasePrevention.Where(m => m.AnimalHealthService == "Fecal Examination").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int FecalExaminationFiscalMonth = fiscalyearDisease.Where(m => m.AnimalHealthService == "Fecal Examination").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));


                List<int> FecalExaminationData = new List<int> { FecalExaminationLastMonth, FecalExaminationThisMonth, FecalExaminationFiscalMonth };
                var rowFecalExamination = new RowDataModel();
                rowFecalExamination.Data = FecalExaminationData;
                rowFecalExamination.Title = "Fecal Examination";
                reportmodel.Add(new MonthlyProgressReportModel {
                    Topic = "Health Service",
                    Unit = (deseasePrevention.Where(m => m.AnimalHealthService == "Fecal Examination").Count() > 0) ? deseasePrevention.Where(m => m.AnimalHealthService == "Fecal Examination").FirstOrDefault().Unit.UnitNameEnglish : "",

                    Rows = rowFecalExamination
                }
                );
                int SamplecollectionLastMonth = previousmonthDesease.Where(m => m.AnimalHealthService == "Sample collection").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int SamplecollectionThisMonth = deseasePrevention.Where(m => m.AnimalHealthService == "Sample collection").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int SamplecollectionFiscalMonth = fiscalyearDisease.Where(m => m.AnimalHealthService == "Sample collection").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));


                List<int> SamplecollectionData = new List<int> { SamplecollectionLastMonth, SamplecollectionThisMonth, SamplecollectionFiscalMonth };
                var rowSamplecollection = new RowDataModel();
                rowSamplecollection.Data = SamplecollectionData;
                rowSamplecollection.Title = "Sample collection";
                reportmodel.Add(new MonthlyProgressReportModel {
                    Topic = "Health Service",
                    Unit = (deseasePrevention.Where(m => m.AnimalHealthService == "Sample collection").Count() > 0) ? deseasePrevention.Where(m => m.AnimalHealthService == "Sample collection").FirstOrDefault().Unit.UnitNameEnglish : "",

                    Rows = rowSamplecollection
                }
                );
                int SwabexaminationLastMonth = previousmonthDesease.Where(m => m.AnimalHealthService == "Swab examination").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int SwabexaminationThisMonth = deseasePrevention.Where(m => m.AnimalHealthService == "Swab examination").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int SwabexaminationFiscalMonth = fiscalyearDisease.Where(m => m.AnimalHealthService == "Swab examination").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));


                List<int> SwabexaminationData = new List<int> { SwabexaminationLastMonth, SwabexaminationThisMonth, SamplecollectionFiscalMonth };
                var rowSwabexamination = new RowDataModel();
                rowSwabexamination.Data = SwabexaminationData;
                rowSwabexamination.Title = "Swab examination";
                reportmodel.Add(new MonthlyProgressReportModel {
                    Topic = "Health Service",
                    Unit = (deseasePrevention.Where(m => m.AnimalHealthService == "Swab examination").Count() > 0) ? deseasePrevention.Where(m => m.AnimalHealthService == "Swab examination").FirstOrDefault().Unit.UnitNameEnglish : "",

                    Rows = rowSwabexamination
                }
                );

                var treatment = await _serviceData.GetService(createdby, "treatment", month);
                var previousmonthtreatment = await _serviceData.GetService(createdby, "treatment", previousMonth);
                var fiscalyeartreatment = await _serviceData.GetService(createdby, "treatment", fiscalyear);
                int treatmentprogressLastMonth = previousmonthDesease.Where(m => m.TreatmentType == "Medical treatment").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int treatmentprogressThisMonth = treatment.Where(m => m.TreatmentType == "Medical treatment").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int treatmentprogressThisFiscalMonth = fiscalyeartreatment.Where(m => m.TreatmentType == "Medical treatment").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                List<int> diseasepreventationData = new List<int> { treatmentprogressLastMonth, treatmentprogressThisMonth, treatmentprogressThisFiscalMonth };
                var rowDiseasepreventation = new RowDataModel();
                rowDiseasepreventation.Data = diseasepreventationData;
                rowSamplecollection.Title = "Medical treatment";
                reportmodel.Add(new MonthlyProgressReportModel {
                    Topic = "Treatment",

                    Unit = (treatment.Where(m => m.TreatmentType == "Medical treatment").Count() > 0) ? treatment.Where(m => m.TreatmentType == "Medical treatment").FirstOrDefault().Unit.UnitNameEnglish : "",
                    Rows = rowSamplecollection
                }
                );
                int minorsurgicalLastMonth = previousmonthtreatment.Where(m => m.TreatmentType == "Minor surgical").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int minorsurgicalThisMonth = treatment.Where(m => m.TreatmentType == "Minor surgical").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int minorsurgicalThisFiscalMonth = fiscalyeartreatment.Where(m => m.TreatmentType == "Minor surgical").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                List<int> minorsurgicalData = new List<int> { minorsurgicalLastMonth, minorsurgicalThisMonth, minorsurgicalThisFiscalMonth };
                var rowminorsurgical = new RowDataModel();
                rowminorsurgical.Data = minorsurgicalData;
                rowminorsurgical.Title = "Minor surgical";
                reportmodel.Add(new MonthlyProgressReportModel {
                    Topic = "Treatment",
                    Unit = (treatment.Where(m => m.TreatmentType == "Minor surgical").Count() > 0) ? treatment.Where(m => m.TreatmentType == "Minor surgical").FirstOrDefault().Unit.UnitNameEnglish : "",

                    Rows = rowminorsurgical
                }
                );
                int gynecologicalTreatmentLastMonth = previousmonthtreatment.Where(m => m.TreatmentType == "Gynecological Treatment").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int gynecologicalTreatmentThisMonth = treatment.Where(m => m.TreatmentType == "Gynecological Treatment").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int gynecologicalTreatmentThisFiscalMonth = fiscalyeartreatment.Where(m => m.TreatmentType == "Gynecological Treatment").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                List<int> gynecologicalTreatmentData = new List<int> { gynecologicalTreatmentLastMonth, gynecologicalTreatmentThisMonth, gynecologicalTreatmentThisFiscalMonth };
                var rowgynecologicalTreatment = new RowDataModel();
                rowgynecologicalTreatment.Data = gynecologicalTreatmentData;
                rowgynecologicalTreatment.Title = "Gynecological Treatment";
                reportmodel.Add(new MonthlyProgressReportModel {
                    Topic = "Treatment",
                    Unit = (treatment.Where(m => m.TreatmentType == "Gynecological Treatment").Count() > 0) ? treatment.Where(m => m.TreatmentType == "Gynecological Treatment").FirstOrDefault().Unit.UnitNameEnglish : "",

                    Rows = rowgynecologicalTreatment
                }
                );



                var Vaccination = await _serviceData.GetService(createdby, "vaccination", month);
                var previousmonthVaccination = await _serviceData.GetService(createdby, "vaccination", previousMonth);
                var fiscalyearVaccination = await _serviceData.GetService(createdby, "vaccination", fiscalyear);
                int PPRLastMonth = previousmonthVaccination.Where(m => m.Vaccination != null && m.Vaccination.MedicalName.ToLower() == "ppr").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int PPRThisMonth = Vaccination.Where(m => m.Vaccination != null && m.Vaccination.MedicalName.ToLower() == "ppr").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int PPRThisFiscalMonth = fiscalyearVaccination.Where(m => m.Vaccination != null && m.Vaccination.MedicalName.ToLower() == "ppr").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));

                List<int> PPRData = new List<int> { PPRLastMonth, PPRThisMonth, PPRThisFiscalMonth };
                var rowPPRData = new RowDataModel();
                rowPPRData.Data = PPRData;
                rowPPRData.Title = "PPR";
                reportmodel.Add(new MonthlyProgressReportModel {
                    Topic = "Vaccination",
                    Unit = (Vaccination.Where(m => m.Vaccination != null && m.Vaccination.MedicalName.ToLower() == "ppr").Count() > 0) ? Vaccination.Where(m => m.Vaccination != null && m.Vaccination.MedicalName.ToLower() == "ppr").FirstOrDefault().Unit.UnitNameEnglish : "",

                    Rows = rowPPRData
                }
                );
                int FMDLastMonth = previousmonthVaccination.Where(m => m.Vaccination != null && m.Vaccination.MedicalName.ToLower() == "fmd").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int FMDThisMonth = Vaccination.Where(m => m.Vaccination != null && m.Vaccination.MedicalName.ToLower() == "fmd").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int FMDThisFiscalMonth = fiscalyearVaccination.Where(m => m.Vaccination != null && m.Vaccination.MedicalName.ToLower() == "fmd").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));

                List<int> FMDData = new List<int> { FMDLastMonth, FMDThisMonth, FMDThisFiscalMonth };
                var rowFMDData = new RowDataModel();
                rowFMDData.Data = FMDData;
                rowFMDData.Title = "FMD";
                reportmodel.Add(new MonthlyProgressReportModel {
                    Topic = "Vaccination",
                    Unit = (Vaccination.Where(m => m.Vaccination != null && m.Vaccination.MedicalName.ToLower() == "fmd").Count() > 0) ? Vaccination.Where(m => m.Vaccination != null && m.Vaccination.MedicalName.ToLower() == "fmd").FirstOrDefault().Unit.UnitNameEnglish : "",

                    Rows = rowFMDData
                }
                );
                int HsLastMonth = previousmonthVaccination.Where(m => m.Vaccination != null && m.Vaccination.MedicalName.Equals("HS/BQ", StringComparison.InvariantCultureIgnoreCase)).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int HsThisMonth = Vaccination.Where(m => m.Vaccination != null && m.Vaccination.MedicalName.Equals("HS/BQ", StringComparison.InvariantCultureIgnoreCase)).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int HsThisFiscalMonth = fiscalyearVaccination.Where(m => m.Vaccination != null && m.Vaccination.MedicalName.Equals("HS/BQ", StringComparison.InvariantCultureIgnoreCase)).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));

                List<int> HsData = new List<int> { HsLastMonth, HsThisMonth, HsThisFiscalMonth };
                var rowHsData = new RowDataModel();
                rowHsData.Data = HsData;
                rowHsData.Title = "HS/BQ";
                reportmodel.Add(new MonthlyProgressReportModel {
                    Topic = "Vaccination",
                    Unit = (Vaccination.Where(m => m.Vaccination != null && m.Vaccination.MedicalName.Equals("HS/BQ", StringComparison.InvariantCultureIgnoreCase)).Count() > 0) ? Vaccination.Where(m => m.Vaccination != null && m.Vaccination.MedicalName.Equals("HS/BQ", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault().Unit.UnitNameEnglish : "",

                    Rows = rowHsData
                }
                );
                int RabbiesLastMonth = previousmonthVaccination.Where(m => m.Vaccination != null && m.Vaccination.MedicalName.Equals("Rabbies", StringComparison.InvariantCultureIgnoreCase)).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int RabbiesThisMonth = Vaccination.Where(m => m.Vaccination != null && m.Vaccination.MedicalName.Equals("Rabbies", StringComparison.InvariantCultureIgnoreCase)).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int RabbiesThisFiscalMonth = fiscalyearVaccination.Where(m => m.Vaccination != null && m.Vaccination.MedicalName.Equals("Rabbies", StringComparison.InvariantCultureIgnoreCase)).Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));

                List<int> RabbiesData = new List<int> { RabbiesLastMonth, RabbiesThisMonth, RabbiesThisFiscalMonth };
                var rowRabbiesData = new RowDataModel();
                rowRabbiesData.Data = RabbiesData;
                rowRabbiesData.Title = "Rabbies";
                reportmodel.Add(new MonthlyProgressReportModel {
                    Topic = "Vaccination",
                    Unit = (Vaccination.Where(m => m.Vaccination != null && m.Vaccination.MedicalName.Equals("Rabbies", StringComparison.InvariantCultureIgnoreCase)).Count() > 0) ? Vaccination.Where(m => m.Vaccination != null && m.Vaccination.MedicalName.Equals("Rabbies", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault().Unit.UnitNameEnglish : "",

                    Rows = rowRabbiesData
                }
                );


                var AI = await _serviceData.GetService(createdby, "ai", month);
                var previousmonthAI = await _serviceData.GetService(createdby, "ai", previousMonth);
                var fiscalyearAi = await _serviceData.GetService(createdby, "ai", fiscalyear);
                int cowAiLastMonth = previousmonthAI.Where(m => m.Species.EnglishName.ToLower() == "cow").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int cowAiThisMonth = AI.Where(m => m.Species.EnglishName.ToLower() == "cow").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int cowAiThisFiscalMonth = fiscalyearAi.Where(m => m.Species.EnglishName.ToLower() == "cow").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));

                List<int> cowAiData = new List<int> { cowAiLastMonth, cowAiThisMonth, cowAiThisFiscalMonth };
                var rowCowAi = new RowDataModel();
                rowCowAi.Data = cowAiData;
                rowCowAi.Title = "Cow";
                reportmodel.Add(new MonthlyProgressReportModel {
                    Topic = "Ai",
                    Unit = (AI.Where(m => m.Species.EnglishName.ToLower() == "cow").Count() > 0) ? AI.Where(m => m.Species.EnglishName.ToLower() == "cow").FirstOrDefault().Unit.UnitNameEnglish : "",

                    Rows = rowCowAi
                }
                );
                int buffaloAiLastMonth = previousmonthAI.Where(m => m.Species.EnglishName.ToLower() == "buffalo").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int buffaloAiThisMonth = AI.Where(m => m.Species.EnglishName.ToLower() == "buffalo").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int buffaloAiThisFiscalMonth = fiscalyearAi.Where(m => m.Species.EnglishName.ToLower() == "buffalo").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));

                List<int> buffaloAiData = new List<int> { buffaloAiLastMonth, buffaloAiThisMonth, buffaloAiThisFiscalMonth };
                var rowbuffaloAi = new RowDataModel();
                rowbuffaloAi.Data = buffaloAiData;
                rowbuffaloAi.Title = "Buffalo";
                reportmodel.Add(new MonthlyProgressReportModel {
                    Topic = "Ai",
                    Unit = (AI.Where(m => m.Species.EnglishName.ToLower() == "buffalo").Count() > 0) ? AI.Where(m => m.Species.EnglishName.ToLower() == "buffalo").FirstOrDefault().Unit.UnitNameEnglish : "",
                    Rows = rowbuffaloAi
                }
                );
                int otherAiLastMonth = previousmonthAI.Where(m => m.Species.EnglishName.ToLower() != "buffalo" && m.Species.EnglishName.ToLower() != "cow").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int OtherAiThisMonth = AI.Where(m => m.Species.EnglishName.ToLower() != "buffalo" && m.Species.EnglishName.ToLower() != "cow").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));
                int otherAiThisFiscalMonth = fiscalyearAi.Where(m => m.Species.EnglishName.ToLower() != "buffalo" && m.Species.EnglishName.ToLower() != "cow").Sum(m => Convert.ToInt32(String.IsNullOrEmpty(m.Quantity) ? "0" : m.Quantity));

                List<int> otherAiData = new List<int> { otherAiLastMonth, OtherAiThisMonth, otherAiThisFiscalMonth };
                var rowotherAi = new RowDataModel();
                rowotherAi.Data = otherAiData;
                rowotherAi.Title = "Other";
                reportmodel.Add(new MonthlyProgressReportModel {
                    Topic = "Ai",
                    Unit = (AI.Where(m => m.Species.EnglishName.ToLower() == "cow").Count() > 0) ? AI.Where(m => m.Species.EnglishName.ToLower() == "cow").FirstOrDefault().Unit.UnitNameEnglish : "",

                    Rows = rowotherAi
                }
                );

                return View(reportmodel);
            }
            else
            {
                string vhlsecid = _workContext.CurrentCustomer.EntityId;
                List<string> lssId = _lssService.GetLssByVhlsecId(vhlsecid).Result.Select(m => m.Id).ToList();
                var customers = _customerService.GetCustomerByLssId(lssId, vhlsecid);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                return View();

            }

        }
    }
}
