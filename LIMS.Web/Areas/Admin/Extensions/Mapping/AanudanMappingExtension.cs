﻿using LIMS.Domain.Bali;
using LIMS.Web.Areas.Admin.Models.Bali;
using LIMS.Web.Areas.Admin.Models.Bali.Aanudan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class AanudanMappingExtension
    {
        public static AanudanModel ToModel(this AanudanKokaryakram entity)
        {
            return entity.MapTo<AanudanKokaryakram, AanudanModel>();
        }

        public static AanudanKokaryakram ToEntity(this AanudanModel model)
        {
            return model.MapTo<AanudanModel, AanudanKokaryakram>();
        }
        public static PujigatKharchaKharakram ToEntity(this PugigatKharchaKaryakramModel model)
        {
            return model.MapTo<PugigatKharchaKaryakramModel, PujigatKharchaKharakram>();
        }
        public static PugigatKharchaKaryakramModel ToModel(this PujigatKharchaKharakram model)
        {
            return model.MapTo<PujigatKharchaKharakram,PugigatKharchaKaryakramModel > ();
        }
        public static PujigatKharchaKharakram ToEntity(this PugigatKharchaKaryakramModel model, PujigatKharchaKharakram destination)
        {
            return model.MapTo(destination);
        }
        public static AanudanKokaryakram ToEntity(this AanudanModel model, AanudanKokaryakram destination)
        {
            return model.MapTo(destination);
        }
    }
}
