using AutoMapper.Configuration.Conventions;
using LIMS.Api.Models;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.AnimalHealth
{
   public class VaccineDto: BaseApiEntityModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
