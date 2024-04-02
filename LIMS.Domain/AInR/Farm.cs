using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.AInR
{
    public class Farm : BaseEntity
    {
        private ICollection<FarmPicture> _farmPictures;
        private ICollection<FarmGrass> _farmGrass;
        private ICollection<FarmShed> _farmSheds;
        public Farm()
        {
            this.FarmId = Guid.NewGuid();
        }
        public Guid FarmId { get; set; }
        public string Category { get; set; }//farmerorfarm
        public string FarmType { get; set; }//for farm or farmer public private semi-public or individual
        public string NameEnglish { get; set; }
        public string NameNepali { get; set; }
        public string RegNo { get; set; }
        public string PanNO { get; set; }
        public string CitizenshipNo { get; set; }
        public string IssueDate { get; set; }
        public string IssueDistrict { get; set; }
        public string RegisteredDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string URL { get; set; }
        public string Provience { get; set; }
        public string District { get; set; }
        public string LocalLevel { get; set; }
        public string Ward { get; set; }
        public string Tole { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string NatureOfWork { get; set; }
        public List<string> Proposes { get; set; }
        public string MunicipalityTaxIdentificationNumber { get; set; }
        public string FarmerEthnicity { get; set; }
        public string PanCertificateId { get; set; }
        public string RegistrationCertificateId { get; set; }
        public string TaxClearanceCertificateId { get; set; }
        public string CitizenshipId { get; set; }
        public string ImageId { get; set; }
        public string EducationQualification { get; set; }
        public bool? ForeignJobExperience { get; set; }
        public bool? RoadFacility { get; set; }
        public bool? DrinkingWaterFacility { get; set; }
        public bool? ElectricityFacility { get; set; }
        public string DistanceFromRoad { get; set; }
        public string DistanceFromHighway { get; set; }
        public string HouseNear50M { get; set; }
        public string HouseNear100M { get; set; }
        public bool PPRS { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
        public string MobileNo { get; set; }
        public string Source { get; set; }
        public string NoOfMember { get; set; }
        public string NoOfDalitMember { get; set; }
        public string NoOfFemaleMember { get; set; }

        public string LandOwnershipType { get; set; }
        public decimal Own { get; set; }
        public decimal Lease { get; set; }
        public string LandAreaUnit { get; set; }
        public string MarketType { get; set; }
        public string Buyer { get; set; }

        /// <summary>
        /// Gets or sets the collection of FarmPicture
        /// </summary>
        public virtual ICollection<FarmPicture> FarmPictures {
            get { return _farmPictures ?? (_farmPictures = new List<FarmPicture>()); }
            protected set { _farmPictures = value; }
        }

        public virtual ICollection<FarmGrass> FarmGrasses {
            get { return _farmGrass ?? (_farmGrass = new List<FarmGrass>()); }
            protected set { _farmGrass = value; }
        }
        public virtual ICollection<FarmShed> FarmSheds {
            get { return _farmSheds ?? (_farmSheds = new List<FarmShed>()); }
            protected set { _farmSheds = value; }
        }

    }
}
