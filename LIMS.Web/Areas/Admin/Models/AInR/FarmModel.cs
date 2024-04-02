using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using LIMS.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LIMS.Web.Areas.Admin.Models.AInR
{
    public class FarmModel : BaseEntity
    {
        [LIMSResourceDisplayName("Admin.Farm.Category")]
        public string Category { get; set; }//farmerorfarm

        [LIMSResourceDisplayName("Admin.Farm.Type")]
        public string FarmType { get; set; }//for farm or farmer public private semi-public or individual

        [LIMSResourceDisplayName("Admin.Farm.NameEnglish")]
        public string NameEnglish { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.NameNeplai")]
        public string NameNepali { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.RegNo")]
        public string RegNo { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.PanNo")]
        public string PanNO { get; set; }

        [LIMSResourceDisplayName("Admin.Common.CitizenshipNo")]
        public string CitizenshipNo { get; set; }
        [LIMSResourceDisplayName("Admin.Common.IssueDate")]
        public string IssueDate { get; set; }
        [LIMSResourceDisplayName("Admin.Common.IssueDistrict")]
        public string IssueDistrict { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.RegistrationDate")]
        public string RegisteredDate { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.Phone")]
        public string Phone { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.Email")]
        public string Email { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.Url")]
        public string URL { get; set; }

        [LIMSResourceDisplayName("Admin.Common.Provience")]
        public string Provience { get; set; }

        [LIMSResourceDisplayName("Admin.Common.District")]
        public string District { get; set; }

        [LIMSResourceDisplayName("Admin.Common.LocalLevel")]
        public string LocalLevel { get; set; }

        [LIMSResourceDisplayName("Admin.Common.Ward")]
        public string Ward { get; set; }

        [LIMSResourceDisplayName("Admin.Common.Tole")]
        public string Tole { get; set; }

        [LIMSResourceDisplayName("Admin.Common.Latitude")]
        public string Latitude { get; set; }

        [LIMSResourceDisplayName("Admin.Common.Longitude")]
        public string Longitude { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.NatureOfWork")]
        public string NatureOfWork { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.Proposes")]
        public List<string> Proposes { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.MunicipalityTaxIdentificationNumber")]
        public string MunicipalityTaxIdentificationNumber { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.FarmerEthinicity")]
        public string FarmerEthnicity { get; set; }
       
        [UIHint("Picture")]
        [LIMSResourceDisplayName("Admin.Farm.PanCertificate")]
        public string PanCertificate { get; set; }

        [UIHint("Picture")]
        [LIMSResourceDisplayName("Admin.Farm.RegistrationCertificate")]
        public string RegistrationCertificate { get; set; }

        [UIHint("Picture")]
        [LIMSResourceDisplayName("Admin.Farm.TaxClearenceCertificate")]
        public string TaxClearanceCertificate { get; set; }

        [UIHint("Picture")]
        [LIMSResourceDisplayName("Admin.Farm.Citizenship")]
        public string Citizenship { get; set; }

        [UIHint("Picture")]
        [LIMSResourceDisplayName("Admin.Farm.Image")]
        public string Image { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.Eduacation")]
        public string EducationQualification { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.Job")]
        public bool? ForeignJobExperience { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.Road")]
        public bool? RoadFacility { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.DrinkingWater")]
        public bool? DrinkingWaterFacility { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.Electricity")]
        public bool? ElectricityFacility { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.DistanceFromRoad")]
        public string DistanceFromRoad { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.DistanceFromHighway")]
        public string DistanceFromHighway { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.HouseNear50M")]
        public string HouseNear50M { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.HouseNear100M")]
        public string HouseNear100M { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.PPRS")]
        public bool Pprs { get; set; }

        //pictures
        public FarmPictureModel AddPictureModel { get; set; }
        public FarmGrassModel AddFarmGrass { get; set; }
        public FarmShedModel AddFarmShed { get; set; }


        public IList<FarmPictureModel> FarmPictureModels { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.MoblileNo")]

        public string MoblileNo { get; set; }

        [LIMSResourceDisplayName("Admin.Farm.NoOfMember")]

        public string NoOfMember { get; set; }
        [LIMSResourceDisplayName("Admin.Farm.NoOfDalitMember")]
        public string NoOfDalitMember { get; set; }
        [LIMSResourceDisplayName("Admin.Farm.NoOfFemaleMemeber")]

        public string NoOfFemaleMember { get; set; }
        [LIMSResourceDisplayName("Admin.Farm.LandOwnershipType")]

        public string LandOwnershipType { get; set; }
        [LIMSResourceDisplayName("Admin.Farm.Own")]

        public decimal Own { get; set; }
        [LIMSResourceDisplayName("Admin.Farm.Lease")]

        public decimal Lease { get; set; }
        [LIMSResourceDisplayName("Admin.Farm.LandAreaUnit")]

        public string LandAreaUnit { get; set; }
        [LIMSResourceDisplayName("Admin.Farm.MarketType")]

        public string MarketType { get; set; }
        [LIMSResourceDisplayName("Admin.Farm.Buyer")]

        public string Buyer { get; set; }
        public partial class FarmPictureModel : BaseEntityModel
        {
            public string FarmId { get; set; }

            [UIHint("Picture")]
            [LIMSResourceDisplayName("Admin.Livestock.Farms.Pictures.Fields.Picture")]
            public string PictureId { get; set; }

            [LIMSResourceDisplayName("Admin.Livestock.Farms.Pictures.Fields.Picture")]
            public string PictureUrl { get; set; }

            [LIMSResourceDisplayName("Admin.Livestock.Farms.Pictures.Fields.DisplayOrder")]
            public int DisplayOrder { get; set; }

            [LIMSResourceDisplayName("Admin.Livestock.Farms.Pictures.Fields.OverrideAltAttribute")]

            public string OverrideAltAttribute { get; set; }

            [LIMSResourceDisplayName("Admin.Livestock.Farms.Pictures.Fields.OverrideTitleAttribute")]

            public string OverrideTitleAttribute { get; set; }
        }
        public partial class FarmShedModel : BaseEntityModel
        {
            public string FarmId { get; set; }
            [LIMSResourceDisplayName(" admin.livestock.farms.shed.fields.Name")]
            public string Name { get; set; }

            [LIMSResourceDisplayName(" admin.livestock.farms.shed.fields.Type")]
            public string Type { get; set; }
            [LIMSResourceDisplayName(" admin.livestock.farms.shed.fields.Length")]
            public string Length { get; set; }
            [LIMSResourceDisplayName(" admin.livestock.farms.shed.fields.Bredth")]
            public string Bredth { get; set; }
            [LIMSResourceDisplayName(" admin.livestock.farms.shed.fields.Height")]
            public string Height { get; set; }
            [LIMSResourceDisplayName(" admin.livestock.farms.shed.fields.Volume")]
            public string Volume { get; set; }
            [UIHint("date")]
            [LIMSResourceDisplayName(" admin.livestock.farms.shed.fields.ConstructedDate")]
            public DateTime ConstructedDate { get; set; }
        }
        public partial class FarmGrassModel : BaseEntityModel
        {
            public string FarmId { get; set; }
            [LIMSResourceDisplayName(" admin.livestock.farms.Grass.fields.Type")]
            public string Type { get; set; }
            [LIMSResourceDisplayName(" admin.livestock.farms.Grass.fields.TotalArea")]
            public string TotalArea { get; set; }
            [LIMSResourceDisplayName(" admin.livestock.farms.Grass.fields.GrassName")]
            public string GrassName { get; set; }
            [LIMSResourceDisplayName(" admin.livestock.farms.Grass.fields.NoOfTree")]
            public string NoOfTree { get; set; }
            [LIMSResourceDisplayName(" admin.livestock.farms.Grass.fields.Season")]
            public string Season { get; set; }

        }
    }
}
