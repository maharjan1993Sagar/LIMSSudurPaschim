using LIMS.Api.Models;

namespace LIMS.Api.DTOs.AINR
{
    public class BreedDto : BaseApiEntityModel
    {
        public string NepaliName { get; set; }

        public string EnglishName { get; set; }

        public string BreedType { get; set; }

        public SpeciesDto Species { get; set; }
    }
}
