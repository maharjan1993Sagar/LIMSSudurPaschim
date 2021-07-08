using LIMS.Domain.NewsEvent;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.GeneralCMS

{
   public class PageContent:BaseEntity
    {
        public NewsEventFile _pageContentFile;
        public PageContent()
        {
            this.PageContentId = Guid.NewGuid();
        }
        public Guid PageContentId { get; set; }
        public string PageName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool ShowImage { get; set; }
        public string UserId {get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public NewsEventFile PageContentFile {
            get { return _pageContentFile ?? (_pageContentFile = new NewsEventFile()); }
            set { _pageContentFile = value; }
        }
    }
}
