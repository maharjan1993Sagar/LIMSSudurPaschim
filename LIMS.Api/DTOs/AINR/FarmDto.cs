using LIMS.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.AINR
{
     public class FarmDto: BaseApiEntityModel
    {
        public string Category { get; set; }//farmerorfarm

        public string FarmType { get; set; }//for farm or farmer public private semi-public or individual

        public string NameEnglish { get; set; }

        public string NameNepali { get; set; }

        public string RegNo { get; set; }

       
        public string PanNO { get; set; }

    
        public string CitizenshipNo { get; set; }

       
        public string RegisteredDate { get; set; }

        public string Phone { get; set; }

       
        public string Email { get; set; }

        public string URL { get; set; }

        public string Province { get; set; }

        public string District { get; set; }

        public string LocalLevel { get; set; }


        public string Ward { get; set; }

       
        public string Tole { get; set; }

       
        public string Latitude { get; set; }

       
        public string Longitude { get; set; }

      
        public string NatureOfWork { get; set; }

       
        public string MunicipalityTaxIdentificationNumber { get; set; }

       
        public string FarmerEthnicity { get; set; }

        public bool? ForeignJobExperience { get; set; }

     
        public bool? RoadFacility { get; set; }


        public bool? DrinkingWaterFacility { get; set; }

     
        public bool? ElectricityFacility { get; set; }

      
        public string DistanceFromRoad { get; set; }

        
        public string DistanceFromHighway { get; set; }

        
        public string HouseNear50M { get; set; }

        public string HouseNear100M { get; set; }

        public bool Pprs { get; set; }

    }
}
