using LIMS.Domain.NewsEvent;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.GeneralCMS

{
   public class Banner:BaseEntity
    {
        public NewsEventFile _image;

        public Banner()
        {
            this.BannerId = Guid.NewGuid();
        }
        public Guid BannerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId {get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual NewsEventFile Image {
            get { return _image ?? (_image = new NewsEventFile()); }
            set { _image = value; }
        }
    }
}
