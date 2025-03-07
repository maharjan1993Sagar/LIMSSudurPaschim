﻿using LIMS.Api.DTOs.AINR;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.Commands.Models.Ainr
{
    public  class AddFarmCommand : IRequest<FarmDto>
    {
        public  FarmDto FarmDto{ get;set;}
    }
}
