using LIMS.Domain.NewsEvent;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.GeneralCMS

{
   public class Gallery:BaseEntity
    {
         public ICollection<NewsEventFile> _images;

        public Gallery()
        {
            this.GalleryId = Guid.NewGuid();
        }
        public Guid GalleryId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string VideoUrl { get; set; }
        public string UserId {get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual ICollection<NewsEventFile> Images {
            get { return _images ?? (_images = new List<NewsEventFile>()); }
            set { _images = value; }
        }
    }
}
