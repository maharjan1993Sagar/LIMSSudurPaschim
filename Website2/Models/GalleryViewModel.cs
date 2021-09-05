using LIMS.Core.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Website1.Models
{
    public class GalleryViewModel
    {
        public List<GalleryModel> Galleries { get; set; }
        public List<GalleryModel> Videos { get; set; }
    }
}