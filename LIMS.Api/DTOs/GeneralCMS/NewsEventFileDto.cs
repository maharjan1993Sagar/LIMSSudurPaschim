namespace LIMS.Api.DTOs.GeneralCMS
{
    public class NewsEventFileDto
    {
        public string CMSEntityId { get; set; }
        public string PictureId { get; set; }

        public string PictureUrl { get; set; }

        public int DisplayOrder { get; set; }

         public string OverrideAltAttribute { get; set; }

         public string OverrideTitleAttribute { get; set; }
        public string Pic { get; set; }
        public string Type { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public decimal FileSize { get; set; }

    }
}