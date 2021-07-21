using LIMS.Core.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Website1.Models
{
    public class GalleryModel
    {
        public string Id { get; set; }
        public Guid GalleryId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string VideoUrl { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<NewsEventFileModel> Images { get; set; }
    }




}